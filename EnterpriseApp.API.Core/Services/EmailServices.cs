using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using AutoMapper;
using EnterpriseApp.API.Core.Exceptions;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Models.Common;
using EnterpriseApp.API.Models.Entity;
using EnterpriseApp.API.Models.ViewModels;

namespace EnterpriseApp.API.Core.Services
{
    public interface IEmailServices
    {
        ForgotPasswordResponse ForgotPassword(ForgotPasswordRequest request);
        void ProblemSendMail(ResCommentRequest model);
    }

    public class EmailServices : BaseService, IEmailServices
    {
        public EmailServices(FirestoreRepository dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public ForgotPasswordResponse ForgotPassword(ForgotPasswordRequest request)
        {
            User user = _dbContext.Users.SingleOrDefault(x => x.UEmail.Equals(request.UEmail, StringComparison.InvariantCultureIgnoreCase));

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            if (user.Status != (int)WellKnownStatus.Active)
                throw new ApplicationDataException(StatusCode.ERROR_UserDeactivated);

            var otp = GenerateRandomOTP(6);

            user.UserOtp = Convert.ToInt32(otp);
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();

            var template = "<div style=font-family:sans-serif;color:#3e4951;padding:40px;width:600px;margin-left:auto;margin-right:auto;border:0.5px;border-color:#00347F;border-style:solid;margin-top:0px><h1 style=color:black;margin-top:0>EnterpriseApp</h1><hr style=border:.5px;border-color:#00347F;background-color:#00347F;border-style:solid;><div style=padding-top:15px;padding-bottom:2px;><p>Bonjour {{userName}},<h3>Votre code de vérification est le: {{code}}</h3> Pour toute question concernant votre compte, n’hésitez pas à entrer en contact avec nous.</div><div><p> Sincèrement, <br> <a style=text-decoration:none; href={{CompanyWeb}}>EnterpriseApp</a> <br></p></div><hr style=border:.5px;border-color:#00347F;background-color:#00347F;border-style:solid;><p style=text-align:center;color:#888> <small>© {{Year}} EnterpriseApp. All Rights Reserved.</small> <br> <small>25 rue Arthur Rozier 75019 Paris</small></p></div>";
            var userName = user.UFirstName + " " + user.ULastName;

            var temp = template.Replace("{{userName}}", userName)
                               .Replace("{{Year}}", DateTime.Now.Year.ToString())
                               .Replace("{{code}}", otp);

            MailMessage message = new MailMessage();

            message.To.Add(new MailAddress(user.UEmail, userName));
            message.From = new MailAddress("admin@locatieApp.io", "EnterpriseApp");
            message.Subject = "OTP";
            message.Body = temp;
            message.IsBodyHtml = true;

            SmtpClient smtpServer = new SmtpClient("ssl0.ovh.net");
            smtpServer.Port = 587;
            smtpServer.Credentials = new NetworkCredential("admin@locatieApp.io", "adminBuddhika") as ICredentialsByHost;
            smtpServer.EnableSsl = true;

            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };

            smtpServer.Send(message);

            return new ForgotPasswordResponse()
            {
                IsSuccessful = true,
            };
        }

        public void ProblemSendMail(ResCommentRequest model)
        {
            User user = _dbContext.Users.SingleOrDefault(x => x.Id == model.UserId);

            var article = _dbContext.Articles.SingleOrDefault(r => r.Id == model.ArticleId);

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            if (user.Status != (int)WellKnownStatus.Active)
                throw new ApplicationDataException(StatusCode.ERROR_UserDeactivated);

            var otp = GenerateRandomOTP(6);

            user.UserOtp = Convert.ToInt32(otp);
            _dbContext.Users.Update(user);
            _dbContext.SaveChanges();

            var template = "<div style=font-family:sans-serif;color:#3e4951;padding:40px;width:600px;margin-left:auto;margin-right:auto;border:0.5px;border-color:#00347F;border-style:solid;margin-top:0px> <h1 style=color:black;margin-top:0>EnterpriseApp</h1> <hr style=border:.5px;border-color:#00347F;background-color:#00347F;border-style:solid;> <div style=padding-top:15px;padding-bottom:2px;> <p>Bonjour,{{userName}}has notified the followking issues for article{{rName}}:{{issues}}</div><div> <p> Sincèrement, <br><a style=text-decoration:none; href={{CompanyWeb}}>EnterpriseApp</a> <br></p></div><hr style=border:.5px;border-color:#00347F;background-color:#00347F;border-style:solid;> <p style=text-align:center;color:#888> <small>©{{Year}}EnterpriseApp. All Rights Reserved.</small> <br><small>25 rue Arthur Rozier 75019 Paris</small></p></div>";
            var userName = user.UFirstName + " " + user.ULastName;

            string issues = "<br>";

            foreach (var rProblem in model.RProblems)
            {
                issues += $"- {rProblem} <br>";
            }


            var temp = template.Replace("{{userName}}", userName)
                               .Replace("{{Year}}", DateTime.Now.Year.ToString())
                               .Replace("{{issues}}", issues)
                               .Replace("{{rName}}", article.RName);

            MailMessage message = new MailMessage();

            message.To.Add(new MailAddress("infos@locatieApp.io", "EnterpriseApp"));
            message.From = new MailAddress("admin@locatieApp.io", "EnterpriseApp");
            message.Subject = "User has notified problems";
            message.Body = temp;
            message.IsBodyHtml = true;

            SmtpClient smtpServer = new SmtpClient("ssl0.ovh.net");
            smtpServer.Port = 587;
            smtpServer.Credentials = new NetworkCredential("admin@locatieApp.io", "adminBuddhika") as ICredentialsByHost;
            smtpServer.EnableSsl = true;

            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };

            smtpServer.Send(message);
        }

        private string GenerateRandomOTP(int iOTPLength)
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sOTP = String.Empty;
            string sTempChars = String.Empty;

            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }

            return sOTP;
        }
    }
}
