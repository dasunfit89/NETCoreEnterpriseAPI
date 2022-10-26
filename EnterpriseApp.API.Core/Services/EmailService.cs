using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using EnterpriseApp.API.Core.Exceptions;
using EnterpriseApp.API.Core.Services;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Data.Entity;
using EnterpriseApp.API.Data.ViewModels;
using Microsoft.Extensions.Options;

namespace EnterpriseApp.API.Core.Services
{
    public interface IEmailService
    {
        Task<SendEmailResponse> EmailOTP(SendEmailRequest request);
        string GenerateRandomOTP(int iOTPLength);
    }

    public class EmailService : BaseService, IEmailService
    {
        private readonly IFileService _fileService;

        public EmailService(IRepository dbContext, IFileService fileService, IMapper mapper, IOptions<AppSettings> appSettings) :
            base(dbContext, mapper, appSettings)
        {
            _fileService = fileService;
        }

        public async Task<SendEmailResponse> EmailOTP(SendEmailRequest request)
        {
            User user = await _dbContext.FilterOneAsync<User>(x => x.Email.ToLower() == request.Email.ToLower());

            if (user == null)
                throw new ApplicationDataException(StatusCode.ERROR_InvalidUser);

            string otp = user.OTP;

            var template = @"<div style=font-family:sans-serif;color:#3e4951;padding:40px;width:600px;margin-left:auto;margin-right:auto;border:0.5px;border-color:#00347F;border-style:solid;margin-top:0px>
                           <h1 style=color:black;margin-top:0>SLFP දෑත</h1>
                           <hr style=border:.5px;border-color:#00347F;background-color:#00347F;border-style:solid;>
                           <div style=padding-top:15px;padding-bottom:2px;>
                              <p>Hello {{userName}},
                              <h3>Your verification code is : {{code}}</h3>
                              For any questions regarding your account, please do not hesitate to contact us.
                           </div>
                           <div>
                              <p> Sincerely, <br> <a style=text-decoration:none; href={{CompanyWeb}}>SLFP දෑත</a> <br></p>
                           </div>
                           <hr style=border:.5px;border-color:#00347F;background-color:#00347F;border-style:solid;>
                           <p style=text-align:center;color:#888> <small>© {{Year}} SLFP දෑත. All Rights Reserved.</small> <br> <small>ශ්‍රී ලංකා නිදහස් පක්ෂ මුලස්ථානය, 301 ටි.බි ජයා මාවත, කොලඹ 10, ශ්‍රී ලංකාව</small></p>
                        </div>";

            var userName = user.FirstName + " " + user.LastName;

            var temp = template.Replace("{{userName}}", userName)
                               .Replace("{{Year}}", DateTime.Now.Year.ToString())
                               .Replace("{{code}}", otp)
                               .Replace("{{CompanyWeb}}", "http://www.slfp.lk");


            MailMessage message = new MailMessage();

            message.To.Add(new MailAddress(user.Email, userName));
            message.From = new MailAddress("slfpdetha@zohomail.com", "SLFP Detha");
            message.Subject = "SLFP App-OTP";
            message.Body = temp;
            message.IsBodyHtml = true;

            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };

            using (SmtpClient client = new SmtpClient())
            {
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Host = "smtp.zoho.com";
                client.Port = 587;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("slfpdetha@zohomail.com", "Neverforgetthis123");
                client.Send(message);

            }

            return new SendEmailResponse()
            {
                IsSuccessful = true,
            };
        }
    }
}
