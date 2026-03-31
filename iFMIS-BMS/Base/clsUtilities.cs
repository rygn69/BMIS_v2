using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Telerik.Reporting.Processing;
using iFMIS_BMS.Models;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;

namespace iFMIS_BMS.Base
{
     

    public class UTILITIES
    {

        //[DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);


        public static string IniReadValue(string Section, string Key, string Path)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, Path);

            return temp.ToString();

        }

        public static void IniWriteValue(string Section, string Key, string Value, string Path)
        {
            WritePrivateProfileString(Section, Key, Value, Path);
        }

        // GENERATE UNIQUE CODE
        public static string UniqueKey(int size = 5)
        {
            char[] chars = new char[62];

            chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        // CREATE PDF FILE
        public static void createPDFFile(string FullPath, RenderingResult result)
        {

            MemoryStream ms = new MemoryStream(result.DocumentBytes);

            BinaryReader br = new BinaryReader(ms);
            Byte[] bytes = br.ReadBytes((Int32)ms.Length);

            string strFileToSave = FullPath;
            FileStream file = new FileStream(strFileToSave, FileMode.Create, FileAccess.Write);
            file.Write(bytes, 0, bytes.Length);
            file.Close();
        }

        // CREATE PDF Binary
        public static Byte[] createPDFBinary(RenderingResult result)
        {
            MemoryStream ms = new MemoryStream(result.DocumentBytes);

            BinaryReader br = new BinaryReader(ms);
            Byte[] bytes = br.ReadBytes((Int32)ms.Length);

            return bytes;
        }

        // CREATE UPDATE PDF Binary
        public static Byte[] createPDFBinary(HttpPostedFileBase file)
        {
           
            BinaryReader br = new BinaryReader(file.InputStream);
            Byte[] bytes = br.ReadBytes((Int32)file.InputStream.Length);

            return bytes;
        }

        /// <summary>
        /// Takes a collection of BMP files and converts them into a PDF document
        /// </summary>
        /// <param name="bmpFilePaths"></param>
        /// <returns></returns>
        //public static byte[] CreatePdfFromImage(Image imageFile)
        //{
        //    using (var ms = new MemoryStream())
        //    {
        //        var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER.Rotate(), 0, 0, 0, 0);
        //        iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms).SetFullCompression();
        //        document.Open();

        //        var imgStream = GetImageStream(imageFile);
        //        var image = iTextSharp.text.Image.GetInstance(imgStream);
        //        image.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
        //        document.Add(image);

        //        document.Close();
        //        return ms.ToArray();
        //    }
        //}

        /// <summary>
        /// Gets the image at the specified path, shrinks it, converts to JPG, and returns as a stream
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        private static Stream GetImageStream(Image image)
        {
            var ms = new MemoryStream();
            using (var img = image)
            {
                var jpegCodec = ImageCodecInfo.GetImageEncoders()
                    .Where(x => x.MimeType == "image/jpeg")
                    .FirstOrDefault();

                var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)20);

                int dpi = 175;
                var thumb = img.GetThumbnailImage((int)(11 * dpi), (int)(8.5 * dpi), null, IntPtr.Zero);
                thumb.Save(ms, jpegCodec, encoderParams);
            }
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        // SAVE REPORT GENERATED
        //public static void saveReportLog(int wplist_id, string file_name, string Unique_Code, Byte[] bytes, string docDesc, DateTime tdate)
        //{

        //    using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["attachment_adodb"].ToString()))
        //    {
        //        con.Open();
        //        string strQry = "insert into t_reports_generation_logs(wplist_id,file_name,unique_code,pdf_attachment,doc_brief_description,datetime_generated,generated_by) values (@wplist_id,@file_name,@unique_code, @pdf_attachment,@docDesc,@datetime_generated,@generated_by)";
        //        using (var cmd = new SqlCommand(strQry, con))
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            cmd.CommandTimeout = Int32.MaxValue;

        //            cmd.Parameters.Add("@unique_code", SqlDbType.VarChar).Value = Unique_Code;// Unique_Code.Encrypt();
        //            cmd.Parameters.Add("@pdf_attachment", SqlDbType.Binary).Value = bytes;
        //            cmd.Parameters.Add("@wplist_id", SqlDbType.Int).Value = wplist_id;
        //            cmd.Parameters.Add("@docDesc", SqlDbType.VarChar).Value = docDesc;
        //            cmd.Parameters.Add("@datetime_generated", SqlDbType.DateTime).Value = tdate;
        //            cmd.Parameters.Add("@generated_by", SqlDbType.Int).Value = USER.Data.eid;
        //            cmd.Parameters.Add("@file_name", SqlDbType.VarChar).Value = file_name;



        //            cmd.ExecuteNonQuery();
        //        }

        //        con.Close();
        //    }

        //}

        // RETRIEVE REPORT GENERATED
        public static string retrieveReportLog(DataTable dt, string directory)
        {
            byte[] objData;
            objData = (byte[])dt.Rows[0]["pdf_attachment"];

            string strFileToSave = directory + @"\" + dt.Rows[0]["file_name"].ToString();
            FileStream file = new FileStream(strFileToSave, FileMode.Create, FileAccess.Write);
            file.Write(objData, 0, objData.Length);
            file.Close();

            return strFileToSave;
           
        }

        
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }


       
    }

    


}
