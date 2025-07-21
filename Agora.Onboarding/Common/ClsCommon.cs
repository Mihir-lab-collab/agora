using Agora.Onboarding.DAL;
using Agora.Onboarding.Models;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Xml;

namespace Agora.Onboarding.Common
{
    public class ClsCommon
    {
        DbContext _context = new DbContext();
        public ClsCommon()
        {

        }
        public void SendEmail(EmployeeMaster emp)
        {
            MailMessage msg = new MailMessage();
            MailAddress msgFrom = new MailAddress(ConfigurationManager.AppSettings.Get("fromEmail"),
                ConfigurationManager.AppSettings.Get("compName"));
            msg.From = msgFrom;
            //foreach (string emailAddress in emp.EmpAddress.ToString().Trim().Split(','))
            //{
            //    msg.CC.Add(emailAddress);
            //}
            msg.IsBodyHtml = true;
            //msg.Subject = "Agora - Onboarding";
            string toMessage = System.Configuration.ConfigurationManager.AppSettings["HREmail"];
            msg.To.Add(toMessage);
            string subject = string.Empty;
            msg.Body = MessageBody(emp.EmpName, emp.EmpAddress, emp.EmpId, out subject).ToString();
            msg.Subject = subject;
            SmtpClient mailClient = new SmtpClient(ConfigurationManager.AppSettings["SMTP"]);
            mailClient.UseDefaultCredentials = true;
            mailClient.Port = int.Parse(ConfigurationManager.AppSettings.Get("Port"));
            mailClient.EnableSsl = true;
            mailClient.Credentials = new System.Net.NetworkCredential("smtp@intelgain.com", "intelgain");
            try
            {
                mailClient.Send(msg);
                
            }
            catch (Exception)
            {


            }

        }
        protected StringBuilder MessageBody(string empName, string empEmail, int empId, out string emailSubject)
        {
            StringBuilder mailbody = new StringBuilder();
            string createPath = System.IO.Path.Combine(System.Configuration.ConfigurationManager.AppSettings["OnboardingXMLPath"], "OnboardingCompletingSendEmail.xml");
            if (System.IO.File.Exists(createPath))
            {
                // Create an XmlDocument instance and load the XML file
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(createPath);
                DataSet ds = new DataSet();
                StringReader reader = new StringReader(xmlDoc.OuterXml);
                ds.ReadXml(reader);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    emailSubject = (dt.Rows[0]["EmailSubject"].ToString());
                    mailbody.Append("<div style='font - family: Tahoma; '><p><img src='http://emp.intelegain.com/Member/images/logo.png'><br>");
                    mailbody.Append("<p>");
                    mailbody.Append(dt.Rows[0]["ReceiverWelcome"].ToString());
                    mailbody.Append("</p>");
                    mailbody.Append("<p>");
                    mailbody.Append(dt.Rows[0]["MainBody"].ToString().Replace("{employee_name}", empName+" ").Replace("{employee_id}", empId.ToString()));
                    mailbody.Append("<br/>");
                    mailbody.Append("<br/>");
                    mailbody.Append(dt.Rows[0]["Signature"].ToString());
                    mailbody.Append("<br/>");
                    mailbody.Append(dt.Rows[0]["SenderSignature"].ToString());
                    mailbody.Append("</p>");
                    mailbody.Append("</div>");

                }
                else
                {

                    emailSubject = string.Empty;
                }
            }
            else
            {
                emailSubject = string.Empty;
            }
            return mailbody;

        }

    }
}