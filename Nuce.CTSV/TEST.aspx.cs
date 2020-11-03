using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Nuce.CTSV
{
    public partial class TEST : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed1_Click(object sender, EventArgs e)
        {
            
            var url = "http://mail.amtool.vn/api/send-email";
            var body = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("emails", "thanghn@nuce.edu.vn"),
            new KeyValuePair<string, string>("template", "11"),
            new KeyValuePair<string, string>("subject", "12"),
            new KeyValuePair<string, string>("email_identifier", "emails"),
            new KeyValuePair<string, string>("datetime", "04-09-2020 3:30 pm"),
            new KeyValuePair<string, string>("send_later_email", "0"),
            new KeyValuePair<string, string>("timezone", "7")

        };
            using (var client = new HttpClient())
            {
                HttpContent httpContent = new FormUrlEncodedContent(body);
                var postTask = client.PostAsync("http://mail.amtool.vn/api/send-email", httpContent);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    //var readTask = result.Content.ReadAsStringAsync();
                    //readTask.Wait();
                    divHienThi.InnerHtml = "Thanh cong";

                }
                else
                {
                    divHienThi.InnerHtml = result.StatusCode.ToString();
                }
               
            }
        
        }
    }
}