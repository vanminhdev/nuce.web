﻿alter table [AS_Academy_Faculty]
add [Order] int null
go

update [AS_Academy_Faculty]
set [Order] = 1 where Code = 'KX'

update [AS_Academy_Faculty]
set [Order] = 2 where Code = 'KC'

update [AS_Academy_Faculty]
set [Order] = 3 where Code = 'KD'

update [AS_Academy_Faculty]
set [Order] = 4 where Code = 'KT'

update [AS_Academy_Faculty]
set [Order] = 5 where Code = 'KV'

update [AS_Academy_Faculty]
set [Order] = 6 where Code = 'CT'

update [AS_Academy_Faculty]
set [Order] = 7 where Code = 'KM'

update [AS_Academy_Faculty]
set [Order] = 8 where Code = 'IT'

update [AS_Academy_Faculty]
set [Order] = 9 where Code = 'MT'

update [AS_Academy_Faculty]
set [Order] = 10 where Code = 'CB'

update [AS_Academy_Faculty]
set [Order] = 11 where Code = 'KS'

update [AS_Academy_Faculty]
set [Order] = 12 where Code = 'ML'

update [AS_Academy_Faculty]
set [Order] = 13 where Code = 'QP'

update [AS_Academy_Faculty]
set [Order] = 14 where Code = 'QT'

update [AS_Academy_Faculty]
set [Order] = 15 where Code = 'GH'

update [AS_Academy_Faculty]
set [Order] = 16 where Code = 'DT'

update [AS_Academy_Faculty]
set [Order] = 17 where Code = 'TC'

update [AS_Academy_Faculty]
set [Order] = 18 where Code = '*'


--cập nhật lại ngành chuyên ngành trong bảng bài làm của sinh viên trước tốt nghiệp

update AS_Edu_Survey_Undergraduate_BaiKhaoSat_SinhVien
set Nganh = (select tennganh from AS_Edu_Survey_Undergraduate_Student where AS_Edu_Survey_Undergraduate_BaiKhaoSat_SinhVien.StudentCode = AS_Edu_Survey_Undergraduate_Student.ex_masv)

update AS_Edu_Survey_Undergraduate_BaiKhaoSat_SinhVien
set ChuyenNganh = (select tenchnga from AS_Edu_Survey_Undergraduate_Student where AS_Edu_Survey_Undergraduate_BaiKhaoSat_SinhVien.StudentCode = AS_Edu_Survey_Undergraduate_Student.ex_masv)


--cập nhật lại bảng report total
alter table AS_Edu_Survey_ReportTotal
add SubjectCode varchar(50)

alter table AS_Edu_Survey_ReportTotal
add ClassRoom varchar(50)

alter table AS_Edu_Survey_Undergraduate_BaiKhaoSat
alter column [Status] INT not NULL