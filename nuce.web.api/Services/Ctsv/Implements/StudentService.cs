using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using nuce.web.api.Helper;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.Services.Ctsv.Interfaces;
using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Ctsv.Implements
{
    public class StudentService : IStudentService
    {
        #region declare
        private readonly IStudentRepository _studentRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuaTrinhHocRepository _quaTrinhHocRepository;
        private readonly IGiaDinhRepository _giaDinhRepository;
        private readonly IThiHsgRepository _thiHsgRepository;
        private readonly IParameterService _paramService;
        private readonly ILogger<StudentService> _logger;
        private readonly IPathProvider _pathProvider;
        public StudentService(
            IStudentRepository _studentRepository,
            IUserService _userService, IUnitOfWork _unitOfWork,
            IQuaTrinhHocRepository _quaTrinhHocRepository,
            IThiHsgRepository _thiHsgRepository,
            IGiaDinhRepository _giaDinhRepository,
            IParameterService _paramService,
            ILogger<StudentService> _logger,
            IPathProvider _pathProvider
        )
        {
            this._studentRepository = _studentRepository;
            this._unitOfWork = _unitOfWork;
            this._userService = _userService;
            this._quaTrinhHocRepository = _quaTrinhHocRepository;
            this._thiHsgRepository = _thiHsgRepository;
            this._giaDinhRepository = _giaDinhRepository;
            this._paramService = _paramService;
            this._logger = _logger;
            this._pathProvider = _pathProvider;
        }
        #endregion

        public async Task<FullStudentModel> GetFullStudentByCode(string studentCode)
        {
            var quaTrinhHoc = await _quaTrinhHocRepository.FindByCodeAsync(studentCode);
            var giaDinh = await _giaDinhRepository.FindByCodeAsync(studentCode);
            var thiHsg = await _thiHsgRepository.FindByCodeAsync(studentCode);
            var student = _studentRepository.FindByCode(studentCode);
            return new FullStudentModel
            {
                Student = student,
                ThiHSG = thiHsg,
                GiaDinh = giaDinh,
                QuaTrinhHoc = quaTrinhHoc
            };
        }
        /// <summary>
        /// Thông tin bảng student
        /// </summary>
        /// <param name="studentCode"></param>
        /// <returns></returns>
        public AsAcademyStudent GetStudentByCode(string studentCode)
        {
            var student = _studentRepository.FindByCode(studentCode);
            return student;
        }

        public async Task<byte[]> GetStudentAvatar(string code, int? width, int? height)
        {
            string imgPath = $"ANHSV/{code}.jpg";
            string fullImgPath = _pathProvider.MapPathStudentImage(imgPath);
            var dataStream = await FileHelper.DownloadFileAsync(fullImgPath);
            if (width == null || height == null)
            {
                return dataStream.ToArray();
            }
            Image img = Image.FromStream(dataStream);

            Image resizedNewImg = FileHelper.ResizeImage(img, width ?? 0, 2000, false);
            var newImg = FileHelper.CropImage(resizedNewImg, width ?? 0, height ?? 0);

            var result = FileHelper.ImageToByte(newImg);
            return result;
        }

        public StudentAllowUpdateModel GetStudentByCodeAllowUpdate(string studentCode)
        {
            return new StudentAllowUpdateModel
            {
                Student = GetStudentByCode(studentCode),
                Enabled = _paramService.isCapNhatHoSo(),
            };
        }

        public async Task<DataTableResponse<AsAcademyStudent>> FindStudent(string text, int seen, int size)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new DataTableResponse<AsAcademyStudent>
                {
                    Data = new List<AsAcademyStudent>(),
                    RecordsFiltered = 0,
                    RecordsTotal = 0,
                };
            }
            var students = _studentRepository.GetAll().AsNoTracking()
                            .Where(s => s.FulName.Contains(text) || s.ClassCode.Contains(text) ||
                                        s.Code.Contains(text) || s.Email.Contains(text) ||
                                        s.Mobile.Contains(text))
                            .OrderBy(s => s.FulName);
            var takedStudents = students.Skip(seen).Take(size);
            return new DataTableResponse<AsAcademyStudent>
            {
                Data = await takedStudents.ToListAsync(),
                RecordsTotal = students.Count(),
                RecordsFiltered = takedStudents.Count(),
            };
        }

        public async Task<ResponseBody> UpdateStudent(AsAcademyStudent student)
        {
            try
            {
                var currentStudent = _userService.GetCurrentStudent();
                _logger.LogInformation("model: " + JsonConvert.SerializeObject(student).ToString());
                _logger.LogInformation("Thông tin SV: " + JsonConvert.SerializeObject(currentStudent).ToString());

                if (currentStudent == null)
                {
                    currentStudent = _studentRepository.FindByCode(student.Code);
                }

                currentStudent.HkttPhuong = student.HkttPhuong;
                currentStudent.HkttPho = student.HkttPho;
                currentStudent.HkttQuan = student.HkttQuan;
                currentStudent.HkttTinh = student.HkttTinh;
                currentStudent.HkttSoNha = student.HkttSoNha;
                currentStudent.Cmt = student.Cmt;
                currentStudent.CmtNgayCap = student.CmtNgayCap;
                currentStudent.CmtNoiCap = student.CmtNoiCap;
                currentStudent.LaNoiTru = student.LaNoiTru;
                currentStudent.Mobile = student.Mobile;
                currentStudent.Email = student.Email;
                currentStudent.EmailNhaTruong = student.EmailNhaTruong;
                _logger.LogInformation("save change");
                _studentRepository.Update(currentStudent);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Cập nhật thông tin SV: " + $"{ex.ToString()} \n" + JsonConvert.SerializeObject(student).ToString());
                return new ResponseBody { Message = "Lỗi hệ thống" };
            }
            return null;
        }

        public async Task<ResponseBody> UpdateStudentBasic(StudentUpdateModel basicStudent)
        {
            try
            {
                if (!Common.Validate.IsMobile(basicStudent.Mobile.Trim()))
                {
                    return new ResponseBody { Message = "Mobile sai quy cách" };
                }
                else if (!Common.Validate.IsMobile(basicStudent.MobileBaoTin.Trim()))
                {
                    return new ResponseBody { Message = "Số điện thoại người nhận sai quy cách" };
                }
                else if (!Common.Validate.IsValidEmail(basicStudent.Email.Trim()))
                {
                    return new ResponseBody { Message = "Email sai quy cách" };
                }
                else if (!Common.Validate.IsValidEmail(basicStudent.EmailBaoTin.Trim()))
                {
                    return new ResponseBody { Message = "Email người nhận sai quy cách" };
                }

                var student = _userService.GetCurrentStudent();
                student.Email = basicStudent.Email.Trim();
                student.Mobile = basicStudent.Mobile.Trim();
                student.BaoTinDiaChi = basicStudent.DiaChiBaotin.Trim();
                student.BaoTinHoVaTen = basicStudent.HoTenBaoTin.Trim();
                student.BaoTinEmail = basicStudent.EmailBaoTin.Trim();
                student.BaoTinSoDienThoai = basicStudent.MobileBaoTin.Trim();
                student.BaoTinDiaChiNguoiNhan = basicStudent.DiaChiNguoiNhanBaotin.Trim();
                student.LaNoiTru = basicStudent.CoNoiOCuThe;
                student.DiaChiCuThe = basicStudent.DiaChiCuThe.Trim();

                student.HkttPhuong = basicStudent.PhuongXa.Trim();
                student.HkttQuan = basicStudent.QuanHuyen.Trim();
                student.HkttTinh = basicStudent.TinhThanhPho.Trim();
                _studentRepository.Update(student);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Cập nhật thông tin SV: " + $"{ex.ToString()} \n" + JsonConvert.SerializeObject(basicStudent).ToString());
                return new ResponseBody { Message = "Lỗi hệ thống", Data = ex };
            }

            return null;
        }

        public async Task<string> UpdateStudentImage(IFormFile formFile, string studentCode)
        {
            if (!FileHelper.isValidImageUpload(formFile))
            {
                throw new Exception("Ảnh không hợp lệ");
            }
            // 1mb
            long maxSize = 1024 * 1024;
            if (!FileHelper.isValidSize(formFile, maxSize))
            {
                throw new Exception("Dung lượng phải nhỏ hơn 1MB");
            }

            var student = _studentRepository.FindByCode(studentCode);
            if (student == null)
            {
                throw new Exception("Sinh viên không tồn tại");
            }
            string dir = "Resources/Students";
            string mappedDir = _pathProvider.MapPath(dir);
            Directory.CreateDirectory(mappedDir);
            string newFileName = $"ava_{studentCode}{Path.GetExtension(formFile.FileName).ToLower()}";
            string filePath = _pathProvider.MapPath($"{dir}/{newFileName}");

            try
            {
                await FileHelper.SaveFileAsync(formFile, filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{studentCode} || {ex.Message} || {ex.ToString()} ");
                throw ex;
            }

            student.File1 = $"Resources/Students/{newFileName}";
            _studentRepository.Update(student);
            await _unitOfWork.SaveAsync();
            return student.File1;
        }

        public Image Resize(Image image, int newWidth, int maxHeight, bool onlyResizeIfWider)
        {
            if (onlyResizeIfWider && image.Width <= newWidth) newWidth = image.Width;

            var newHeight = image.Height * newWidth / image.Width;
            if (newHeight > maxHeight)
            {
                // Resize with height instead  
                newWidth = image.Width * maxHeight / image.Height;
                newHeight = maxHeight;
            }

            var res = new Bitmap(newWidth, newHeight);

            using (var graphic = Graphics.FromImage(res))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return res;
        }

        public Image cropImage(Image src, int width, int height)
        {
            Rectangle cropRect = new Rectangle(0, 0, width, height);
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage((Bitmap)src, new Rectangle(0, 0, target.Width, target.Height),
                                 cropRect,
                                 GraphicsUnit.Pixel);
                return target;
            }
        }
    }
}
