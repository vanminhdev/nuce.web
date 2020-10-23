using System;
using System.Web.UI;

namespace DotNetNuke.UI.Skins.Controls
{
    public partial class NUCE_SO_SinhVien_Menu : SkinObjectBase
    {
        #region "Private Properties"
        private const string MyFileName = "NUCE_SO_SinhVien_Menu.ascx";
        #endregion
        #region "Event Handlers"
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if(Session[nuce.web.Utils.session_sinhvien]==null)
                {
                    divMenu.InnerHtml = "";
                }
                else
                {
                    string strHtml = "<div id='mainMenu' class='fl'>";
                    //strHtml+= "<ul><li><a class='active' href='/Sinh-Vien' title='Thi - kiểm tra'>THI - KIỂM TRA</a></li><li><a href='/tabid/122/default.aspx' title='Nội quy - quy chế'>NỘI QUY - QUY CHẾ</a></li></ul></div>";
                    strHtml += "<ul><li><a class='active' href='/Sinh-Vien/Sinh-Vien-Hoc' title='Học'>HỌC</a></li><li><a href='/tabid/122/default.aspx' title='Nội quy - quy chế'>NỘI QUY - QUY CHẾ</a></li></ul></div>";
                    divMenu.InnerHtml = strHtml;
                }
            }
            base.OnLoad(e);
        }
        #endregion
    }
}