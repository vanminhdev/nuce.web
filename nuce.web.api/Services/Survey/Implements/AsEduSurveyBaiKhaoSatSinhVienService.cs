using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Survey.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Survey.Implements
{
    public class AsEduSurveyBaiKhaoSatSinhVienService : IAsEduSurveyBaiKhaoSatSinhVienService
    {
        private readonly SurveyContext _context;

        public AsEduSurveyBaiKhaoSatSinhVienService(SurveyContext context)
        {
            _context = context;
        }

        public async Task<Guid> GetIdByCode(string studentCode, string classroomCode)
        {
            var studentSurvey = await _context.AsEduSurveyBaiKhaoSatSinhVien.FirstOrDefaultAsync(o => o.ClassRoomCode == classroomCode);
            if (studentSurvey == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài làm");
            }
            return studentSurvey.Id;
        }

        public async Task GenerateTheSurveyStudent()
        {
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


        /// <summary>
        /// Lưu bài làm
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public async Task SaveTask(string id, string task, string ipAddress)
        {
            var surveyStudent = await _context.AsEduSurveyBaiKhaoSatSinhVien.FirstOrDefaultAsync(o => o.Id.ToString() == id && o.Status != (int)SurveyStudentStatus.Done);
            if(surveyStudent == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bài làm");
            }
            surveyStudent.BaiLam = task.Trim();
            surveyStudent.Type = (int)SurveyStudentStatus.Done;
            surveyStudent.LogIp = ipAddress;
            await _context.SaveChangesAsync();
        }
    }
}
