1. tổ chức context
	1.1. context identity core: đăng nhập, log
		- gồm các bảng:
			AspNet*
			Log
		- cli:
	1.2. context core: cấu hình
		- cli:
			dotnet ef dbcontext scaffold "Data Source=.\sqlexpress;Initial Catalog=NUCE_CORE;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -o Models/Core -c NuceCoreContext -f -t ManagerBackup -t News_Cats -t News_Items -t News_Cat_Item -t FileUpload -t ClientParameters
	1.3. context edu data: những dữ liệu lấy từ đào tạo
		- cli: 
			dotnet ef dbcontext scaffold "Data Source=.\sqlexpress;Initial Catalog=NUCE_SURVEY;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -o Models/EduData -c EduDataContext -f -t AS_Academy_Faculty -t AS_Academy_Department -t AS_Academy_Academics -t AS_Academy_Subject -t AS_Academy_Class -t AS_Academy_Lecturer -t AS_Academy_Student -t AS_Academy_ClassRoom -t AS_Academy_Lecturer_ClassRoom -t AS_Academy_Semester -t AS_Academy_Student_ClassRoom -t AS_Academy_C_ClassRoom -t AS_Academy_C_Lecturer_ClassRoom -t AS_Academy_C_Student_ClassRoom -t AS_Academy_Subject_Extend
		- gồm các bảng:
			AS_Academy_faculty
			AS_Academy_
	1.4. context SURVEY: những dữ liệu liên quan đến khảo thí như câu hỏi,...
		- gồm các bảng:
		- cli:
			dotnet ef dbcontext scaffold "Data Source=.\sqlexpress;Initial Catalog=NUCE_SURVEY;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -o Models/Survey -c SurveyContext -f -t AS_Edu_Survey_BaiKhaoSat -t AS_Edu_Survey_BaiKhaoSat_SinhVien -t AS_Edu_Survey_CauHoi -t AS_Edu_Survey_CauTrucDe -t AS_Edu_Survey_DapAn -t AS_Edu_Survey_DeThi -t AS_Edu_Survey_Undergraduate_CauHoi -t AS_Edu_Survey_Undergraduate_CauTrucDe -t AS_Edu_Survey_Undergraduate_DapAn -t AS_Edu_Survey_Undergraduate_DeThi -t AS_Edu_Survey_Graduate_CauHoi -t AS_Edu_Survey_Graduate_CauTrucDe -t AS_Edu_Survey_Graduate_DapAn -t AS_Edu_Survey_Graduate_DeThi -t AS_Edu_Survey_DotKhaoSat -t AS_Edu_Survey_ReportTotal -t AS_Edu_Survey_Graduate_SurveyRound -t AS_Edu_Survey_Graduate_Student -t AS_Edu_Survey_Graduate_BaiKhaoSat -t AS_Edu_Survey_Graduate_BaiKhaoSat_SinhVien -t AS_Edu_Survey_Undergraduate_SurveyRound -t AS_Edu_Survey_Undergraduate_Student -t AS_Edu_Survey_Undergraduate_BaiKhaoSat -t AS_Edu_Survey_Undergraduate_BaiKhaoSat_SinhVien -t AS_Edu_Survey_Undergraduate_ReportTotal
		- thư mục jsondata: là các model đưa dữ liệu đã query về dạng json string
			
	1.5. context Ctsv: công tác sinh viên
		- cli:
			dotnet ef dbcontext scaffold "Data Source=.\sqlexpress;Initial Catalog=CTSVNUCE_DATA;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -o Models/Ctsv -c CTSVNUCE_DATAContext -f --project nuce.web.api
	1.6. context status: lưu trạng thái làm việc của các bảng mất nhiều thời gian
		-cli:
			dotnet ef dbcontext scaffold "Data Source=.\sqlexpress;Initial Catalog=NUCE_SURVEY;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -o Models/Status -c StatusContext -f -t AS_Status_Table_Task -t AS_Academy_Semester

2. công cụ
	dotnet tool install --global dotnet-ef
	dotnet tool update --global dotnet-ef

3. xem database diagram
	-lỗi dbo
		use [YourDatabaseName] EXEC sp_changedbowner 'sa'