using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class CHelpers
{
    public static RotateFlipType GetOrientationToFlipType(int orientationValue)
    {
        RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone;

        switch (orientationValue)
        {
            case 1:
                rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                break;
            case 2:
                rotateFlipType = RotateFlipType.RotateNoneFlipX;
                break;
            case 3:
                rotateFlipType = RotateFlipType.Rotate180FlipNone;
                break;
            case 4:
                rotateFlipType = RotateFlipType.Rotate180FlipX;
                break;
            case 5:
                rotateFlipType = RotateFlipType.Rotate90FlipX;
                break;
            case 6:
                rotateFlipType = RotateFlipType.Rotate90FlipNone;
                break;
            case 7:
                rotateFlipType = RotateFlipType.Rotate270FlipX;
                break;
            case 8:
                rotateFlipType = RotateFlipType.Rotate270FlipNone;
                break;
            default:
                rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                break;
        }
        
        return rotateFlipType;
    }

    public static double ForceConvertToDbl(object val)
    {
        try
        {
            return Convert.ToDouble(val);
        }
        catch
        {
            return 0;
        }
    }

    public static string Format(object val)
    {
        return Convert.ToDouble(val).ToString("0.000");
    }

    public static string ForceFormat(object val)
    {
        try
        {
            var a = 
            val.ToString().Replace("P", "").Replace("p", "").Replace(" ", "").Replace("h", "").Replace("H", "");

            return Convert.ToDecimal(a).ToString("N2");
        }
        catch
        {
            return val.ToString();
        }
    }

    public static string FormatDate(string date)
    {
        return Convert.ToDateTime(date).ToString("MM/dd/yyyy");
    }

    public static string ConvertImageTo64BaseString(Image image)
    {
        try
        {
            using (MemoryStream m = new MemoryStream())
            {
                image.Save(m, image.RawFormat);
                byte[] imageBytes = m.ToArray();

                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
        catch
        {
            return "";
        }
    }

    public static int MaxAvatarSize { get { return 100; } }

    public static int StandardAvatarHeightWidth { get { return 320; } }

    public static short StandardAvatarBitDepth { get { return 32; } }

    public static void ReduceImageSizeAndSave(string filename, Image image)
    {
        foreach (var prop in image.PropertyItems)
        {
            if (prop.Id == 0x0112) //value of EXIF
            {
                int orientationValue = image.GetPropertyItem(prop.Id).Value[0];
                RotateFlipType rotateFlipType = GetOrientationToFlipType(orientationValue);
                image.RotateFlip(rotateFlipType);
                break;
            }
        }

        int baseScale = image.Width > image.Height ? image.Width : image.Height;
        double scaleFactor  = StandardAvatarHeightWidth.ToDouble() / baseScale.ToDouble();
        
        // width of image as we want
        var newWidth = (int)(image.Width * scaleFactor);
        // height of image as we want  
        var newHeight = (int)(image.Height * scaleFactor);

        var thumbnailImg = new Bitmap(newWidth, newHeight);
        var thumbGraph = Graphics.FromImage(thumbnailImg);
        thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
        thumbGraph.DrawImage(image, imageRectangle);

        Encoder myEncoder;
        myEncoder = Encoder.ColorDepth;

        EncoderParameter myEncoderParameter;
        myEncoderParameter =
            new EncoderParameter(myEncoder, StandardAvatarBitDepth);

        EncoderParameters myEncoderParameters = new EncoderParameters(1);
        myEncoderParameters.Param[0] = myEncoderParameter;

        ImageCodecInfo myImageCodecInfo;
        myImageCodecInfo = GetEncoderInfo("image/png");

        thumbnailImg.Save(filename, myImageCodecInfo, myEncoderParameters);
    }

    public static Image ReduceImageSizeAndSave(Image image)
    {
        foreach (var prop in image.PropertyItems)
        {
            if (prop.Id == 0x0112) //value of EXIF
            {
                int orientationValue = image.GetPropertyItem(prop.Id).Value[0];
                RotateFlipType rotateFlipType = GetOrientationToFlipType(orientationValue);
                image.RotateFlip(rotateFlipType);
                break;
            }
        }

        int baseScale = image.Width > image.Height ? image.Width : image.Height;
        double scaleFactor = StandardAvatarHeightWidth.ToDouble() / baseScale.ToDouble();

        // width of image as we want
        var newWidth = (int)(image.Width * scaleFactor);
        // height of image as we want  
        var newHeight = (int)(image.Height * scaleFactor);

        var thumbnailImg = new Bitmap(newWidth, newHeight);
        var thumbGraph = Graphics.FromImage(thumbnailImg);
        thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
        thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
        thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
        var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
        thumbGraph.DrawImage(image, imageRectangle);

        Encoder myEncoder;
        myEncoder = Encoder.ColorDepth;

        EncoderParameter myEncoderParameter;
        myEncoderParameter =
            new EncoderParameter(myEncoder, StandardAvatarBitDepth);

        EncoderParameters myEncoderParameters = new EncoderParameters(1);
        myEncoderParameters.Param[0] = myEncoderParameter;

        ImageCodecInfo myImageCodecInfo;
        myImageCodecInfo = GetEncoderInfo("image/png");

        var ms = new MemoryStream();
        thumbnailImg.Save(ms, myImageCodecInfo, myEncoderParameters);

        return Image.FromStream(ms);
    }

    public static ImageCodecInfo GetEncoderInfo(string mimeType)
    {
        int j;
        ImageCodecInfo[] encoders;
        encoders = ImageCodecInfo.GetImageEncoders();
        for (j = 0; j < encoders.Length; ++j)
        {
            if (encoders[j].MimeType == mimeType)
                return encoders[j];
        }
        return null;
    }

    public static string ToTitleString(string val)
    {
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        return textInfo.ToTitleCase(val.ToLower()).Replace("Na", "na").Replace("At", "at");
    }

    public static string GetTagalogPrefix(int sex_id, int civilsatus_id)
    {
        if (sex_id == 1)
        {
            return "Ginoong";
        }
        else if(sex_id == 2)
        {
            if (civilsatus_id == 2)
            {
                return "Ginang";
            }
            else
            {
                return "Binibini";
            }
        }
        else
        {
            return "";
        }
    }

    public static string ForceFormat(DateTime date, string format)
    {
        return date.ToString(format);
    }

    public static string NumberToWords(int number)
    {
        if (number == 0)
            return "zero";

        if (number < 0)
            return "minus " + NumberToWords(Math.Abs(number));

        string words = "";

        if ((number / 1000000) > 0)
        {
            words += NumberToWords(number / 1000000) + " million ";
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            words += NumberToWords(number / 1000) + " thousand ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += NumberToWords(number / 100) + " hundred ";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "and ";

            var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += "-" + unitsMap[number % 10];
            }
        }

        return words;
    }
}

public static class ObjectExtentions
{
    public static int ForceConvertToInt32(this object val)
    {
        try
        {
            return Convert.ToInt32(val);
        }
        catch
        {
            return 0;
        }
    }

    public static int? ForceConvertToInt32Nullable(this object val)
    {
        try
        {
            return Convert.ToInt32(val);
        }
        catch
        {
            return null;
        }
    }

    public static byte? ForceConvertToByteNullable(this object val)
    {
        try
        {
            return Convert.ToByte(val);
        }
        catch
        {
            return null;
        }
    }

    #region DataType Convertion
    /// <summary>
    /// Convert object to byte
    /// </summary>
    /// <returns>byte</returns>
    public static byte ToByte(this object obj)
    {
        return Convert.ToByte(obj);
    }
    /// <summary>
    /// Convert object to Int16.
    /// </summary>
    /// <returns>Int16</returns>
    public static short ToShort(this object obj)
    {
        return Convert.ToInt16(obj);
    }
    /// <summary>
    /// Convert object to Int32.
    /// </summary>
    /// <returns>Int32</returns>
    public static int ToInt(this object obj)
    {
        return Convert.ToInt32(obj);
    }
    /// <summary>
    /// Convert object to Int64
    /// </summary>
    /// <returns>long</returns>
    public static long ToLong(this object obj)
    {
        return Convert.ToInt64(obj);
    }

    /// <summary>
    /// Convert object to double
    /// </summary>
    /// <returns>double</returns>
    public static double ToDouble(this object obj)
    {
        return Convert.ToDouble(obj);
    }

    /// <summary>
    /// Convert object to decimal
    /// </summary>
    /// <returns>decimal</returns>
    public static decimal ToDecimal(this object obj)
    {
        return Convert.ToDecimal(obj);
    }

    /// <summary>
    /// Convert object to float
    /// </summary>
    /// <returns>float</returns>
    public static float ToFloat(this object obj)
    {
        return (float)obj;
    }

    /// <summary>
    /// Convert object to DateTime
    /// </summary>
    /// <param name="obj">Value to convert</param>
    /// <returns>DateTime</returns>
    public static DateTime ToDateTime(this object obj)
    {
        return Convert.ToDateTime(obj);
    }

    /// <summary>
    /// Divert value base on the provided oldValue
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    /// <returns></returns>
    public static object DivertValue(this object obj, object oldValue, object newValue)
    {
        if (obj.ToString() == oldValue.ToString())
            return newValue;
        else
            return obj;
    }

    public enum FromDateFormat { SLMMddyyyy, DSMMddyyy };
    /// <summary>
    /// Convert object to DateTime
    /// </summary>
    /// <param name="obj">Value to convert</param>
    /// <returns>DateTime</returns>
    public static DateTime? ToDateTime(this object obj, FromDateFormat fromFormat)
    {
        try
        {
            var strD = obj.ToString();
            switch (fromFormat)
            {
                case FromDateFormat.SLMMddyyyy:
                    strD = $"{strD.Split('/')[2]}-{strD.Split('/')[0]}-{strD.Split('/')[1]}";
                    break;
                case FromDateFormat.DSMMddyyy:
                    strD = $"{strD.Split('-')[2]}-{strD.Split('-')[0]}-{strD.Split('-')[1]}";
                    break;
            }
            return Convert.ToDateTime(strD);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Convert object to Nullable DateTime
    /// </summary>
    /// <param name="obj">Value to convert</param>
    /// <returns>DateTime</returns>
    public static DateTime? ToNullableDateTime(this object obj)
    {
        try
        {
            return Convert.ToDateTime(obj);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Convert object to DateTime
    /// </summary>
    /// <param name="obj">Value to convert</param>
    /// <returns>Boolean</returns>
    public static bool ToBoolean(this object obj)
    {
        return Convert.ToBoolean(obj);
    }

    public static string TryParseDate(this object obj)
    {
        try
        {
            return Convert.ToDateTime(obj).ToString("yyyy-MM-dd");
        }
        catch
        {
            return obj.ToString();
        }
    }
    #endregion

    public static string ConvertToString(this PartialViewResult partialView,
                                              ControllerContext controllerContext)
    {
        using (var sw = new StringWriter())
        {
            partialView.View = ViewEngines.Engines
              .FindPartialView(controllerContext, partialView.ViewName).View;

            var vc = new ViewContext(
              controllerContext, partialView.View, partialView.ViewData, partialView.TempData, sw);
            partialView.View.Render(vc, sw);

            var partialViewString = sw.GetStringBuilder().ToString();

            return partialViewString;
        }
    }
}