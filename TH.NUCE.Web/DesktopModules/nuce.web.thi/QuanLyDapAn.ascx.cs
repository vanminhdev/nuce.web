using System;
using System.Data;
using System.Web;
namespace nuce.web.thi
{
    public partial class QuanLyDapAn : CoreModule
    {
        protected override void OnInit(EventArgs e)
        {
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ma"] != null && Request.QueryString["ma"] != "")
                {
                    string tukhoa = "";
                    if (Request.QueryString["tukhoa"] != null && Request.QueryString["tukhoa"] != "")
                        tukhoa = Request.QueryString["tukhoa"].Trim();
                    txtTuKhoa.Text = tukhoa;

                    string macauhoi = Request.QueryString["ma"].Trim();
                    string mabocauhoi = Request.QueryString["mabocauhoi"].Trim();
                    txtMaCauHoi.Text = macauhoi;
                    txtMaBoCauHoi.Text = mabocauhoi;
                    // Kiểm tra xem có lớp
                    DataTable dtCauHoi  = data.dnn_NuceThi_CauHoi.getByMa(macauhoi);
                    if (dtCauHoi.Rows.Count > 0)
                    {
                        int iCauHoiID=int.Parse(dtCauHoi.Rows[0]["CauHoiID"].ToString())
                        ;
                         int iType=int.Parse(dtCauHoi.Rows[0]["Type"].ToString())
                        ;
                        string strNameType = this.GetLoaiCauHoi(iType).Name;
                        string strNoiDungCauHoi = HttpUtility.HtmlDecode(dtCauHoi.Rows[0]["Content"].ToString());
                        strNoiDungCauHoi= string.Format("<b>Câu hỏi (loại {1})</b> : {0} </br> <a href='/tabid/{2}/default.aspx?ma={3}&&tukhoa={4}'>Quay lại danh sách</a>", strNoiDungCauHoi, this.GetLoaiCauHoi(iType).Description,101, mabocauhoi,txtTuKhoa.Text);
                        //txtLopQuanLyID.Text = iLopQuanLyID.ToString();
                        if (Request.QueryString["EditType"] != null && Request.QueryString["EditType"] != "")
                        {
                            string strType = Request.QueryString["EditType"];
                            
                            txtType.Text = strType;
                            divAnnouce.InnerHtml = "";
                            if (strType == "edit" || strType == "insert")
                            {
                                switch (strNameType)
                                {
                                    case "MA":
                                        trMatching.Visible = true;
                                        trIsChecked.Visible = false;
                                        break;
                                    case "SC":
                                    case "MC":
                                    case "TQ":
                                    case "FQ":
                                    case "TL":
                                        trMatching.Visible = false;
                                        trIsChecked.Visible = true;
                                        break;
                                    default: break;
                                }


                                txtCauHoiID.Text = iCauHoiID.ToString();
                                if (strType == "insert")
                                {
                                    divAnnouce.InnerHtml = string.Format("Thêm mới đáp án trong {0}", strNoiDungCauHoi);
                                }
                                else
                                {
                                    divAnnouce.InnerHtml = string.Format("Chỉnh sửa thông tin của đáp án trong {0}", strNoiDungCauHoi);
                                    int iID = int.Parse(Request.QueryString["id"]);
                                    DataTable dtDapAn = data.dnn_NuceThi_DapAn.get(iID);
                                    if(dtCauHoi.Rows.Count>0)
                                    {
                                        txtID.Text = iID.ToString();
                                        txtNoiDung.Text= dtDapAn.Rows[0]["Content"].ToString(); ;
                                        txtMatching.Text = dtDapAn.Rows[0]["Matching"].ToString(); 
                                        //txtBoCauHoiID.Text= dtCauHoi.Rows[0]["BoCauHoiID"].ToString();
                                        txtOrder.Text= dtDapAn.Rows[0]["Order"].ToString();
                                        chkIsChecked.Checked = bool.Parse(dtDapAn.Rows[0]["IsCheck"].ToString());
                                        //txtLevel.Text= dtCauHoi.Rows[0]["Level"].ToString();
                                        //txtTypeCauHoi.Text= dtCauHoi.Rows[0]["Type"].ToString();
                                    }
                                    else
                                    {
                                        returnMain(macauhoi);
                                    }
                                }
                                //divDapAn.InnerHtml = strTableDapAn;
                            }
                            else if (strType == "delete")
                            {
                                int iID = int.Parse(Request.QueryString["id"]);
                                data.dnn_NuceThi_CauHoi.updateStatus(iID, 4);
                                returnMain(Request.QueryString["ma"]);
                            }
                            else if (strType == "changefinish")
                            {
                                int iID = int.Parse(Request.QueryString["id"]);
                                data.dnn_NuceThi_CauHoi.updateStatus(iID, 3);
                                returnMain(Request.QueryString["ma"]);
                            }
                            else if (strType == "changeactive")
                            {
                                int iID = int.Parse(Request.QueryString["id"]);
                                data.dnn_NuceThi_CauHoi.updateStatus(iID, 1);
                                returnMain(Request.QueryString["ma"]);
                            }
                        }
                        else
                        {
                            divAnnouce.InnerHtml = strNoiDungCauHoi;
                            divEdit.Visible = false;
                            displayList(iCauHoiID, strNameType);
                        }
                    }
                    else
                    {
                        divAnnouce.InnerHtml = "Không tồn tại mã câu hỏi này đề nghị bạn chọn lại !";
                        divEdit.Visible = false;
                    }
                }
                else
                {
                    divAnnouce.InnerHtml = "Không có mã câu hỏi !";
                    divEdit.Visible = false;
                }
            }
        }

        private void displayList(int CauHoiID,string NameType)
        {
            DataTable tblTable = data.dnn_NuceThi_DapAn.getByCauHoi(CauHoiID);
            string strTable = "<div>" +
                              "<div style='font-weight:bold; font-size:16px; color:#333; float:left; padding-left:20px;'>Danh sác đáp án </div>" +
                              "<div class='welcome_b'></div>" +
                              "<div class='nd fl'>" +
                              "<div class='noidung'>";
            strTable += "<div class='nd_t fl'>";
            strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", "STT");
            strTable += string.Format("<div class='nd_t22 fl'>{0}</div>", "Nội dung");
            switch(NameType)
            {
                case "MA":
                    strTable += string.Format("<div class='nd_t22 fl'>{0}</div>", "Nội dung ghép");
                    break;
                case "SC":
                case "MC":
                case "TQ":
                case "FQ": strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Là đáp án");
                    break;
                default:break;
            }
            
           
            strTable += string.Format("<div class='nd_t33 fl'>{0}</div>", "Trạng thái");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Chỉnh sửa");
            strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", "Xóa");
            strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&ma={1}&&mabocauhoi={2}&&tukhoa={3}'>Thêm mới</a></div>", this.TabId, txtMaCauHoi.Text,txtMaBoCauHoi.Text,txtTuKhoa.Text);
            strTable += "</div>";
            strTable += "<div class='nd_b fl'>";
            for (int i = 0; i < tblTable.Rows.Count; i++)
            {
                strTable += " <div class='nd_b1 fl'>";
                strTable += string.Format("<div class='nd_t1 fl'>{0}</div>", (i + 1));
                strTable += string.Format("<div class='nd_t22 fl'>{0}</div>",HttpUtility.HtmlDecode( tblTable.Rows[i]["Content"].ToString()));
                switch (NameType)
                {
                    case "MA":
                        strTable += string.Format("<div class='nd_t22 fl'>{0}</div>", HttpUtility.HtmlDecode(tblTable.Rows[i]["Matching"].ToString()));
                        break;
                    case "SC":
                    case "MC":
                    case "TQ":
                    case "FQ":
                        strTable += string.Format("<div class='nd_t4 fl'>{0}</div>", bool.Parse(tblTable.Rows[i]["IsCheck"].ToString())?"Là đáp án":"không là đáp án");
                        break;
                    default: break;
                }
                int iStatus = int.Parse(tblTable.Rows[i]["Status"].ToString());
                if (iStatus.Equals(1))
                {
                    strTable += string.Format("<div class='nd_t33 fl'>{0} ", "Hoạt động");
                    strTable += string.Format(" - <a href='/tabid/{0}/default.aspx?EditType=changefinish&&id={1}&&ma={2}&&mabocauhoi={3}&&tukhoa={4}'>(Chuyển)</a></div>", this.TabId, tblTable.Rows[i]["DapAnID"].ToString(), txtMaCauHoi.Text,txtMaBoCauHoi.Text,txtTuKhoa.Text);
                }
                else
                {
                    strTable += string.Format("<div class='nd_t33 fl'>{0}", "Dừng hoạt động");
                    strTable += string.Format(" - <a href='/tabid/{0}/default.aspx?EditType=changeactive&&id={1}&&ma={2}&&mabocauhoi={3}&&tukhoa={4}'>(Chuyển)</a></div>", this.TabId, tblTable.Rows[i]["DapAnID"].ToString(), txtMaCauHoi.Text, txtMaBoCauHoi.Text,txtTuKhoa.Text);
                }
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=edit&&id={1}&&ma={2}&&mabocauhoi={3}&&tukhoa={4}'>Sửa</a></div>", this.TabId, tblTable.Rows[i]["DapAnID"].ToString(), txtMaCauHoi.Text, txtMaBoCauHoi.Text, txtTuKhoa.Text);
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=delete&&id={1}&ma={2}&&mabocauhoi={3}&&tukhoa={4}'>Xóa</a></div>", this.TabId, tblTable.Rows[i]["DapAnID"].ToString(), txtMaCauHoi.Text, txtMaBoCauHoi.Text, txtTuKhoa.Text);
                strTable += string.Format("<div class='nd_t4 fl'><a href='/tabid/{0}/default.aspx?EditType=insert&&ma={1}&&mabocauhoi={2}&&tukhoa={3}'>Thêm mới</a></div>", this.TabId, txtMaCauHoi.Text, txtMaBoCauHoi.Text, txtTuKhoa.Text);
                strTable += "</div>";
            }
            strTable += "</div>";
            strTable += "</div>";
            strTable += "</div>";
            strTable += "</div>";
            divContent.InnerHtml = strTable;
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string strType = txtType.Text;
            string strMa = txtMaCauHoi.Text;
            string strNoiDung = txtNoiDung.Text;
            bool isCheck = chkIsChecked.Checked;
            string strGhepNoiDung = txtMatching.Text;
            int iCauHoiID = int.Parse(txtCauHoiID.Text);
            int iOrder = int.Parse(txtOrder.Text);

            //int imabocauhoiQuanLy = int.Parse(txtLopQuanLyID.Text)
            if (strMa.Trim().Equals(""))
            {
                divAnnouce.InnerText = "Không được để mã trắng";
                return;
            }
            //int iLopQuanLy = int.Parse(txtLopQuanLyID.Text);
            if (strType == "insert")
            {
                int iReturnID = data.dnn_NuceThi_DapAn.insert(iCauHoiID, -1, strNoiDung, isCheck, strGhepNoiDung, 0, 0, iOrder, 1);
                if (iReturnID > 0)
                {
                    returnMain(txtMaCauHoi.Text);
                }
                else
                {
                    divAnnouce.InnerHtml = divAnnouce.InnerHtml + " --- Thêm mới thất bại !!!";
                }
            }
            else
            {
                if (strType == "edit")
                {
                    int iID = int.Parse(txtID.Text);
                    data.dnn_NuceThi_DapAn.update(iID, iCauHoiID, -1, strNoiDung, isCheck, strGhepNoiDung, 0, 0, iOrder);
                    returnMain(txtMaCauHoi.Text);
                }
            }
        }
        protected void btnQuayLai_Click(object sender, EventArgs e)
        {
            returnMain(txtMaCauHoi.Text)
            ;
        }
        private void returnMain(string mabocauhoi)
        {
            Response.Redirect(string.Format("/tabid/{0}/default.aspx?ma={1}&&mabocauhoi={2}&&tukhoa={3}", this.TabId, mabocauhoi,txtMaBoCauHoi.Text,txtTuKhoa.Text));
        }
        protected void btnTimBoCauHoi_Click(object sender, EventArgs e)
        {
            returnMain(txtMaCauHoi.Text.Trim());
        }
    }
}