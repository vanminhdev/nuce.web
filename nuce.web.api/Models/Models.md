1. tổ chức context
	1.1. context identity core: đăng nhập, log
		- gồm các bảng:
			AspNet*
			Log
		- cli:
	1.2. context core: cấu hình
		- cli:
		 dotnet ef dbcontext scaffold "Data Source=.\sqlexpress;Initial Catalog=NUCE_CORE;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -o Models/Core -c NuceCoreContext -f -t ManagerBackup
	1.3. context edu data: những dữ liệu lấy từ đào tạo
		- cli: 
			dotnet ef dbcontext scaffold "Data Source=.\sqlexpress;Initial Catalog=NUCE_SURVEY;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -o Models/EduData -c EduDataContext -f -t AS_Academy_Faculty -t AS_Academy_Department -t AS_Academy_Academics -t AS_Academy_Subject -t AS_Academy_Class -t AS_Academy_Lecturer -t AS_Academy_Student -t AS_Academy_ClassRoom -t AS_Academy_Lecturer_ClassRoom -t AS_Academy_Semester -t AS_Academy_Student_ClassRoom -t AS_Academy_C_ClassRoom -t AS_Academy_C_Lecturer_ClassRoom -t AS_Academy_C_Student_ClassRoom
		- gồm các bảng:
			AS_Academy_faculty
			AS_Academy_
	1.4. context survey: những dữ liệu liên quan đến khảo thí như câu hỏi,...
		- gồm các bảng:
	1.5. context Ctsv: công tác sinh viên

2. công cụ
	dotnet tool install --global dotnet-ef
	dotnet tool update --global dotnet-ef