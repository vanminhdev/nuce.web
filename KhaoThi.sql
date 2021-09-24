/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [ID]
      ,[SurveyRoundId]
      ,[TheSurveyId]
      ,[ChuyenNganh]
      ,[QuestionCode]
      ,[AnswerCode]
      ,[Total]
      ,[Content]
  FROM [NUCE_SURVEY_PUBLISH].[dbo].[AS_Edu_Survey_Undergraduate_ReportTotal]

  select sum(t.Total) from [AS_Edu_Survey_Undergraduate_ReportTotal] as t
  where QuestionCode = '00059' and AnswerCode = '00234'


  select t.ChuyenNganh, SUM(t.Total) from [AS_Edu_Survey_Undergraduate_ReportTotal] as t
  where QuestionCode = '00059' and AnswerCode = '00234'
  group by ChuyenNganh
  order by ChuyenNganh

  select distinct a.BaiLam, a.StudentCode, a.NgayGioNopBai, a.Nganh, a.ChuyenNganh, b.tennganh, b.tenchnga from AS_Edu_Survey_Undergraduate_BaiKhaoSat_SinhVien as a
  inner join AS_Edu_Survey_Undergraduate_Student as b on a.StudentCode = b.ex_masv
  where (b.tennganh = N'Kiến trúc' or b.tenchnga = N'Kiến trúc') and a.Status = 5 and NgayGioNopBai >= '2020-11-30'

  select * from AS_Edu_Survey_Undergraduate_BaiKhaoSat_SinhVien as a
  where a.StudentCode = '2010260'


  select tennganh, tenchnga from  AS_Edu_Survey_Undergraduate_Student

  select * from AS_Edu_Survey_Undergraduate_BaiKhaoSat_SinhVien
  where NgayGioNopBai >= '2020-11-30' and ( Nganh = N'Kiến trúc' or ChuyenNganh = N'Kiến trúc') and Status = 5


  select sum(total) from AS_Edu_Survey_Undergraduate_ReportTotal
  where ChuyenNganh = N'Kiến trúc' and QuestionCode = '00059'


    select * from AS_Edu_Survey_Undergraduate_ReportTotal
  where ChuyenNganh = N'Kiến trúc' and QuestionCode = '00059'


  select distinct tennganh from AS_Edu_Survey_Undergraduate_Student
  where tennganh = N'Kiến trúc'

  select distinct tennganh from AS_Edu_Survey_Undergraduate_Student

  select distinct ChuyenNganh from AS_Edu_Survey_Undergraduate_BaiKhaoSat_SinhVien


  select count(*) from AS_Edu_Survey_Undergraduate_BaiKhaoSat_SinhVien
  where Nganh = N'Kỹ thuật xây dựng Công trình Giao thông' and NgayGioNopBai >= '2020-11-30' and NgayGioNopBai <= '2021-06-24' and Status = 5


  select BaiLam from AS_Edu_Survey_Undergraduate_BaiKhaoSat_SinhVien
  where Nganh = N'Kỹ thuật xây dựng Công trình Giao thông' and NgayGioNopBai >= '2020-11-30' and NgayGioNopBai <= '2021-06-24' and Status = 5


  select Count(*) from AS_Edu_Survey_Undergraduate_BaiKhaoSat_SinhVien
  where Nganh = N'Kỹ thuật xây dựng Công trình Giao thông' and NgayGioNopBai >= '2020-11-30' and NgayGioNopBai <= '2021-06-24' and Status = 5
  and BaiLam not like N'%"QuestionCode":"00059"%'


  select sum(a.Total) from AS_Edu_Survey_Undergraduate_ReportTotal as a
  where QuestionCode = '00059' and ChuyenNganh = N'Kỹ thuật xây dựng Công trình Giao thông'