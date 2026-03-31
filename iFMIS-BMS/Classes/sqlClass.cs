//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Data;
//using System.Data.SqlClient;
//using System.Security;
//using System.Security.Cryptography;
//using System.Text;
//using System.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using iFMIS_BMS.Models;

namespace iFMIS_BMS.Classes
{
    public struct FUNCTION{

        public static string GeneratePISControl()
        {
            var EncryptPISNo = UniqueKey(8);
            return EncryptPISNo;
        }
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
        
    } 




}