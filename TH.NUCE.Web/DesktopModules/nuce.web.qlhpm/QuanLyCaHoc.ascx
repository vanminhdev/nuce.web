<%@ control language="C#" autoeventwireup="true" codebehind="QuanLyCaHoc.ascx.cs" inherits="nuce.web.qlhpm.QuanLyCaHoc" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div style="text-align: center;" id="divFilter" runat="server">
    <span>Chọn Môn học: </span>
    <span>
        <asp:DropDownList ID="ddlNguoiDung_MonHoc" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlNguoiDung_MonHoc_SelectedIndexChanged">
        </asp:DropDownList></span>
    <span>Block học: </span>
    <span>
        <asp:DropDownList ID="ddlBlockHoc" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBlockHoc_SelectedIndexChanged">
        </asp:DropDownList></span>
</div>
<div runat="server" id="divContent"></div>
<div runat="server" id="divEdit">
    <table width="100%" cellpadding="10" cellspacing="10">
        <tr>
            <td colspan="2">
                <asp:Label ID="lblInfoEdit" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTen" runat="server" Text="Tên : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtTen" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMoTa" runat="server" Text="Mô tả : "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtMoTa" TextMode="MultiLine" runat="server" Width="100%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblNgay" runat="server" Text="Ngày: "></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtNgay" runat="server" Width="100px"></asp:TextBox>
                <asp:HyperLink ID="lnkNgay" runat="server">
                    <asp:Image ID="imgNgay" runat="server" resourceKey="CalendarAlt.Text" />
                </asp:HyperLink>
                (dd/mm/yyyy)
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblBatDau" runat="server" Text="Bắt đầu : "></asp:Label>
            </td>
            <td>
                <table>
                    <tr>
                        <td>Giờ:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlGioBatDau" runat="server" AutoPostBack="false">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Phút:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPhutBatDau" runat="server" AutoPostBack="false">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblKetThuc" runat="server" Text="Kết thúc : "></asp:Label>
            </td>
            <td>
                <table>
                    <tr>
                        <td>Giờ:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlGioKetThuc" runat="server" AutoPostBack="false">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Phút:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPhutKetThuc" runat="server" AutoPostBack="false">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblKieuBaoMat" runat="server" Text="Bảo mật : "></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlBaoMat" runat="server" AutoPostBack="false">
                    <asp:ListItem Value="1">Bình thường</asp:ListItem>
                    <asp:ListItem Value="2">Bảo mật</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="trAddLopHoc" style="padding-top: 10px; padding-bottom: 10px; margin-top: 10px; margin-bottom: 10px;" runat="server">
            <td colspan="2">
                <table style="width: 100%">
                    <tr>
                        <td colspan="3" style="text-align: center; font-weight: bold; color: mediumblue;">
                            <div>Quản lý lớp học trong ca học</div>
                            <div id="divAnnounce1" runat="server"></div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; color: mediumblue; width: 30%">Danh sách lớp học không trong ca học
                        </td>
                        <td style="text-align: center; width: 30%"></td>
                        <td style="text-align: center; color: mediumblue; width: 40%">Danh sách lớp học trong ca học
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; vertical-align: top; width: 30%">
                            <asp:ListBox Width="100%" Height="100%" ID="lsbDanhSachLopHocChuaTrongKiThi" runat="server"></asp:ListBox>
                        </td>
                        <td style="text-align: center; vertical-align: central; width: 20%">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <span>Phòng học</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPhongHoc" runat="server" AutoPostBack="false">
                                            <asp:ListItem Value="1">Phòng 2</asp:ListItem>
                                            <asp:ListItem Value="2">Phòng 3</asp:ListItem>
                                            <asp:ListItem Value="3">Phòng 4</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span>Mô tả</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMoTaLopHocTrongKiThi" TextMode="MultiLine" runat="server" Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center; vertical-align: central;">
                                        <asp:Button ID="btnChuyenTraiQuanPhai" runat="server" Text=">>>>>" OnClick="btnChuyenTraiQuanPhai_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center; vertical-align: central;">
                                        <asp:Button ID="btnChuyenPhaiQuaTrai" runat="server" Text="<<<<<" OnClick="btnChuyenPhaiQuaTrai_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="text-align: center; vertical-align: top;">
                            <asp:ListBox Width="100%" Height="100%" ID="lsbDanhSachLopHocTrongKiThi" runat="server"></asp:ListBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <asp:Button ID="btnUpdate" runat="server" Text="Cập nhật" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnSinhVienTrongLopHoc" runat="server" Text="Quản lý sinh viên" OnClick="btnQLSV_Click" />
                <asp:Button ID="btnQuayLai" runat="server" Text="Quay lại" OnClick="btnQuayLai_Click" />

            </td>
        </tr>
    </table>
</div>
<div style="display: none;">
    <asp:TextBox ID="txtType" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtNguoiDung_MonHocID" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtBlockHocID" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
    <asp:TextBox ID="txtStatus" runat="server"></asp:TextBox>
</div>
