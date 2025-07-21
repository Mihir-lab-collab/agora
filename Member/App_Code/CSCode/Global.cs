using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using dwtDAL;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Drawing;

namespace CSCode
{
    public static class Global
    {
        public static string SendMail(string bodyStr, string subjectStr, string toMailId)
        {
            return SendMail(bodyStr, subjectStr, toMailId, ConfigurationManager.AppSettings.Get("SupportEmail"), true, "", "");
        }

        public static string SendMail(string bodyStr, string subjectStr, string toMailId, string ccMailId)
        {
            return SendMail(bodyStr, subjectStr, toMailId, ConfigurationManager.AppSettings.Get("SupportEmail"), true, ccMailId, "");
        }

        public static string SendMail(string bodyStr, string subjectStr, string toMailId, string fromMailId, bool isHTML, string ccStr, string bccStr)
        {
            return SendMail(bodyStr, subjectStr, toMailId, ConfigurationManager.AppSettings.Get("SupportEmail"), true, ccStr, bccStr, new Storage.ModuleType(), "");
        }

        public static string SendMail(string bodyStr, string subjectStr, string toMailId, string fromMailId, bool isHTML, string ccStr, string bccStr, Storage.ModuleType StorageModuleType, string Attachments)
        {
            string Message = "";
            try
            {
                MailMessage objMail = new MailMessage();
                int i = 0;

                if (!string.IsNullOrEmpty(toMailId))
                {
                    toMailId = toMailId.Replace(";", ",");
                    string[] objTo = toMailId.Split(',');
                    for (i = 0; i < objTo.Length; i++)
                    {
                        if (objTo[i] != "")
                            objMail.To.Add(objTo[i]);
                    }
                }
                if (!string.IsNullOrEmpty(ccStr))
                {
                    ccStr = ccStr.Replace(";", ",");
                    string[] objCc = ccStr.Split(',');
                    for (i = 0; i < objCc.Length; i++)
                    {
                        if (objCc[i] != "")
                            objMail.CC.Add(objCc[i]);
                    }
                }
                if (!string.IsNullOrEmpty(bccStr))
                {
                    bccStr = bccStr.Replace(";", ",");
                    string[] objBCC = bccStr.Split(',');
                    for (i = 0; i < objBCC.Length; i++)
                    {
                        if (objBCC[i] != "")
                            objMail.Bcc.Add(objBCC[i]);
                    }
                }

                objMail.ReplyToList.Add(fromMailId);
                objMail.From = new MailAddress(fromMailId, fromMailId);
                objMail.Bcc.Add(WebConfigurationManager.AppSettings["BccMail"].ToString());

                objMail.Subject = subjectStr;
                objMail.Body = bodyStr;

                if (isHTML)
                {
                    objMail.IsBodyHtml = true;
                }

                if (Attachments != "")
                {
                    Attachments = Attachments.Replace(";", ",");
                    string[] strAttachArr = Attachments.Split(',');
                    Storage ObjStorge = new Storage();
                    for (int f = 0; f <= strAttachArr.Length - 1; f++)
                    {
                        if (strAttachArr[f] != "")
                        {
                            string FileName = strAttachArr[f];
                            MemoryStream ms = ObjStorge.FileRead(StorageModuleType, FileName);
                            MemoryStream MSAttach = new MemoryStream(ms.ToArray());
                            objMail.Attachments.Add(new Attachment(MSAttach, FileName, System.Net.Mime.MediaTypeNames.Application.Octet));
                        }
                    }
                }

                SmtpClient mailClient = new SmtpClient();
                mailClient = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPServer"));
                mailClient.Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPPort"));
                mailClient.Credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPLogin"),
                    System.Configuration.ConfigurationManager.AppSettings.Get("SMTPPassword"));
                mailClient.EnableSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPSSL"));
                mailClient.Send(objMail);
            }

            catch (Exception ex)
            {
                Message = ex.Message.ToString().Replace("'", "");
            }
            return Message;
        }

        public static string SendMailWithAttachmentsForEmp(string bodyStr, string subjectStr, string toMailId, string fromMailId, bool isHTML, string ccStr, string Attachments)
        {
            string Message = "";
            try
            {
                MailMessage objMail = new MailMessage();
                int i = 0;

                if (!string.IsNullOrEmpty(toMailId))
                {
                    toMailId = toMailId.Replace(";", ",");
                    string[] objTo = toMailId.Split(',');
                    for (i = 0; i < objTo.Length; i++)
                    {
                        if (objTo[i] != "")
                            objMail.To.Add(objTo[i]);

                    }
                }
                if (!string.IsNullOrEmpty(ccStr))
                {
                    ccStr = ccStr.Replace(";", ",");
                    string[] objCc = ccStr.Split(',');
                    for (i = 0; i < objCc.Length; i++)
                    {
                        if (objCc[i] != "")
                            objMail.CC.Add(objCc[i]);

                    }
                }


                if (Attachments != "")
                {
                    Attachments = Attachments.Replace(";", ",");
                    string[] strAttachArr = Attachments.Split(',');
                    Storage ObjStorge = new Storage();
                    for (int f = 0; f <= strAttachArr.Length - 1; f++)
                    {
                        if (strAttachArr[f] != "")
                        {
                            string FileName = strAttachArr[f];
                            MemoryStream ms = ObjStorge.FileRead(Storage.ModuleType.ProjectReport, FileName);
                            MemoryStream MSAttach = new MemoryStream(ms.ToArray());
                            objMail.Attachments.Add(new Attachment(MSAttach, FileName, System.Net.Mime.MediaTypeNames.Application.Octet));
                        }
                    }
                }

                objMail.From = new MailAddress(fromMailId);
                objMail.Bcc.Add(WebConfigurationManager.AppSettings["BccMail"].ToString());
                objMail.Subject = subjectStr;
                objMail.Body = bodyStr;

                if (isHTML)
                {
                    objMail.IsBodyHtml = true;
                }

                SmtpClient mailClient = new SmtpClient(ConfigurationManager.AppSettings.Get("SMTP"));

                mailClient.Host = "localhost";
                mailClient.Port = 25;
                mailClient.UseDefaultCredentials = false;
                mailClient.EnableSsl = false;
                mailClient.Send(objMail);
                Message = "Mail Sent";
            }

            catch (Exception ex)
            {
                Message = ex.Message.ToString().Replace("'", "");
            }
            return Message;
        }
        public static DataTable CheckValidUser(string ProfileID, string UserType, string PageName)
        {
            DataTable ValidDetail = new DataTable();
            int isvalid = validUser(ProfileID, UserType, PageName);
            ValidDetail = GeTableSecurity(ProfileID, PageName);
            ValidDetail.Columns.Add("isvalid", typeof(int));
            ValidDetail.Rows[0]["isvalid"] = isvalid;
            ValidDetail.AcceptChanges();
            return ValidDetail;
        }

        public static DataTable GeTableSecurity(string ProfileID, string PageName)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_SecurityProfile", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
            cmd.Parameters.AddWithValue("@PageName", PageName);

            DataTable t1 = new DataTable();
            using (SqlDataAdapter a = new SqlDataAdapter(cmd))
            {
                a.Fill(t1);
            }
            return t1;
        }

        public static int validUser(string ProfileID, string UserType, string PageName)
        {

            int outputid = 0;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("sp_ProfileCheck", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProfileID", ProfileID);
            cmd.Parameters.AddWithValue("@PageName", PageName);
            try
            {
                using (con)
                {
                    con.Open();
                    outputid = Convert.ToInt32(cmd.ExecuteScalar());

                }
            }
            catch (Exception ex)
            { }
            return outputid;


        }

        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            string key = "SecurityKeyXSDFG";

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString, bool useHashing)
        {
            try
            {
                byte[] keyArray;
                //get the byte code of the string

                byte[] toEncryptArray = Convert.FromBase64String(cipherString);

                //System.Configuration.AppSettingsReader settingsReader =  new AppSettingsReader();
                ////Get your key from config file to open the lock!
                //string key = (string)settingsReader.GetValue("SecurityKeyXSDFG",typeof(String));
                string key = "SecurityKeyXSDFG";

                if (useHashing)
                {
                    //if hashing was used get the hash code with regards to your key
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    //release any resource held by the MD5CryptoServiceProvider

                    hashmd5.Clear();
                }
                else
                {
                    //if hashing was not implemented get the byte code of the key
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);
                }

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                //mode of operation. there are other 4 modes. 
                //We choose ECB(Electronic code Book)

                tdes.Mode = CipherMode.ECB;
                //padding mode(if any extra byte added)
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(
                                     toEncryptArray, 0, toEncryptArray.Length);
                //Release resources held by TripleDes Encryptor                
                tdes.Clear();
                //return the Clear decrypted TEXT
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string SendMailWithAttachments(string bodyStr, string subjectStr, string toMailId, string fromMailId, bool isHTML, string ccStr, string bccStr, string Attachments)
        {
            string Message = "";
            try
            {
                MailMessage objMail = new MailMessage();
                int i = 0;

                if (!string.IsNullOrEmpty(toMailId))
                {
                    toMailId = toMailId.Replace(";", ",");
                    string[] objTo = toMailId.Split(',');
                    for (i = 0; i < objTo.Length; i++)
                    {
                        if (objTo[i] != "")
                            objMail.To.Add(objTo[i]);

                    }
                }
                if (!string.IsNullOrEmpty(ccStr))
                {
                    ccStr = ccStr.Replace(";", ",");
                    string[] objCc = ccStr.Split(',');
                    for (i = 0; i < objCc.Length; i++)
                    {
                        if (objCc[i] != "")
                            objMail.CC.Add(objCc[i]);

                    }
                }


                if (!string.IsNullOrEmpty(bccStr))
                {

                    bccStr = bccStr.Replace(";", ",");
                    string[] objBcc = bccStr.Split(',');
                    for (i = 0; i < objBcc.Length; i++)
                    {
                        if (objBcc[i] != "")
                            objMail.Bcc.Add(objBcc[i]);

                    }
                }
                if (Attachments != null)
                {
                    Attachments = Attachments.Replace(";", ",");
                    string[] strAttachArr = Attachments.Split(',');

                    Attachment CurAttachment;
                    for (int f = 0; f <= strAttachArr.Length - 1; f++)
                    {
                        if (strAttachArr[f] != "")
                        {
                            CurAttachment = new Attachment(strAttachArr[f]);
                            objMail.Attachments.Add(CurAttachment);
                        }
                    }
                }

                objMail.ReplyToList.Add(fromMailId);
                objMail.From = new MailAddress(fromMailId, fromMailId);
                objMail.Bcc.Add(WebConfigurationManager.AppSettings["BccMail"].ToString());
                objMail.Subject = subjectStr;
                objMail.Body = bodyStr;

                if (isHTML)
                {
                    objMail.IsBodyHtml = true;
                }

                SmtpClient mailClient = new SmtpClient();
                mailClient = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPServer"));
                mailClient.Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPPort"));
                mailClient.Credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPLogin"),
                    System.Configuration.ConfigurationManager.AppSettings.Get("SMTPPassword"));
                mailClient.EnableSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPSSL"));
               mailClient.Send(objMail);

                Message = "Mail Sent";
                DisposeAttachments(objMail);

            }

            catch (Exception ex)
            {
                Message = ex.Message.ToString().Replace("'", "");
            }
            return Message;
        }

        private static void DisposeAttachments(MailMessage Message)
        {
            foreach (Attachment attachment in Message.Attachments)
            {
                attachment.Dispose();
            }
            Message.Attachments.Dispose();
            Message = null;
        }


        public static string SendmailUser(string toEmailid, string ComapanyName, string Name, string ContactNo, string Login, string Password)
        {
            string Message = "";
            try
            {
                System.IO.StreamReader DynamicFileReader = null;
                string fileContent = null;
                DynamicFileReader = System.IO.File.OpenText(System.Web.HttpContext.Current.Server.MapPath(@"/MailTemplates/NewCustomerUser.htm"));

                fileContent = DynamicFileReader.ReadToEnd();
                fileContent = fileContent.Replace("{DateToday}", DateTime.Today.ToString("dd/MM/yyyy"));
                fileContent = fileContent.Replace("{ComapanyName}", ComapanyName);
                fileContent = fileContent.Replace("{Name}", Name);
                fileContent = fileContent.Replace("{Contactno}", ContactNo);
                fileContent = fileContent.Replace("{Login}", Login);
                fileContent = fileContent.Replace("{Password}", Password);
                Message = CSCode.Global.SendMail(fileContent, "User Credentials ", Convert.ToString(toEmailid), ConfigurationManager.AppSettings.Get("fromEmail"), true, "", "");
            }
            catch (Exception ex)
            {
                Message = "Error in sending email. " + ex.Message;
            }
            return Message;

        }

        public static string RandomPassword(int length)
        {
            string[] array = new string[54]
                {
                    "0","2","3","4","5","6","8","9",
                    "a","b","c","d","e","f","g","h","j","k","m","n","p","q","r","s","t","u","v","w","x","y","z",
                    "A","B","C","D","E","F","G","H","J","K","L","M","N","P","R","S","T","U","V","W","X","Y","Z"
                };
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < length; i++) sb.Append(array[GetRandomNumber(53)]);
            return sb.ToString();
        }

        private static int GetRandomNumber(int maxNumber)
        {
            if (maxNumber < 1)
                throw new System.Exception("The maxNumber value should be greater than 1");
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            int seed = (b[0] & 0x7f) << 24 | b[1] << 16 | b[2] << 8 | b[3];
            System.Random r = new System.Random(seed);
            return r.Next(1, maxNumber);
        }

        public static string CreatePassword(string strPassword)
        {
            string comppwd = null;
            string haskey = ConfigurationManager.AppSettings["hashKey"].ToString();
            byte tempbyte = byte.Parse("123");
            byte[] bytearr = new byte[2];
            bytearr[0] = tempbyte;
            return comppwd = SessionHelper.ComputeHash(strPassword, haskey, bytearr);
        }

        public static string CreateLoginToken(string strPassword,int EmployeeID,string JoiningDate)
        {
            string comppwd = null;
            string haskey = ConfigurationManager.AppSettings["hashKey"].ToString();
            //byte byteEmployeeID = byte.Parse(Convert.ToString(EmployeeID));
            //byte byteJoiningDate = byte.Parse(Convert.ToString(JoiningDate).Replace("/","").Replace("-", "").Replace(".", ""));
            byte[] bytearr = Encoding.ASCII.GetBytes(Convert.ToString(EmployeeID)+ Convert.ToString(JoiningDate).Replace("/", "").Replace("-", "").Replace(".", ""));
            //byte[] bytearr = new byte[2];
            //bytearr[0] = byteEmployeeID;
            //bytearr[1] = byteJoiningDate;
            return comppwd = SessionHelper.ComputeHash(strPassword, haskey, bytearr);
        }

        public static DataTable IncompleteTS(int EmpID, int Days, DateTime StartDate, int LocationID)
        {
            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_ReportTS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", EmpID);
            cmd.Parameters.AddWithValue("@Days", Days);
            cmd.Parameters.AddWithValue("@FromDate", StartDate);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            try
            {
                using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                {
                    DA.Fill(DT);
                }
                return DT;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }
            return DT;
        }

        public static string ExportDatatableToHtml(DataTable dt)
        {
            StringBuilder strHTMLBuilder = new StringBuilder();
            strHTMLBuilder.Append("<html >");
            strHTMLBuilder.Append("<head>");
            strHTMLBuilder.Append("</head>");
            strHTMLBuilder.Append("<body>");
            //strHTMLBuilder.Append("<table width='100%' border='1' BorderColor='black' cellspacing='0' cellpadding='5'  style='font-size:15px border-style:solid;border-width:thin;border-left:none;'>");
            strHTMLBuilder.Append("<table width='100%' BorderColor='black' cellspacing='0' cellpadding='5'  style='font-size:15px;border-style:solid; border-width:thin;border-left:none;'>");
            strHTMLBuilder.Append("<tr align=left>");

            foreach (DataColumn myColumn in dt.Columns)
            {
                strHTMLBuilder.Append("<th bgcolor='#00B0F0' align=left style='border-style:solid;border-width:thin;border-top:none;border-right:none;border-bottom:none;'>");
                strHTMLBuilder.Append("<h5><font color=white>" + myColumn.ColumnName + "</font></h5>");
                strHTMLBuilder.Append("</th>");
            }
            strHTMLBuilder.Append("</tr>");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow myRow in dt.Rows)
                {
                    strHTMLBuilder.Append("<tr align=left>");
                    foreach (DataColumn myColumn in dt.Columns)
                    {
                        strHTMLBuilder.Append("<td style='border-style:solid;border-width:thin;border-bottom:none;border-right:none;'>");
                        strHTMLBuilder.Append(myRow[myColumn.ColumnName].ToString());
                        strHTMLBuilder.Append("</td>");

                    }
                    strHTMLBuilder.Append("</tr>");
                }
            }
            else
            {
                strHTMLBuilder.Append("<tr align=center>");
                strHTMLBuilder.Append("<td colspan = '"+5+"' style='border-style:solid;border-width:thin;border-bottom:none;border-right:none;'>");
                strHTMLBuilder.Append("No Leave Applied");
                strHTMLBuilder.Append("</td>");
            }

            strHTMLBuilder.Append("</table>");
            strHTMLBuilder.Append("</body>");
            strHTMLBuilder.Append("</html>");
            string Htmltext = strHTMLBuilder.ToString();
            return Htmltext;
        }

        public static string ExportDatatableToHtmlForTds(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            string reportMonth = DateTime.Now.AddMonths(-1).ToString("MMMM yyyy");

            sb.Append("<html><body>");
            sb.Append("<p>Hi Team,</p>");
            sb.Append("<p>Following is the report of the employees who have updated the declaration forms in <b>" + reportMonth + "</b>.</p>");

            sb.Append("<table border='1' cellspacing='0' cellpadding='5' style='border-collapse:collapse; width:100%;'>");

            sb.Append("<tr>");
            sb.Append("<th style='width:20%; text-align:center;'>Employee ID</th>");
            sb.Append("<th style='width:50%; text-align:center;'>Employee Name</th>");
            sb.Append("<th style='width:30%; text-align:center;'>Modified Date Time</th>");
            sb.Append("</tr>");

            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td style='width:20%; text-align:center;'>{0}</td>", row["empid"]);
                sb.AppendFormat("<td style='width:50%; text-align:left;'>{0}</td>", row["empName"]);
                sb.AppendFormat("<td style='width:30%; text-align:center;'>{0}</td>", Convert.ToDateTime(row["ModifiedOn"]).ToString("dd-MMM-yyyy HH:mm:ss"));
                sb.Append("</tr>");
            }

            sb.Append("</table>");
            sb.Append("</body></html>");

            return sb.ToString();
        }

        public static string BIReportProcess()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_DataMap";
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            cmd.Connection = sqlConn;
            sqlConn.Open();
            try
            {

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                sqlConn.Close();
                return "BIReportProcess: Success";
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
                return "BIReportProcess: " + ex.Message;
            }
        }

        public static void WriteErrorLog(this Exception ex)
        {

            string strLogPath = "";
            strLogPath = ConfigurationManager.AppSettings["DataBank"].ToString() + @"\ErrorLog\";

            if (!Directory.Exists(strLogPath))
            {
                Directory.CreateDirectory(strLogPath);
            }

            FileInfo _fleErrorPointer = new FileInfo(strLogPath + "\\Error" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt");
            StreamWriter _swToErrorPointer;
            if (_fleErrorPointer.Exists)
            {
                _swToErrorPointer = _fleErrorPointer.AppendText();
            }
            else
            {
                _swToErrorPointer = _fleErrorPointer.CreateText();
            }
            try
            {
                _swToErrorPointer.WriteLine("=============================================================");
                _swToErrorPointer.WriteLine("Date/Time   : " + DateTime.Now.ToString("dd-MM-yy  hh:mm:ss"));
                _swToErrorPointer.WriteLine("Source   : " + ex.Source);
                _swToErrorPointer.WriteLine("InnerException   : " + ex.InnerException);
                _swToErrorPointer.WriteLine("Message :" + ex.Message);
                _swToErrorPointer.WriteLine("StackTrace :" + ex.StackTrace);
                _swToErrorPointer.WriteLine("==============================================================");

                //LogErrorToDB(ex);
            }
            catch (Exception Ex)
            {

            }
            finally
            {
                _swToErrorPointer.Flush();
                _swToErrorPointer.Close();
            }
        }

        public static void LogErrorToDB(Exception ex)
        {
            HttpContext ctxObject = HttpContext.Current;
            string strLogConnString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
            string logDateTime = DateTime.Now.ToString("g");
            string strReqURL = (ctxObject.Request.Url != null) ? ctxObject.Request.Url.ToString() : String.Empty;
            string strReqQS = (ctxObject.Request.QueryString != null) ? ctxObject.Request.QueryString.ToString() : String.Empty;
            string strServerName = String.Empty;
            if (ctxObject.Request.ServerVariables["HTTP_REFERER"] != null)
            {
                strServerName = ctxObject.Request.ServerVariables["HTTP_REFERER"].ToString();
            }
            string strUserAgent = (ctxObject.Request.UserAgent != null) ? ctxObject.Request.UserAgent : String.Empty;
            string strUserIP = (ctxObject.Request.UserHostAddress != null) ? ctxObject.Request.UserHostAddress : String.Empty;
            string strUserAuthen = (ctxObject.User.Identity.IsAuthenticated.ToString() != null) ? ctxObject.User.Identity.IsAuthenticated.ToString() : String.Empty;
            string strUserName = (ctxObject.User.Identity.Name != null) ? ctxObject.User.Identity.Name : String.Empty;
            string strMessage = string.Empty, strSource = string.Empty, strTargetSite = string.Empty, strStackTrace = string.Empty;
            while (ex != null)
            {
                strMessage = ex.Message;
                strSource = ex.Source;
                strTargetSite = ex.TargetSite.ToString();
                strStackTrace = ex.StackTrace;
                ex = ex.InnerException;
            }
            if (strLogConnString.Length > 0)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "USP_LogExceptionToDB";
                SqlConnection sqlConn = new SqlConnection(strLogConnString);
                cmd.Connection = sqlConn;
                sqlConn.Open();
                try
                {
                    cmd.Parameters.Add(new SqlParameter("@Source", strSource));
                    cmd.Parameters.Add(new SqlParameter("@LogDateTime", logDateTime));
                    cmd.Parameters.Add(new SqlParameter("@Message", strMessage));
                    cmd.Parameters.Add(new SqlParameter("@QueryString", strReqQS));
                    cmd.Parameters.Add(new SqlParameter("@TargetSite", strTargetSite));
                    cmd.Parameters.Add(new SqlParameter("@StackTrace", strStackTrace));
                    cmd.Parameters.Add(new SqlParameter("@ServerName", strServerName));
                    cmd.Parameters.Add(new SqlParameter("@RequestURL", strReqURL));
                    cmd.Parameters.Add(new SqlParameter("@UserAgent", strUserAgent));
                    cmd.Parameters.Add(new SqlParameter("@UserIP", strUserIP));
                    cmd.Parameters.Add(new SqlParameter("@UserName", strUserName));
                    cmd.Parameters.Add(new SqlParameter("@Mode", "Insert"));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    sqlConn.Close();
                }
                catch (Exception exc)
                {
                    WriteErrorLog(exc);  //EventLog.WriteEntry(exc.Source, "Database Error From Exception Log!", EventLogEntryType.Error, 65535);
                }
                finally
                {
                    cmd.Dispose();
                    sqlConn.Close();
                }
            }

        }

        public static DataTable GetEmployees(int LocationID = 0, Boolean IsActive = true)
        {
            DataTable DT = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("SP_Employees", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Mode", "GET");
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@IsActive", IsActive);

                using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                {
                    DA.Fill(DT);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }

            return DT;
        }

        public static DataTable Events(string LocationID = "0", int Days = 7)
        {
            DataTable DT = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
                SqlCommand cmd = new SqlCommand("SP_Events", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@Days", Days);

                using (SqlDataAdapter DA = new SqlDataAdapter(cmd))
                {
                    DA.Fill(DT);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }

            return DT;
        }

        public static string GetLocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }

        public static string GetCurrencyFormat(double amt)
        {
            return String.Format("{0:0,0}", amt);
         }

        public static void ProcessRecurringMilestones()
        {
            List<mileStone> lstGetRecurringMileStone = mileStone.getRecurringMilestone();
            if (lstGetRecurringMileStone.Count > 0)
            {
                for (int i = 0; i < lstGetRecurringMileStone.Count; i++)
                {
                    mileStone obj = new mileStone();
                    obj.amount = lstGetRecurringMileStone[i].amount;
                    obj.Description = lstGetRecurringMileStone[i].Description;
                    obj.EstHours = lstGetRecurringMileStone[i].EstHours;
                    obj.ExRate = lstGetRecurringMileStone[i].ExRate;
                    obj.projID = lstGetRecurringMileStone[i].projID;
                    DateTime DueDate = Convert.ToDateTime(lstGetRecurringMileStone[i].dueDate);
                    DueDate = DueDate.AddMonths(1);
                    obj.dueDate = Convert.ToString(DueDate);
                    string strMonth = DueDate.ToString("MMM", CultureInfo.InvariantCulture);
                    string strYear = Convert.ToString(DueDate.Year);
                    obj.name = strMonth + "_" + strYear;
                    obj.IsRecurring = lstGetRecurringMileStone[i].IsRecurring;
                    obj.insertedBy = lstGetRecurringMileStone[i].insertedBy;
                    //obj.modifiedBy = lstGetRecurringMileStone[i].insertedBy;
                    obj.BalanceAmount = lstGetRecurringMileStone[i].BalanceAmount;
                    obj.RecurringMSID = lstGetRecurringMileStone[i].projMilestoneID;
                    obj.insertMileStoneData("InsertOrUpdate", obj.projID, 0, obj.name, obj.amount, obj.ExRate, obj.dueDate, obj.DeliveryDate, obj.EstHours, obj.Description, obj.insertedBy, obj.BalanceAmount, obj.IsRecurring, obj.RecurringMSID);
                }
            }
        }

        public static string SendKBMail(string bodyStr, string subjectStr, string toMailId, string fromMailId, bool isHTML)
        {
            string Message = "";
            try
            {
                MailMessage objMail = new MailMessage();

                objMail.To.Add(new MailAddress(toMailId));
                objMail.From = new MailAddress(fromMailId, ConfigurationManager.AppSettings.Get("compName"));
                objMail.Bcc.Add(WebConfigurationManager.AppSettings["BccMail"].ToString());
                objMail.Subject = subjectStr;
                objMail.Body = bodyStr;
                if (isHTML)
                {
                    objMail.IsBodyHtml = true;
                }


                //**** For Live****///
                SmtpClient mailClient = new SmtpClient();
                mailClient = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings.Get("SMTP"));
                mailClient.Port = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("Port"));
                mailClient.Credentials = new System.Net.NetworkCredential("smtp@intelgain.com", "intelgain");
                mailClient.EnableSsl = false;
                mailClient.Send(objMail);
                Message = "Mail Sent";
            }

            catch (Exception ex)
            {
                Message = ex.Message.ToString().Replace("'", "");
            }
            return Message;
        }
        public static string GetChartData(string mode)
        {
            string Message = "";
            DataTable dt = new DataTable();


            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            //SqlCommand cmd = new SqlCommand("SP_Employee", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@Mode", mode);

            try
            { 
            //{
            //    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            //    {
            //        da.Fill(dt);
            //    }

            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            string empName = Convert.ToString(dt.Rows[i]["empName"]);
            //            string empEmail = Convert.ToString(dt.Rows[i]["empEmail"]);

            //            string basepageUrl = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"].ToString();
            //            string path = HttpContext.Current.Server.MapPath(@"Images/birthdayTemp.jpg");
            //            LinkedResource Img = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
            //            Img.ContentId = "MyImage";

                        System.IO.StreamReader DynamicFileReader = null;
                        string fileContent = null;
                        DynamicFileReader = System.IO.File.OpenText(System.Web.HttpContext.Current.Server.MapPath(@"../MailTemplates/highchart_Intelegain1.html"));
                        fileContent = DynamicFileReader.ReadToEnd();
                        


                       
                        AlternateView avHtml = AlternateView.CreateAlternateViewFromString(fileContent, null, System.Net.Mime.MediaTypeNames.Text.Html);//: System.Net.Mime.MediaTypeNames.Text.Plain
                        //avHtml.LinkedResources.Add(Img);
                        //avHtml.LinkedResources.Add(ImgLogo);
                        //fileContent = fileContent.Replace("{Image}", "cid:MyImage");
                        //fileContent = fileContent.Replace("{ImgLogo}", "cid:Logo");
                        MailMessage message = new MailMessage();
                        message.To.Add("trupti.d@intelegain.com");// Email-ID of Receiver  

                        message.Subject = "Report";// Subject of Email  
                        message.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings.Get("fromEmail"));// Email-ID of Sender  
                        message.IsBodyHtml = true;
                        message.AlternateViews.Add(avHtml);
                        SmtpClient SmtpMail = new SmtpClient();
                        SmtpMail.Host = "smtp.gmail.com";//name or IP-Address of Host used for SMTP transactions  
                        SmtpMail.Port = 587;//Port for sending the mail  
                        SmtpMail.Credentials = new
            System.Net.NetworkCredential("smtp@intelgain.com", "intelgain");//username/password of network, if apply  
                        SmtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;
                        SmtpMail.EnableSsl = true;

                        SmtpMail.ServicePoint.MaxIdleTime = 0;
                        SmtpMail.ServicePoint.SetTcpKeepAlive(true, 2000, 2000);
                        message.BodyEncoding = Encoding.Default;
                        message.Priority = MailPriority.High;
                        SmtpMail.Send(message);
                   // }

               // }
                Message = "Mail Sent";

            }
            catch (Exception ex)
            {
                Message = "Error in sending email. " + ex.Message + Convert.ToString(ex.StackTrace);

            }
            return Message;

        }

        public static string GetMonthlyTdsReportMail(string mode)
        {
            string errorMessage = "";
            DataTable dt = new DataTable();
            string reportMonth = DateTime.Now.AddMonths(-1).ToString("MMM-yyyy");

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("USP_MonthlyTdsReportLog", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);

            int status = 0;

            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

                string mailTo = ConfigurationManager.AppSettings["AccountEmail"];
                string mailFrom = ConfigurationManager.AppSettings["SupportEmail"];

                SmtpClient smtpClient = new SmtpClient
                {
                    Host = ConfigurationManager.AppSettings["SMTPServer"],
                    Port = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]),
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SMTPLogin"],
                                                        ConfigurationManager.AppSettings["SMTPPassword"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTPSSL"])
                };

                if (dt != null && dt.Rows.Count > 0)
                {
                    string emailBody = ExportDatatableToHtmlForTds(dt);
                    MailMessage message = new MailMessage(mailFrom, mailTo);
                    message.IsBodyHtml = true;
                    message.Subject = "Monthly report of Employees Investment declaration form " + reportMonth;
                    message.Body = emailBody;

                    smtpClient.Send(message);
                }
                else
                {
                    MailMessage message = new MailMessage(mailFrom, mailTo);
                    message.IsBodyHtml = true;
                    message.Subject = "Monthly report of Employees Investment declaration form " + reportMonth;
                    message.Body = "Hi Team,<br/><br/>No employee has updated their investment declaration form in " + reportMonth + "<br/><br/>";

                    smtpClient.Send(message);
                }

                status = 1;
            }
            catch (Exception ex)
            {
                status = 0;
                errorMessage = "Error in sending email. " + ex.Message + " " + ex.StackTrace;
            }

            InsertTdsReportLog(status.ToString(), errorMessage);
            return status.ToString();
        }
        public static bool CheckTdsReportLog()
        {
            bool attemptMade = false;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_MonthlyTdsReportLog", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "Check");
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    attemptMade = (result != null && Convert.ToInt32(result) == 1);

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error in CheckTdsReportLog: " + ex.Message);
                }
            }

            return attemptMade;
        }
        public static void InsertTdsReportLog(string status, string errorMessage)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_MonthlyTdsReportLog", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "Insert");
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@ErrorMessage", (object)errorMessage ?? DBNull.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error in InsertTdsReportLog: " + ex.Message);
                }
            }
        }
        public static string BirthdayWishes(string mode)
        {
            string Message = "";
            DataTable dt = new DataTable();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_Employee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);

            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string empName = Convert.ToString(dt.Rows[i]["empName"]);
                        string empEmail = Convert.ToString(dt.Rows[i]["empEmail"]);

                        string basepageUrl = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"].ToString();
                        string path = HttpContext.Current.Server.MapPath(@"Images/birthdayTemp.jpg");
                        LinkedResource Img = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                        Img.ContentId = "MyImage";
                        
                        System.IO.StreamReader DynamicFileReader = null;
                        string fileContent = null;
                        DynamicFileReader = System.IO.File.OpenText(System.Web.HttpContext.Current.Server.MapPath(@"../MailTemplates/BirthdayEmailTemplat.htm"));
                        fileContent = DynamicFileReader.ReadToEnd();
                        fileContent = fileContent.Replace("{EmpName}", empName);
                        fileContent = fileContent.Replace("{BaseUrl}", basepageUrl);
                        
                       
                        string pathLogo = HttpContext.Current.Server.MapPath(@"Images/logo.png");
                        LinkedResource ImgLogo = new LinkedResource(pathLogo, MediaTypeNames.Image.Jpeg);
                        ImgLogo.ContentId = "Logo";
                        AlternateView avHtml = AlternateView.CreateAlternateViewFromString(fileContent, null, System.Net.Mime.MediaTypeNames.Text.Html);//: System.Net.Mime.MediaTypeNames.Text.Plain
                        avHtml.LinkedResources.Add(Img);
                        avHtml.LinkedResources.Add(ImgLogo);
                        fileContent = fileContent.Replace("{Image}", "cid:MyImage");
                        fileContent = fileContent.Replace("{ImgLogo}", "cid:Logo");
                        MailMessage message = new MailMessage();
                        message.To.Add(ConfigurationManager.AppSettings.Get("CommonEmail"));// Email-ID of Receiver  

                        message.Subject = "Happy Birthday " + empName;// Subject of Email  
                        message.From = new
                            System.Net.Mail.MailAddress(ConfigurationManager.AppSettings.Get("fromEmail"));// Email-ID of Sender  
                        message.IsBodyHtml = true;
                        message.AlternateViews.Add(avHtml);
                        SmtpClient SmtpMail = new SmtpClient();
                        SmtpMail.Host = "smtp.gmail.com";//name or IP-Address of Host used for SMTP transactions  
                        SmtpMail.Port = 587;//Port for sending the mail  
                        SmtpMail.Credentials = new
            System.Net.NetworkCredential("smtp@intelgain.com", "intelgain");//username/password of network, if apply  
                        SmtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;
                        SmtpMail.EnableSsl = true;

                        SmtpMail.ServicePoint.MaxIdleTime = 0;
                        SmtpMail.ServicePoint.SetTcpKeepAlive(true, 2000, 2000);
                        message.BodyEncoding = Encoding.Default;
                        message.Priority = MailPriority.High;
                        SmtpMail.Send(message);
                    }

                }               
                Message = "Mail Sent";

            }
            catch (Exception ex)
            {
                Message = "Error in sending email. " + ex.Message + Convert.ToString(ex.StackTrace);

            }
            return Message;
        }

        public static DataTable UpcomingLeaves(string mode)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_Employee", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mode", mode);
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }
            return dt;
        }


        public static DataTable EmployeeQuarterlyAppraisal()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SP_Appraisal", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MODE", "ADD");

            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write(ex.Message);
            }
            return dt;

        }

        public static string GetTemplatePropertyValue(object obj, string propertyName)
        {
            string propertyValue = string.Empty;
            var property = obj.GetType().GetProperties().FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));
            if (property != null)
            {
                propertyValue = (string)property.GetValue(obj, null);
            }
            return propertyValue;
        }
        
        public enum EmProfile
        {
            Accounts = 4
        };

    }
}


