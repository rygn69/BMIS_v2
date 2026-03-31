using iFMIS_BMS.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using System.Web.Script.Serialization;

public class Account
{
    public static UserModel UserInfo
    {
        get
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            HttpContext ctx = HttpContext.Current;            
                var decrypt = FormsAuthentication.Decrypt(cookie.Value);
                UserModel data = (UserModel)(new JavaScriptSerializer().Deserialize(decrypt.UserData, typeof(UserModel)));

                if (data != null)
                {
                    return data;
                }
                else
                {
                    return null;
                }
             
        }
    }
}