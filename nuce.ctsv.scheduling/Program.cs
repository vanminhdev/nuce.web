using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace nuce.ctsv.scheduling
{
    class Program
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            common.WriteLog("Start");
            while (true)
            {
                //Lấy dữ liệu từ bảng tin nhắn
                string strSql = string.Format(@"SELECT * FROM [dbo].[AS_Academy_Student_TinNhan] where Status = 1 ");

                DataTable dt = Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteDataset(nuce.web.data.Nuce_Common.ConnectionString, CommandType.Text, strSql).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var receiver = dt.Rows[i]["receiverEmail"].ToString();
                    common.WriteLog(receiver);
                    Logger.Info(receiver);
                    var data = new SendEmailData();

                    data.Body = dt.Rows[i]["Content"].ToString();
                    data.Receiver = receiver;
                    data.Subject = string.Format("{0}", dt.Rows[i]["Title"].ToString());

                    try
                    {
                        SendEmail(data);
                        Console.WriteLine("Receive");
                        strSql = string.Format(@"Update [dbo].[AS_Academy_Student_TinNhan] set Status=2 where Id={0}", dt.Rows[i]["ID"].ToString());
                        Microsoft.ApplicationBlocks.Data.SqlHelper.ExecuteNonQuery(nuce.web.data.Nuce_Common.ConnectionString, CommandType.Text, strSql);
                    }
                    catch (SmtpException ex)
                    {
                        string msg = "Mail cannot be sent because of the server problem: ";
                        msg += ex.Message;
                        Logger.Error(ex, " Send Email Error");
                        Logger.Error("Subject: ", data.Subject);
                        Logger.Error("Body: ", data.Body);
                        Console.WriteLine("Failed: {0}", msg);
                    }
                }
                common.WriteLog("Waiting !!!!!");
                Thread.Sleep(60 * 2000);
            }
            //common.WriteLog("Finish");
            //Console.ReadLine();
        }

        static void SendEmail(SendEmailData data)
        {
            var smtpClient = new SmtpClient(common.MailServer, common.MailPort);
            smtpClient.Credentials = new System.Net.NetworkCredential(common.MailUser, common.MailPassword);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();

            //Setting From , To and CC
            mail.From = new MailAddress(common.MailFromAddress, common.MailFromName);
            mail.To.Add(new MailAddress(data.Receiver));
            mail.Body = data.Body;
            mail.Subject = data.Subject;
            mail.IsBodyHtml = true;
            smtpClient.Send(mail);
        }
    }

    public class SendEmailData
    {
        public string Subject{ get; set; }
        public string Body { get; set; }
        public string Receiver { get; set; }
    }

}
