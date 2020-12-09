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

        public async Task<List<TheSurveyStudent>> GetTheSurvey(string studentCode)
        {
            return await _context.AsEduSurveyBaiKhaoSatSinhVien.Where(o => o.StudentCode == studentCode)
                .Select(o => new TheSurveyStudent
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

            try
            {
                var queryTheSurvey = _context.AsEduSurveyBaiKhaoSat.Where(o => o.Status == (int)TheSurveyStatus.New);
                AsEduSurveyBaiKhaoSat temp = null;
                temp = queryTheSurvey.FirstOrDefault(o => o.Type == (int)TheSurveyType.TheoreticalSubjects);
                if (temp == null)
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
            catch (Exception e)
            {
                status.Status = (int)TableTaskStatus.Done;
                status.IsSuccess = false;
                status.Message = e.Message;
                await _statusContext.SaveChangesAsync();
                throw e;
            }
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

                if (list.Count == 0) //nếu k còn phần tử nào thì bỏ hẳn
                    return null;
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
        public async Task AutoSave(string id, string questionCode, string answerCode, string answerCodeInMulSelect, string answerContent, bool isAnswerCodesAdd = true)
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
                    if (answerCode != null) //lựa chọn chọn 1
                    {
                        item.AnswerCode = answerCode.Trim();
                    }
                    else if (answerCodeInMulSelect != null) // lựa chọn chọn nhiều
                    {
                        item.AnswerCodes = AddOrRemoveAnswerCodes(item.AnswerCodes, answerCodeInMulSelect, isAnswerCodesAdd);
                    }
                    item.AnswerContent = answerContent; // câu trả lời text
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

                if(questionCode.Split('_').Length == 2) //là câu hỏi con
                {
                    newSelectedAnswer.IsAnswerChildQuestion = true;
                }

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
            //kiểm tra đủ số câu hỏi bắt buộc
            var theSurvey = await _context.AsEduSurveyBaiKhaoSat.FirstOrDefaultAsync(o => o.Id == surveyStudent.BaiKhaoSatId);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát");
            }

            var examQuestions = await _context.AsEduSurveyDeThi.FirstOrDefaultAsync(o => o.Id == theSurvey.DeThiId);
            if (examQuestions == null)
            {
                throw new RecordNotFoundException("Không tìm thấy nội dung bài khảo sát");
            }

            var questions = JsonSerializer.Deserialize<List<QuestionJson>>(examQuestions.NoiDungDeThi);
            var answerSave = JsonSerializer.Deserialize<List<SelectedAnswer>>(surveyStudent.BaiLam);

            var test = questions.Where(o => o.Type == QuestionType.SC || o.Type == QuestionType.MC).ToList();
            foreach (var q in questions)
            {
                //Câu hỏi ngắn không bắt buộc
                if(q.Type == QuestionType.SC && answerSave.FirstOrDefault(o => o.QuestionCode == q.Code && o.AnswerCode != null) == null)
                {
                    throw new InvalidDataException("Chưa trả lời đủ số câu hỏi");
                }
                else if (q.Type == QuestionType.MC && answerSave.FirstOrDefault(o => o.QuestionCode == q.Code && o.AnswerCodes != null && o.AnswerCodes.Count > 0) == null)
                {
                    throw new InvalidDataException("Chưa trả lời đủ số câu hỏi");
                }
            }

            surveyStudent.NgayGioNopBai = DateTime.Now;
            surveyStudent.Status = (int)SurveyStudentStatus.Done;
            surveyStudent.LogIp = ipAddress;
            await _context.SaveChangesAsync();
        }
    }
}
