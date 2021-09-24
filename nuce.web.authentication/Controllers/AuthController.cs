using nuce.web.authentication.Models;
using nuce.web.authentication.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace nuce.web.authentication.Controllers
{
    public class AuthController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Test()
        {
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult Login([FromBody] LoginModel model)
        {
            string strConnectionString = ConfigurationManager.ConnectionStrings["eduwebConnectionString"].ConnectionString;

            string strSql = string.Format(@"SELECT count(MaND) 
                FROM [dbo].[NguoiDung] where MaND='{0}' and (MatKhau='{1}'
                or (exists (select MaND from NguoiDungOnline where  MaND='{0}') and '{1}'='23fbf23c921b754adcb2fcac8e4b19b8a7c740'))", model.MaND, PasswordUtil.HashPassword(model.Pass));

            using (SqlConnection connection = new SqlConnection(strConnectionString))
            {
                SqlCommand command = new SqlCommand(strSql, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        var count = (int)reader[0];
                        if(count == 1)
                        {
                            reader.Close();
                            return Ok();
                        }
                        return Unauthorized();
                    }
                    else
                    {
                        reader.Close();
                        return Unauthorized();
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
        }
    }
}
