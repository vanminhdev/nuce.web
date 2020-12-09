using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Status;
using nuce.web.api.Models.Survey;
using nuce.web.api.Models.Survey.JsonData;
using nuce.web.api.Services.Survey.Interfaces;
using nuce.web.api.ViewModel.Survey;
using nuce.web.api.ViewModel.Survey.Graduate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements
{
    public class AsEduSurveyGraduateBaiKhaoSatSinhVienService : IAsEduSurveyGraduateBaiKhaoSatSinhVienService
    {
        private readonly SurveyContext _context;
        private readonly StatusContext _statusContext;

        public AsEduSurveyGraduateBaiKhaoSatSinhVienService(SurveyContext context, StatusContext statusContext)
        {
            _context = context;
            _statusContext = statusContext;
        }

        public async Task<string> GetTheSurveyContent(string studentCode, string id)
        {
            var baiKSSinhVien = await _context.AsEduSurveyGraduateBaiKhaoSatSinhVien.Where(o => o.StudentCode == studentCode).ToListAsync();
            if(baiKSSinhVien.FirstOrDefault(o => o.BaiKhaoSatId.ToString() == id) == null)
            {
                throw new RecordNotFoundException("Sinh viên không có bài khảo sát này");
            }

            var theSurvey = await _context.AsEduSurveyGraduateBaiKhaoSat.FirstOrDefaultAsync(o => o.Id.ToString() == id);
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

        public async Task<List<GraduateTheSurveyStudent>> GetTheSurvey(string studentCode)
        {
            return await _context.AsEduSurveyGraduateBaiKhaoSatSinhVien.Where(o => o.StudentCode == studentCode)
                .Join(_context.AsEduSurveyGraduateBaiKhaoSat, o => o.BaiKhaoSatId, o => o.Id, (baikssv, baiks) => new { baikssv, baiks })
                .Select(o => new GraduateTheSurveyStudent
                {
                    Id = o.baikssv.Id,
                    BaiKhaoSatId = o.baikssv.BaiKhaoSatId,
                    DepartmentCode = o.baikssv.DepartmentCode,
                    Name = o.baiks.Name,
                    Type = o.baikssv.Type,
                    Status = o.baikssv.Status,
                })
                .ToListAsync();
        }

        public async Task<int> GetGenerateTheSurveyStudentStatus()
        {
            var status = await _statusContext.AsStatusTableTask.FirstOrDefaultAsync(o => o.TableName == "AS_Edu_Survey_Graduate_BaiKhaoSat_SinhVien");
            if (status == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bản ghi cập nhật trạng thái cho bảng bài khảo sát sinh viên");
            }
            return status.Status;
        }

        public async Task GenerateTheSurveyStudent(Guid theSurveyId)
        {
            var status = await _statusContext.AsStatusTableTask.FirstOrDefaultAsync(o => o.TableName == "AS_Edu_Survey_Graduate_BaiKhaoSat_SinhVien");
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

            var theSurvey = await _context.AsEduSurveyGraduateBaiKhaoSat.FirstOrDefaultAsync(o => o.Id == theSurveyId && o.Status == (int)TheSurveyStatus.New);
            if (theSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát");
            }

            try
            {
                IQueryable<AsEduSurveyGraduateStudent> query = _context.AsEduSurveyGraduateStudent.OrderBy(o => o.Id);
                var numStudent = query.Count();
                var skip = 0;
                var take = 500;

                List<AsEduSurveyGraduateStudent> students;
                while (skip <= numStudent)
                {
                    students = await query.Skip(skip).Take(take).ToListAsync();
                    foreach (var student in students)
                    {
                        //nếu chưa có thì thêm
                        if( await _context.AsEduSurveyBaiKhaoSatSinhVien.FirstOrDefaultAsync(o => o.BaiKhaoSatId == theSurvey.Id && o.StudentCode == student.ExMasv) == null )
                        {
                            _context.AsEduSurveyGraduateBaiKhaoSatSinhVien.Add(new AsEduSurveyGraduateBaiKhaoSatSinhVien
                            {
                                Id = Guid.NewGuid(),
                                BaiKhaoSatId = theSurvey.Id,
                                DepartmentCode = student?.Manganh ?? "",
                                StudentCode = student?.ExMasv ?? "",
                                DeThi = "",
                                BaiLam = "",
                                NgayGioBatDau = DateTime.Now,
                                NgayGioNopBai = DateTime.Now,
                                Status = (int)SurveyStudentStatus.DoNot,
                                Type = 1,
                            });
                        }
                    }
                    await _context.SaveChangesAsync();
                    skip += take;
                }
            }
            catch (Exception e)
            {
                status.Status = (int)TableTaskStatus.Done;
                status.IsSuccess = false;
                status.Message = UtilsException.GetMainMessage(e);
                await _statusContext.SaveChangesAsync();
                throw e;
            }
            //chuyển trạng thái thành đã xuất bản tức đã có dữ liệu trong bảng bài khảo sát sinh viên
            theSurvey.Status = (int)TheSurveyStatus.Published;
            await _context.SaveChangesAsync();

            status.Status = (int)TableTaskStatus.Done;
            status.IsSuccess = true;
            await _statusContext.SaveChangesAsync();
        }

        public async Task<string> GetSelectedAnswerAutoSave(Guid theSurveyId, string studentCode)
        {
            var surveyStudent = await _context.AsEduSurveyGraduateBaiKhaoSatSinhVien
                .FirstOrDefaultAsync(o => o.BaiKhaoSatId == theSurveyId && o.StudentCode == studentCode && o.Status != (int)SurveyStudentStatus.Done && o.Status != (int)SurveyStudentStatus.Close);
            if (surveyStudent == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài khảo sát sinh viên");
            }
            return surveyStudent.BaiLam;
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
        public async Task AutoSave(Guid theSurveyId, string studentCode, string questionCode, string answerCode, string answerCodeInMulSelect, string answerContent, bool isAnswerCodesAdd = true)
        {
            //bài khảo sát sinh viên trạng thái không phải là đã hoàn thành hoặc bị close do kết thúc đợt khảo sát
            var surveyStudent = await _context.AsEduSurveyGraduateBaiKhaoSatSinhVien
                .FirstOrDefaultAsync(o => o.BaiKhaoSatId == theSurveyId && o.StudentCode == studentCode && o.Status != (int)SurveyStudentStatus.Done && o.Status != (int)SurveyStudentStatus.Close);
            if (surveyStudent == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài làm");
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
        /// Lưu bài làm sau khi hoàn thành
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task SaveSelectedAnswer(Guid theSurveyId, string studentCode, string ipAddress)
        {
            var surveyStudent = await _context.AsEduSurveyGraduateBaiKhaoSatSinhVien
                .FirstOrDefaultAsync(o => o.BaiKhaoSatId == theSurveyId && o.StudentCode == studentCode && o.Status != (int)SurveyStudentStatus.Done && o.Status != (int)SurveyStudentStatus.Close);
            if (surveyStudent == null)
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
