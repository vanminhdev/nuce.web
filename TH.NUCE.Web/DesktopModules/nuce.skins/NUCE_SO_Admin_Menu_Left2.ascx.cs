using DotNetNuke.Entities.Users;
using System;
using System.Web.UI;

namespace DotNetNuke.UI.Skins.Controls
{
    public partial class NUCE_SO_Admin_Menu_Left2 : SkinObjectBase
    {
        #region "Private Properties"
        private const string MyFileName = "NUCE_SO_Admin_Menu_Left2.ascx";
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
                string strQuanLyPhongMay = "<li class=\"closed\"><span class=\"folder\">Quản lý phòng máy</span><ul>";
                string strGiangVien_QuanLyLichPhongMay= "<li class=\"closed\"><span class=\"folder\">Lịch phòng máy</span><ul>";

                if (_currentUser.IsInRole(nuce.web.Utils.role_Admin) || _currentUser.IsInRole(nuce.web.Utils.role_QuanTriThongTinChung))
                {
                    strQuanLyThongTinChung += "<li><a href = \"/Admin/User-Accounts\" ><span class=\"file\">Quản lý tài khoản</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/35/default.aspx\" ><span class=\"file\">Quản lý năm học</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/36/default.aspx\" ><span class=\"file\">Quản lý học kỳ</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/36/default.aspx\" ><span class=\"file\">Quản lý thông tin khoa</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/36/default.aspx\" ><span class=\"file\">Quản lý thông tin bộ môn</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/36/default.aspx\" ><span class=\"file\">Quản lý giảng viên trong bộ môn</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/36/default.aspx\" ><span class=\"file\">Quản lý môn học</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/36/default.aspx\" ><span class=\"file\">Quản lý giảng viên dạy môn học</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/36/default.aspx\" ><span class=\"file\">Quản lý lớp quản lý</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/36/default.aspx\" ><span class=\"file\">Quản lý sinh viên</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/36/default.aspx\" ><span class=\"file\">Quản lý Toà nhà</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/34/default.aspx\" ><span class=\"file\">Quản lý phòng học</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/34/default.aspx\" ><span class=\"file\">Quản lý ca học</span></a></li>";
                    strQuanLyThongTinChung += "<li><a href = \"/tabid/43/default.aspx\" ><span class=\"file\">Quản lý đồng bộ thông tin phòng đào tạo</span></a></li>";
                    strQuanLyThongTinChung += "</ul></li>";
                }
                else
                {
                    strQuanLyThongTinChung = "";
                }
                if (_currentUser.IsInRole(nuce.web.Utils.role_Admin) || _currentUser.IsInRole(nuce.web.Utils.role_QuanTriPhongMay))
                {
                    strQuanLyPhongMay += "<li><a href = \"/Home/Quan-tri/Lich-phong-may\" ><span class=\"file\">Lịch phòng máy</span></a></li>";
                    strQuanLyPhongMay += "<li><a href = \"/Home/Quan-tri/CB_DanhSachDangKi\" ><span class=\"file\">Danh sách đăng kí</span></a></li>";
                    strQuanLyPhongMay += "<li><a href = \"/Home/Quan-tri/CB_LichSuDangKi\" ><span class=\"file\">Lịch sử đăng kí</span></a></li>";

                    strQuanLyPhongMay += "<li><a href = \"/Home/Quan-tri/Lich-phong-may\" ><span class=\"file\">Thống kê theo lớp học</span></a></li>";
                    strQuanLyPhongMay += "<li><a href = \"/Home/Quan-tri/Lich-phong-may\" ><span class=\"file\">Thống kê theo giảng viên</span></a></li>";
                    strQuanLyPhongMay += "<li><a href = \"/Home/Quan-tri/Lich-phong-may\" ><span class=\"file\">Thống kê theo môn học</span></a></li>";
                    strQuanLyPhongMay += "<li><a href = \"/Home/Quan-tri/Lich-phong-may\" ><span class=\"file\">Thống kê theo sinh viên</span></a></li>";
                    strQuanLyPhongMay += "</ul></li>";

                }
                else
                {
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
                }
                else
                    strQuanLyThiTrucTuyen = "";

                if (_currentUser.IsInRole(nuce.web.Utils.role_GiangVien_DangKiLichPhongMay))
                {
                    strGiangVien_QuanLyLichPhongMay += "<li><a href = \"/Home/Quan-tri/GV_LichPhongMay\" ><span class=\"file\">Lịch phòng máy</span></a></li>";
                    strGiangVien_QuanLyLichPhongMay += "<li><a href = \"/Home/Quan-tri/GV_LichSuDangKi\" ><span class=\"file\">Lịch sử đăng kí</span></a></li>";
                    strGiangVien_QuanLyLichPhongMay += "</ul></li>";
                }
                else
                    strGiangVien_QuanLyLichPhongMay = "";



                strContent += "<ul id=\"ulMenuLeft\" class=\"filetree\">";

                strContent += strQuanLyThongTinChung;
                strContent += strQuanLyPhongMay;
                //strContent += strQuanLyThiTrucTuyen;
                strContent += strGiangVien_QuanLyLichPhongMay;

                strContent += "<li class=\"closed\"><a href = \"/\" ><span class=\"folder\">Liên hệ</span></a></li>";
                strContent += "</ul>";
                divContent.InnerHtml = strContent;
            }
            base.OnLoad(e);
        }
        #endregion
    }
}