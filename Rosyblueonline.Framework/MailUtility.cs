using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Rosyblueonline.Framework
{
    public class MailAttachment
    {
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }
    }

    public interface IMailUtility
    {
        void SendMail(List<string> ToEmailID, string Subject, bool IsBodyHtml, string Body, List<MailAttachment> attachments = null);
    }

    public class MailUtility
    {
        MailMessage mail = null;
        SmtpClient SmtpServer = null;
        string SMTP = string.Empty, Password = string.Empty, From = string.Empty;
        int PortNo = 25;
        bool EnableSSL = false;

        public MailUtility()
        {
            From = ConfigurationManager.AppSettings["Email_ID"].ToString();
            Password = ConfigurationManager.AppSettings["Email_Password"].ToString();
            SMTP = ConfigurationManager.AppSettings["Email_SMTP"].ToString();
            PortNo = Convert.ToInt32(ConfigurationManager.AppSettings["Email_PORT"].ToString());
            EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["Email_SSL"].ToString());
        }

        public void SendSMTPMailWithMultipleAttachments(string to, string cc, string subject, string body, Attachment[] attachments)
        {
            var mailContent = new MailMessage();
            mailContent.To.Add(to);
            mailContent.IsBodyHtml = true;
            mailContent.Subject = subject;
            mailContent.From = new MailAddress(From);
            mailContent.Priority = MailPriority.High;
            mailContent.Body = body;
            if (attachments != null)
            {
                foreach (Attachment attachment in attachments)
                {
                    mailContent.Attachments.Add(attachment);
                }
            }
            SendMailMessage(mailContent);
        }

        public SmtpClient GetSMTPClient()
        {
            var smtpClient = new SmtpClient
            {
                EnableSsl = false,
                Credentials = new System.Net.NetworkCredential(From, Password),
                Port = PortNo,
                Host = SMTP,
                Timeout = 1000000,
            };

            return smtpClient;
        }

        public void SendMailMessage(MailMessage mailMessage)
        {
            var smtpClient = GetSMTPClient();
            // mailMessage.CC.Add("support@goldeshop.com");
            // mailMessage.Bcc.Add("goldeshopemails@gmail.com");
            try
            {
                //Logger.LogWebActivity("Email Initiated for " + mailMessage.To + " Subject: " + mailMessage.Subject);
                smtpClient.Send(mailMessage);
                //Logger.LogWebActivity("Email Success for " + mailMessage.To + " Subject: " + mailMessage.Subject);
            }
            catch (Exception ex)
            {
                //Logger.LogWebActivity("Email Error for " + mailMessage.To + " Subject: " + mailMessage.Subject + " Exception: " + ex.Message + "InnerException:" + ex.InnerException);
                throw ex;
            }
            finally
            {
                smtpClient.Dispose();
            }

        }

        //public void SendMail(List<string> ToEmailID, string Subject, bool IsBodyHtml, string Body, List<MailAttachment> attachments = null, string CC = null, string BCC = null)
        //{
        //    try
        //    {
        //        bool AllowMails = Convert.ToBoolean(ConfigurationManager.AppSettings["AllowMails"]);
        //        if (AllowMails)
        //        {
        //            mail = new MailMessage();
        //            SmtpServer = new SmtpClient(SMTP);
        //            mail.From = new MailAddress(From);
        //            for (int i = 0; i < ToEmailID.Count; i++)
        //            {
        //                mail.To.Add(ToEmailID[i]);
        //            }
        //            if (string.IsNullOrEmpty(CC))
        //            {
        //                mail.CC.Add(new MailAddress(CC));
        //            }
        //            if (string.IsNullOrEmpty(BCC))
        //            {
        //                mail.Bcc.Add(new MailAddress(BCC));
        //            }
        //            mail.Subject = Subject;
        //            mail.Body = Body;
        //            mail.IsBodyHtml = IsBodyHtml;
        //            //
        //            if (attachments != null && attachments.Count > 0)
        //            {
        //                for (int i = 0; i < attachments.Count; i++)
        //                {
        //                    using (var fs = new MemoryStream(attachments[i].FileBytes))
        //                    {
        //                        mail.Attachments.Add(new Attachment(fs, attachments[i].FileName));
        //                    }
        //                }
        //            }
        //            SmtpServer.Port = PortNo;
        //            SmtpServer.Credentials = new System.Net.NetworkCredential(From, Password);
        //            SmtpServer.EnableSsl = EnableSSL;
        //            SmtpServer.Timeout = 1000000;
        //            SmtpServer.UseDefaultCredentials = false;
        //            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object s,
        //            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
        //            System.Security.Cryptography.X509Certificates.X509Chain chain,
        //            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        //            {
        //                return true;
        //            };
        //            //#if DEBUG
        //            //                    //NEVER_EAT_POISON_Disable_CertificateValidation();
        //            //#endif
        //            SmtpServer.Send(mail);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void SendMail(List<string> ToEmailID, string Subject, bool IsBodyHtml, string Body, List<MailAttachment> attachments = null, string CC = null, string BCC = null)
        {
            //NEVER_EAT_POISON_Disable_CertificateValidation();
            bool AllowMails = Convert.ToBoolean(ConfigurationManager.AppSettings["AllowMails"]);
            mail = new MailMessage();
            SmtpServer = new SmtpClient(SMTP);
            mail.From = new MailAddress(From);
            for (int i = 0; i < ToEmailID.Count; i++)
            {
                if (!string.IsNullOrEmpty(ToEmailID[i]))
                {
                    mail.To.Add(ToEmailID[i]);
                }
            }
            if (!string.IsNullOrEmpty(CC))
            {
                string[] ccs = CC.Split(',');
                for (int i = 0; i < ccs.Length; i++)
                {
                    mail.CC.Add(new MailAddress(ccs[i]));
                }
            }
            if (!string.IsNullOrEmpty(BCC))
            {
                string[] bccs = CC.Split(',');
                for (int i = 0; i < bccs.Length; i++)
                {
                    mail.Bcc.Add(new MailAddress(bccs[i]));
                }
            }
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = IsBodyHtml;
            MemoryStream fs = null;
            if (attachments != null && attachments.Count > 0)
            {
                for (int i = 0; i < attachments.Count; i++)
                {
                    fs = new MemoryStream(attachments[i].FileBytes);
                    mail.Attachments.Add(new Attachment(fs, attachments[i].FileName));
                }
            }
            SmtpServer.Port = PortNo;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential(From, Password);
            SmtpServer.EnableSsl = EnableSSL;
            //SmtpServer.Timeout = 1000000;
            if (AllowMails)
            {
                SmtpServer.Send(mail);
                if (fs != null)
                {
                    fs.Flush();
                    fs.Dispose();
                    fs.Close();
                }
            }
        }


        //[Obsolete("Do not use this in Production code!!!", true)]
        //void NEVER_EAT_POISON_Disable_CertificateValidation()
        //{
        //    // Disabling certificate validation can expose you to a man-in-the-middle attack
        //    // which may allow your encrypted message to be read by an attacker
        //    // https://stackoverflow.com/a/14907718/740639
        //    ServicePointManager.ServerCertificateValidationCallback =
        //        delegate (
        //            object s,
        //            X509Certificate certificate,
        //            X509Chain chain,
        //            SslPolicyErrors sslPolicyErrors
        //        )
        //        {
        //            return true;
        //        };
        //}

        public void SendMail(string ToEmailID, string Subject, bool IsBodyHtml, string Body)
        {
            try
            {

                bool AllowMails = Convert.ToBoolean(ConfigurationManager.AppSettings["AllowMails"]);
                if (AllowMails)
                {
                    mail = new MailMessage();
                    SmtpServer = new SmtpClient(SMTP);
                    mail.From = new MailAddress(From);
                    //for (int i = 0; i < ToEmailID.Count; i++)
                    //{
                    if (!string.IsNullOrEmpty(ToEmailID))
                    {
                        mail.To.Add(ToEmailID);
                    }
                    //}
                    mail.Subject = Subject;
                    mail.Body = Body;
                    mail.IsBodyHtml = IsBodyHtml;

                    SmtpServer.Port = PortNo;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(From, Password);
                    SmtpServer.EnableSsl = EnableSSL;
                    SmtpServer.Timeout = 1000000;
                    SmtpServer.Send(mail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
