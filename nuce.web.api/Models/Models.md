1. tổ chức context
	1.1. context core: đăng nhập, log
		- gồm các bảng:
			AspNet*
			Log
	1.2. context edu data: những dữ liệu lấy từ đào tạo
		dotnet ef dbcontext scaffold "Data Source=inspiron\sqlexpress;Initial Catalog=NUCE_SURVEY;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -o Models/EduData -c EduDataContext -f -t AS_Academy_faculty
		- gồm các bảng:
			AS_Academy_faculty
			AS_Academy_
	1.3. context survey: những dữ liệu liên quan đến khảo thí như câu hỏi,...
		- gồm các bảng:
	1.4. context Ctsv: công tác sinh viên