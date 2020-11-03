<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NUCE_SO_Admin_Menu_Left1.ascx.cs"
    Inherits="DotNetNuke.UI.Skins.Controls.NUCE_SO_Admin_Menu_Left1" %>
<div class="category1 fl" id="divContent" runat="server">
    Danh mục chức năng
			    <ul id="ulMenuLeft" class="filetree">
                    <li class="closed"><span class="folder">Quản lý chung</span>
                        <ul>
                            <li>
                                <a href="/tabid/89/default.aspx"><span class="file">Thông tin trường</span></a>
                            </li>
                            <li>
                                <a href="/tabid/90/default.aspx"><span class="file">Quản lý khoa</span></a>
                            </li>
                            <li>
                                <a href="/tabid/91/default.aspx"><span class="file">Quản lý bộ môn</span></a>
                            </li>
                            <li>
                                <a href="/tabid/92/default.aspx"><span class="file">Quản lý môn học</span></a>
                            </li>
                            <li>
                                <a href="/tabid/93/default.aspx"><span class="file">Quản lý năm học</span></a>
                            </li>
                            <li>
                                <a href="/tabid/94/default.aspx"><span class="file">Quản lý block học</span></a>
                            </li>
                            <li>
                                <a href="/tabid/95/default.aspx"><span class="file">Quản lý lớp</span></a>
                            </li>
                            <li>
                                <a href="/tabid/96/default.aspx"><span class="file">Quản lý lớp học</span></a>
                            </li>
                            <li>
                                <a href="/tabid/98/default.aspx"><span class="file">Quản lý SV trong lớp học</span></a>
                            </li>
                            <li>
                                <a href="/tabid/97/default.aspx"><span class="file">Quản lý SV</span></a>
                            </li>
                            <li>
                                <a href="/tabid/104/default.aspx"><span class="file">Quản lý phòng học</span></a>
                            </li>
                        </ul>
                    </li>
                    <li class="closed"><span class="folder">Quản lý thi trực tuyến</span>
                        <ul>
                             <li>
                                <a href="/tabid/117/default.aspx"><span class="file">DS Dang thi</span></a>
                            </li>
                             <li>
                                <a href="/tabid/118/default.aspx"><span class="file">DS lớp trong kì thi ở TT Kết thúc</span></a>
                            </li>
                            <li>
                                <a href="/tabid/116/default.aspx"><span class="file">DS lớp trong kì thi ở TT bắt đầu</span></a>
                            </li>
                            <li>
                                <a href="/tabid/115/default.aspx"><span class="file">Quản lý kì thi</span></a>
                            </li>
                            <li>
                                <a href="/tabid/114/default.aspx"><span class="file">Quản lý bộ đề</span></a>
                            </li>
                            <li>
                                <a href="/tabid/100/default.aspx"><span class="file">Quản lý bộ câu hỏi</span></a>
                            </li>
                            <li>
                                <a href="/tabid/101/default.aspx"><span class="file">Quản lý câu hỏi</span></a>
                            </li>
                            <li>
                                <a href="/tabid/102/default.aspx"><span class="file">Quản lý thang điểm</span></a>
                            </li>
                            <li>
                                <a href="/tabid/103/default.aspx"><span class="file">Quản lý mức điểm</span></a>
                            </li>
                        </ul>
                    </li>
                    <li class="closed">
                        <span class="folder">Quản lý phòng máy</span>
                        <ul>
                            <%-- <li>
                                <a href="/tabid/106/default.aspx"><span class="file">Tuần hiện tại</span></a>
                            </li>
                            <li>
                                <a href="/tabid/107/default.aspx"><span class="file">Tuần tiếp theo</span></a>
                            </li>
                             <li>
                                <a href="/tabid/109/default.aspx"><span class="file">Lịch sử đăng kí</span></a>
                            </li>--%>
                            <li>
                                <a href="/tabid/112/default.aspx"><span class="file">Lớp điểm danh</span></a>
                            </li>
                            <li>
                                <a href="/tabid/110/default.aspx"><span class="file">Vân tay sinh viên</span></a>
                            </li>
                            <li>
                                <a href="/tabid/105/default.aspx"><span class="file">Thống kê theo lớp học</span></a>
                            </li>
                            <li>
                                <a href="/tabid/105/default.aspx"><span class="file">Thống kê theo giảng viên</span></a>
                            </li>
                            <li>
                                <a href="/tabid/105/default.aspx"><span class="file">Thống kê theo môn học</span></a>
                            </li>
                            <li>
                                <a href="/tabid/105/default.aspx"><span class="file">Thống kê theo sinh viên</span></a>
                            </li>
                        </ul>
                    </li>
                    <li class="closed">
                        <a href="/"><span class="folder">Liên hệ</span></a>
                    </li>
                </ul>
</div>

<script>
    $(document).ready(function () {
        // second example
        $("#ulMenuLeft").treeview({
            animated: "normal",
            persist: "cookie"
        });
    });
</script>
