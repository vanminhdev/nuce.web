using System;
using System.Web.UI;

namespace DotNetNuke.UI.Skins.Controls
{
    public partial class NUCE_SO_SinhVien_LinkInfo : SkinObjectBase
    {
        #region "Private Properties"
        private const string MyFileName = "NUCE_SO_SinhVien_LinkInfo.ascx";
        #endregion
        #region "Event Handlers"
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if(Session[nuce.web.Utils.session_sinhvien]==null)
                {
                    divLinkInfo.InnerHtml = "";
                }
                else
                {
                    nuce.web.model.SinhVien SinhVien = (nuce.web.model.SinhVien)Session[nuce.web.Utils.session_sinhvien];
                    divLinkInfo.InnerHtml = string.Format("<div id='userLogin' class='fr'><span>Xin chào: {0} - {1} (<a href='/tabid/121/default.aspx?dieukhien=logout'>Thoát</a> - <a href='/tabid/121/default.aspx'>Đổi mật khẩu</a>)</span></div>",SinhVien.Ho,SinhVien.Ten);
                }
            }
            base.OnLoad(e);
        }
        #endregion
    }
}