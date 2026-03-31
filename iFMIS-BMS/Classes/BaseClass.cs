using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Web.Mvc;

public class UserDataModelBinder<T> : IModelBinder
{
    public object BindModel(ControllerContext controllerContext,
        ModelBindingContext bindingContext)
    {
        if (bindingContext.Model != null)
            throw new InvalidOperationException("Cannot update instances");
        if (controllerContext.RequestContext.HttpContext.Request.IsAuthenticated)
        {
            var cookie = controllerContext
                .RequestContext
                .HttpContext
                .Request
                .Cookies[FormsAuthentication.FormsCookieName];

            if (null == cookie)
                return null;

            var decrypted = FormsAuthentication.Decrypt(cookie.Value);

            if (!string.IsNullOrEmpty(decrypted.UserData))
                return new JavaScriptSerializer().Deserialize<T>(decrypted.UserData); // .Deserialize<T>(decrypted.UserData);
        }
        return null;
    }
}
public static class HttpResponseBaseExtensions
{
    public static int SetAuthCookie<T>(this HttpResponseBase responseBase, string name, bool rememberMe, T userData)
    {
        /// In order to pickup the settings from config, we create a default cookie and use its values to create a 
        /// new one.
        var cookie = FormsAuthentication.GetAuthCookie(name, rememberMe);
        var ticket = FormsAuthentication.Decrypt(cookie.Value);
        var data = new JavaScriptSerializer().Serialize(userData);

        var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration,
            ticket.IsPersistent, data, ticket.CookiePath);
        var encTicket = FormsAuthentication.Encrypt(newTicket);

        /// Use existing cookie. Could create new one but would have to copy settings over...
        cookie.Value = encTicket;

        responseBase.Cookies.Add(cookie);

        return encTicket.Length;
    }
}