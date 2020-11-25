using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Status;
using nuce.web.api.Models.Survey;
using nuce.web.api.Models.Survey.JsonData;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements
{
    public class AsEduSurveyBaiKhaoSatSinhVienService : IAsEduSurveyBaiKhaoSatSinhVienService
    {
        private readonly SurveyContext _context;
        private readonly StatusContext _statusContext;

        public AsEduSurveyBaiKhaoSatSinhVienService(SurveyContext context, StatusContext statusContext)
        {
            _context = context;
            _statusContext = statusContext;
        }

        public async Task<string> GetTheSurveyJsonStringByBaiKhaoSatId(string id)
        {
            var theSurvey = await _context.AsEduSurveyBaiKhaoSat.FirstOrDefaultAsync(o => o.Id.ToString() == id);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát");
            }

            var examQuestions = await _context.AsEduSurveyDeThi.FirstOrDefaultAsync(o => o.Id == theSurvey.DeThiId);
            if (examQuestions == null)
            {
                throw new RecordNotFoundException("Không tìm thấy nội dung bài khảo sát");
            }

            return examQuestions.NoiDungDeThi;
        }

        public async Task<List<TheSurveysStudent>> GetTheSurveys(string studentCode)
        {
            return await _context.AsEduSurveyBaiKhaoSatSinhVien.Where(o => o.StudentCode == studentCode)
                .Select(o => new TheSurveysStudent
                {
                    Id = o.Id,
                    BaiKhaoSatId = o.BaiKhaoSatId,
                    StudentCode = o.SubjectCode,
                    LecturerCode = o.LecturerCode,
                    LecturerName = o.LecturerName,
                    ClassRoomCode = o.ClassRoomCode,
                    SubjectCode = o.SubjectCode,
                    SubjectName = o.SubjectName,
                    SubjectType = o.SubjectType,
                    DepartmentCode = o.DepartmentCode,
                    Type = o.Type,
                    Status = o.Status,
                })
                .ToListAsync();
        }

        public async Task<Guid> GetIdByCode(string studentCode, string classroomCode)
        {
            var studentSurvey = await _context.AsEduSurveyBaiKhaoSatSinhVien.FirstOrDefaultAsync(o => o.ClassRoomCode == classroomCode && o.StudentCode == studentCode);
            if (studentSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát sinh viên");
            }
            return studentSurvey.Id;
        }

        public async Task<int> GetGenerateTheSurveyStudentStatus()
        {
            var status = await _statusContext.AsStatusTableTask.FirstOrDefaultAsync(o => o.TableName == "AS_Edu_Survey_BaiKhaoSat_SinhVien");
            if (status == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bản ghi cập nhật trạng thái cho bảng bài khảo sát sinh viên");
            }
            return status.Status;
        }

        public async Task GenerateTheSurveyStudent()
        {
            var status = await _statusContext.AsStatusTableTask.FirstOrDefaultAsync(o => o.TableName == "AS_Edu_Survey_BaiKhaoSat_SinhVien");
            if(status == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bản ghi cập nhật trạng thái cho bảng bài khảo sát sinh viên");
            }

            //bảng đang làm việc
            if (status.Status == (int)TableTaskStatus.Doing)
            {
                throw new TableBusyException("Đang tạo bài khảo sát cho từng sinh viên, thao tác bị huỷ");
            }

            status.Status = (int)TableTaskStatus.Doing;
            await _statusContext.SaveChangesAsync();

            var queryTheSurvey =  _context.AsEduSurveyBaiKhaoSat.Where(o => o.Status == (int)TheSurveyStatus.Active);
            AsEduSurveyBaiKhaoSat temp = null;
            temp = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.TheoreticalSubjects);
            if(temp == null)
            {
                throw new RecordNotFoundException("Không có bài khảo sát chuẩn bị cho môn lý thuyết");
            }
            var idLyThuyet = new SqlParameter("@BaiKSLoai1", temp.Id);

            temp = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.PracticalSubjects);
            if (temp == null)
            {
                throw new RecordNotFoundException("Không có bài khảo sát chuẩn bị cho môn thực hành, thực tập, thí nghiệm");
            }
            var idThucHanhThucTap = new SqlParameter("@BaiKSLoai2", temp.Id);

            temp = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.TheoreticalPracticalSubjects);
            if (temp == null)
            {
                throw new RecordNotFoundException("Không có bài khảo sát chuẩn bị cho môn lý thuyết + thực hành");
            }
            var idLyThuyetThucHanh = new SqlParameter("@BaiKSLoai3", temp.Id);

            temp = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.AssignmentSubjects);
            if (temp == null)
            {
                throw new RecordNotFoundException("Không có bài khảo sát chuẩn bị cho môn đồ án");
            }
            var idDoAn = new SqlParameter("@BaiKSLoai4", temp.Id);

            temp = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.DefaultSubjects);
            if (temp == null)
            {
                throw new RecordNotFoundException("Không có bài khảo sát chuẩn bị cho môn không được phân loại");
            }
            var idMacDinh = new SqlParameter("@BaiKSLoaiMacDinh", temp.Id);
            await _context.Database.ExecuteSqlRawAsync("exec generate_the_survey_student @BaiKSLoai1, @BaiKSLoai2, @BaiKSLoai3, @BaiKSLoai4, @BaiKSLoaiMacDinh", idLyThuyet, idThucHanhThucTap, idLyThuyetThucHanh, idDoAn, idMacDinh);
        }

        public async Task<string> GetSelectedAnswerAutoSave(string studentCode, string classroomCode)
        {
            var studentSurvey = await _context.AsEduSurveyBaiKhaoSatSinhVien.FirstOrDefaultAsync(o => o.ClassRoomCode == classroomCode && o.StudentCode == studentCode);
            if (studentSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát sinh viên");
            }
            return studentSurvey.BaiLam;
        }

        private List<string> AddOrRemoveAnswerCodes (List<string> list,  string answerCodeInMulSelect, bool isAnswerCodesAdd)
        {
            if (isAnswerCodesAdd) //thêm
            {
                if (list != null && !list.Contains(answerCodeInMulSelect)) //chưa có đáp án chọn nhiều này
                {
                    list.Add(answerCodeInMulSelect.Trim());
                }
                else if (list == null) //chưa có bất kì đáp án chọn nhiều nào
                {
                    list = new List<string>() { answerCodeInMulSelect };
                }
            }
            else //bỏ
            {
                if (list != null && list.Contains(answerCodeInMulSelect)) //có đáp án chọn nhiều này
                {
                    list.Remove(answerCodeInMulSelect.Trim());
                }
            }
            return list;
        }

        /// <summary>
        /// Tự động lưu khi click
        /// </summary>
        /// <param name="id"></param>
        /// <param name="questionCode"></param>
        /// <param name="answerCode"></param>
        /// <param name="answerCodeInMulSelect"></param>
        /// <param name="isAnswerCodesAdd">là thêm đáp án lựa chọn nhiều hay bỏ đi</param>
        /// <param name="answerContent"></param>
        /// <returns></returns>
        public async Task AutoSave(string id, string questionCode, string answerCode, string answerCodeInMulSelect, bool isAnswerCodesAdd ,string answerContent)
        {
            var surveyStudent = await _context.AsEduSurveyBaiKhaoSatSinhVien.FirstOrDefaultAsync(o => o.Id.ToString() == id && o.Status != (int)SurveyStudentStatus.Done);
            if (surveyStudent == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài làm");
            }

            if (surveyStudent.Status == (int)SurveyStudentStatus.Done)
            {
                return;
            }

            List<SelectedAnswer> list;
            try
            {
                list = JsonSerializer.Deserialize<List<SelectedAnswer>>(surveyStudent.BaiLam);
            } 
            catch
            {
                list = new List<SelectedAnswer>();
            }

            var exsist = false; //đã tồn tại câu hỏi chưa
            //cập nhật cho câu hỏi tương ứng
            foreach (var item in list)
            {
                if (item.QuestionCode == questionCode)
                {
                    if (answerCode != null)
                    {
                        item.AnswerCode = answerCode.Trim();
                    }
                    else if (answerCodeInMulSelect != null) // lựa chọn chọn nhiều
                    {
                        item.AnswerCodes = AddOrRemoveAnswerCodes(item.AnswerCodes, answerCodeInMulSelect, isAnswerCodesAdd);
                    }
                    item.AnswerContent = answerContent;
                    exsist = true;
                    break;
                }
            }
            //thêm mới cho câu hỏi tương ứng
            if (!exsist)
            {
                var newSelectedAnswer = new SelectedAnswer
                {
                    QuestionCode = questionCode,
                    AnswerCode = answerCode,
                    AnswerContent = answerContent
                };

                if (answerCodeInMulSelect != null && isAnswerCodesAdd) // lựa chọn chọn nhiều
                {
                    newSelectedAnswer.AnswerCodes = new List<string>() { answerCodeInMulSelect };
                }

                list.Add(newSelectedAnswer);
            }

            var options = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };
            surveyStudent.BaiLam = JsonSerializer.Serialize(list, options);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Lưu bài làm
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task SaveSelectedAnswer(string id, string ipAddress)
        {
            var surveyStudent = await _context.AsEduSurveyBaiKhaoSatSinhVien.FirstOrDefaultAsync(o => o.Id.ToString() == id && o.Status != (int)SurveyStudentStatus.Done);
            if(surveyStudent == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài làm");
            }
            if(surveyStudent.Status == (int)SurveyStudentStatus.Done)
            {
                return;
            }
            surveyStudent.Status = (int)SurveyStudentStatus.Done;
            surveyStudent.LogIp = ipAddress;
            await _context.SaveChangesAsync();
        }
    }
}
