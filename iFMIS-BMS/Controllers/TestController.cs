using iFMIS_BMS.BusinessLayer.Layers.BudgetPreparation;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iFMIS_BMS.BusinessLayer.Models;
using System.Net.Mail;
using iFMIS_BMS.BusinessLayer.Models.Email;

namespace iFMIS_BMS.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ViewResult Index(EmailModel _objModelMail)
        {
            if (ModelState.IsValid)
            {
                string sender = "ranel.cator@pgas.ph";
                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress(sender, "iFMIS - BMS");
                mail.Subject = "Notice Account Amount Changes";
                mail.Body = @"
                <style>
                table {
                    font-family:Tahoma;
                    border-collapse:collapse;
                }
                th {
                    padding: 10px;
                    font-weight:500;
                    background-color:#CEE9E0;
                }
                </style>
                <table border='1'>
                <tr>
                <th> Account Name </th>
                <th> Proposed Amount </th>
                <th> Alloted Amount </th>
                        @{
var x = 0;
                        while(x != 2){
<th> hhe </th>
x++;                        
}
                </tr>
                </table>
                ";
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "192.168.2.101";
                
                //smtp.Port = 587;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new System.Net.NetworkCredential
                //("ranel.cator@pgas.gov", "***********");// Enter seders User name and password
                //smtp.EnableSsl = true;
                smtp.Send(mail);
                return View("Index", _objModelMail);
            }
            else
            {
                return View();
            }
        }
       
        //public JsonResult Proposal_Account([DataSourceRequest] DataSourceRequest request)
        //{
        //    OfficeAdmin_Layer program_list = new OfficeAdmin_Layer();
        //    var program_lst = program_list.grOfficeProgramTEST();
        //    return Json(program_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}


        //public JsonResult Proposal_AccountDenomination([DataSourceRequest] DataSourceRequest request, int? ProposalID)
        //{
        //    OfficeAdmin_Layer program_list = new OfficeAdmin_Layer();
        //    var program_lst = program_list.grAccountDenominationTEST(ProposalID);
        //    return Json(program_lst.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult Insert_AccountDenomination([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<AccountDenomination_Layer> Accounts)
        //{
        //    OfficeAdmin_Layer el = new OfficeAdmin_Layer();
        //    try
        //    {
        //        el.UpdateAccountHome2(Accounts);
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(String.Empty, ex.Message);
        //    }
        //    return Json(Accounts.ToDataSourceResult(request, ModelState));
        //}
        
    }
}