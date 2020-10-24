using DotNetNuke.Entities.Users;
using System;
using System.Web.UI;

namespace DotNetNuke.UI.Skins.Controls
{
    public partial class NUCE_SO_Admin_Menu_Left1 : SkinObjectBase
    {
        #region "Private Properties"
        private const string MyFileName = "NUCE_SO_Admin_Menu_Left1.ascx";
        private string _Extension;
        private int _PortalId;
        #endregion
        #region "Public Properties"
        public string Extension
        {
            get
            {
                return _Extension;
            }
            set
            {
                _Extension = value;
            }
        }
        public int PortalId
        {
            get
            {
                return _PortalId;
            }
            set
            {
                _PortalId = value;
            }
        }
        #endregion
        #region "Event Handlers"
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                UserInfo _currentUser = UserController.GetCurrentUserInfo();
                //string[] arrRoles = _currentUser.Roles;
                //string strRoles="";
                //foreach(string strTemp in arrRoles)
                //{
                //    strRoles += (strTemp + "---");
                //}
                string strContent = "Danh mục chức năng";
                string strQuanLyThongTinChung = "<li class=\"closed\"><span class=\"folder\">Quản lý chung</span><ul>";
                string strQuanLyThiTrucTuyen = "<li class=\"closed\"><span class=\"folder\">Quản lý thi trực tuyến</span><ul>";
                string strQuanLyHoc = "<li class=\"closed\"><span class=\"folder\">Quản lý học</span><ul>";
                string strQuanLyPhongMay = "<li class=\"closed\"><span class=\"folder\">Quản lý phòng máy</span><ul>";

                if (_currentUser.IsInRole(nuce.web.Utils.role_Admin) || _currentUser.IsInRole(nuce.web.Utils.role_QuanTriThongTinChung))
                {
                    //strQuanLyThongTinChung += "<li><a href = \"/tabid/89/default.aspx\" ><span class=\"file\">Thông tin trường</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/90/default.aspx\" ><span class=\"file\">Quản lý khoa</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/91/default.aspx\" ><span class=\"file\">Quản lý bộ môn</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/92/default.aspx\" ><span class=\"file\">Quản lý môn học</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/93/default.aspx\" ><span class=\"file\">Quản lý năm học</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/94/default.aspx\" ><span class=\"file\">Quản lý block học</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/95/default.aspx\" ><span class=\"file\">Quản lý lớp</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/96/default.aspx\" ><span class=\"file\">Quản lý lớp học</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/98/default.aspx\" ><span class=\"file\">Quản lý SV trong lớp học</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/97/default.aspx\" ><span class=\"file\">Quản lý SV</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/104/default.aspx\" ><span class=\"file\">Quản lý phòng học</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/102/default.aspx\" ><span class=\"file\">Quản lý thang điểm</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/103/default.aspx\" ><span class=\"file\">Quản lý mức điểm</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/2122/default.aspx\" ><span class=\"file\">Quản lý cựu sinh viên</span></a></li>";
                    strQuanLyThongTinChung += "</ul></li>";


                    strQuanLyPhongMay += "<li><a href = \"/tabid/112/default.aspx\" ><span class=\"file\">Lớp điểm danh</span></a></li>";
                    strQuanLyPhongMay += "<li><a href = \"/tabid/110/default.aspx\" ><span class=\"file\">Vân tay sinh viên</span></a></li>";
                    strQuanLyPhongMay += "<li><a href = \"/tabid/105/default.aspx\" ><span class=\"file\">Thống kê theo lớp học</span></a></li>";
                    strQuanLyPhongMay += "<li><a href = \"/tabid/105/default.aspx\" ><span class=\"file\">Thống kê theo giảng viên</span></a></li>";
                    strQuanLyPhongMay += "<li><a href = \"/tabid/105/default.aspx\" ><span class=\"file\">Thống kê theo môn học</span></a></li>";
                    strQuanLyPhongMay += "<li><a href = \"/tabid/105/default.aspx\" ><span class=\"file\">Thống kê theo sinh viên</span></a></li>";
                    strQuanLyPhongMay += "</ul></li>";

                }
                else
                {
                    strQuanLyThongTinChung = "";
                    strQuanLyPhongMay = "";
                }


                if (_currentUser.IsInRole(nuce.web.Utils.role_GiangVien))
                {
                    strQuanLyThiTrucTuyen += "<li><a href = \"/tabid/117/default.aspx\" ><span class=\"file\">DS Đang thi</span></a></li>";
                    strQuanLyThiTrucTuyen += "<li><a href = \"/tabid/118/default.aspx\" ><span class=\"file\">DS Thi xong</span></a></li>";
                    strQuanLyThiTrucTuyen += "<li><a href = \"/tabid/116/default.aspx\" ><span class=\"file\">DS Thi gốc</span></a></li>";
                    strQuanLyThiTrucTuyen += "<li><a href = \"/tabid/115/default.aspx\" ><span class=\"file\">Quản lý kì thi</span></a></li>";

                    strQuanLyThiTrucTuyen += "<li><a href = \"/tabid/2121/default.aspx\" ><span class=\"file\">Quản lý lớp học</span></a></li>";
                    strQuanLyThiTrucTuyen += "<li><a href = \"/tabid/98/default.aspx\" ><span class=\"file\">Quản lý SV trong lớp học</span></a></li>";

                    strQuanLyThiTrucTuyen += "<li><a href = \"/tabid/114/default.aspx\" ><span class=\"file\">Quản lý bộ đề</span></a></li>";
                    strQuanLyThiTrucTuyen += "<li><a href = \"/tabid/100/default.aspx\" ><span class=\"file\">Quản lý bộ câu hỏi</span></a></li>";
                    strQuanLyThiTrucTuyen += "<li><a href = \"/tabid/101/default.aspx\" ><span class=\"file\">Quản lý câu hỏi</span></a></li>";

                    strQuanLyThiTrucTuyen += "</ul></li>";

                    strQuanLyHoc += "<li><a href = \"/tabid/2121/default.aspx\" ><span class=\"file\">Quản lý lớp học</span></a></li>";
                    strQuanLyHoc += "<li><a href = \"/tabid/98/default.aspx\" ><span class=\"file\">Quản lý SV trong lớp học</span></a></li>";

                    strQuanLyHoc += "<li><a href = \"/tabid/2125/default.aspx\" ><span class=\"file\">Quản lý ca học</span></a></li>";
                    strQuanLyHoc += "<li><a href = \"/tabid/2126/default.aspx\" ><span class=\"file\">Quản lý đợt trao đổi</span></a></li>";

                    strQuanLyHoc += "</ul></li>";
                }
                else
                { 
                    strQuanLyThiTrucTuyen = "";
                    strQuanLyHoc = "";
                }




                strContent += "<ul id=\"ulMenuLeft\" class=\"filetree\">";

                strContent += strQuanLyThongTinChung;
                //strContent += strQuanLyPhongMay;
                strContent += strQuanLyThiTrucTuyen;
                strContent += strQuanLyHoc;

                strContent += "<li class=\"closed\"><a href = \"/\" ><span class=\"folder\">Liên hệ</span></a></li>";
                strContent += "</ul>";
                divContent.InnerHtml = strContent;
            }
            base.OnLoad(e);
        }
        #endregion
    }
}