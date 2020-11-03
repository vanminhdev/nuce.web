<%@ control language="C#" autoeventwireup="true" codebehind="QuanLyThongTinDotTraoDoi.ascx.cs" inherits="nuce.web.qlhpm.QuanLyThongTinDotTraoDoi" %>
<div runat="server" id="divAnnouce" style="font-weight: bold; color: red; text-align: center;"></div>
<div runat="server" id="divDotTraoDoi" style="font-weight: bold; color: red; text-align: center;"></div>
<div runat="server" id="divContent">
</div>

    <script>
        function nopbai(id,userid)
        {
            $.post("/handler/nuce.web.qlhpm/CapNhatTrangThaiBaiNopSinhVien.aspx",
               { id: id, status: 1, userid: userid, action: "capnhat" },
               function (data) {
                   alert(data);
                   alert("Huỷ bài thành công");
                   location.reload();
               });
        };
    </script>
