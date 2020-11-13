using EduWebService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using nuce.web.api.HandleException;
using nuce.web.api.Models.EduData;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Synchronization.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static EduWebService.ServiceSoapClient;

namespace nuce.web.api.Services.Synchronization.Implements
{
    public class SyncEduDatabaseService : ISyncEduDatabaseService
    {
        private readonly EduDataContext _eduDataContext;
        private readonly ServiceSoapClient srvc = new ServiceSoapClient(EndpointConfiguration.ServiceSoap12);

        public SyncEduDatabaseService(EduDataContext eduDataContext)
        {
            _eduDataContext = eduDataContext;
        }

        public async Task SyncAcademics()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            try
            {
                _eduDataContext.Database.ExecuteSqlRaw("TRUNCATE TABLE AS_Academy_Academics");
                var result = await srvc.getNganhAsync();
                XmlNodeList listData = result.Any1.GetElementsByTagName("Table");
                XmlNodeList temp = null;
                foreach (XmlElement item in listData)
                {
                    temp = item.GetElementsByTagName("MaNganh");
                    var maNganh = temp.Count > 0 ? temp[0].InnerText : "";
                    temp = item.GetElementsByTagName("TenNg");
                    var tenNg = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("loainganh");
                    var loainganh = temp.Count > 0 ? temp[0].InnerText : null;

                    _eduDataContext.AsAcademyAcademics.Add(new AsAcademyAcademics
                    {
                        SemesterId = 1,
                        Code = maNganh,
                        Name = tenNg
                    });
                }
                await _eduDataContext.SaveChangesAsync();
                transaction.Commit();
            }
            catch (DbUpdateException e)
            {
                await transaction.RollbackAsync();
                throw e;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                var message = UtilsException.GetMainMessage(e);
                throw new CallEduWebServiceException(message);
            }
        }

        public async Task SyncClass()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            try
            {
                _eduDataContext.Database.ExecuteSqlRaw("TRUNCATE TABLE AS_Academy_Class");
                var result = await srvc.getLopAsync();
                XmlNodeList listData = result.Any1.GetElementsByTagName("Table");
                XmlNodeList temp = null;
                foreach (XmlElement item in listData)
                {
                    temp = item.GetElementsByTagName("MaLop");
                    var MaLop = temp.Count > 0 ? temp[0].InnerText : "";
                    temp = item.GetElementsByTagName("TenLop");
                    var TenLop = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("MaKH");
                    var MaKH = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("MaNg");
                    var MaNg = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("NienKhoa");
                    var NienKhoa = temp.Count > 0 ? temp[0].InnerText : null;

                    var khoaId = -1;
                    var khoa = await _eduDataContext.AsAcademyFaculty.FirstOrDefaultAsync(f => f.Code == MaKH);
                    if(khoa != null)
                    {
                        khoaId = khoa.Id;
                    }

                    var nganhId = -1;
                    var nganh = await _eduDataContext.AsAcademyAcademics.FirstOrDefaultAsync(a => a.Code == MaNg);
                    if (nganh != null)
                    {
                        nganhId = nganh.Id;
                    }

                    _eduDataContext.AsAcademyClass.Add(new AsAcademyClass
                    {
                        Code = MaLop,
                        Name = TenLop,
                        FacultyId = khoaId,
                        FacultyCode = MaKH,
                        AcademicsId = nganhId,
                        AcademicsCode = MaNg,
                        SchoolYear = NienKhoa
                    });
                }
                await _eduDataContext.SaveChangesAsync();
                transaction.Commit();
            }
            catch (DbUpdateException e)
            {
                await transaction.RollbackAsync();
                throw e;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                var message = UtilsException.GetMainMessage(e);
                throw new CallEduWebServiceException(message);
            }
        }


        private class ThoiKhoaBieuJoinToDangKy
        {
            public string MaDK { get; set; }
            public string MaCB { get; set; }
            public string Thu { get; set; }
            public string TietDB { get; set; }
            public string SoTiet { get; set; }
            public string MaPH { get; set; }
            public string TuanHoc { get; set; }
            public string MaMH { get; set; }
        }

        #region tìm cán bộ có mã tương ứng trong lớp ghép 
        private string getCanBoTrongLopGhepTuongUng(List<ThoiKhoaBieuJoinToDangKy> list, 
            string MaMH, string Thu, string TietDB, string MaPH, string TuanHoc)
        {
            foreach (var item in list)
            {
                if (item.MaMH.Equals(MaMH)
                  && item.Thu.Equals(Thu)
                  && item.TietDB.Equals(TietDB)
                  && item.MaPH.Equals(MaPH)
                  && item.TuanHoc.Equals(TuanHoc)
                  && !string.IsNullOrWhiteSpace(item.MaCB.ToString()))
                {
                    return item.MaCB.Trim();
                }
            }
            return "";
        }
        #endregion

        public async Task SyncLecturerClass()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            try
            {
                var result = await srvc.getAllTKB1JoinToDk1Async();
                XmlNodeList listNode = result.Any1.GetElementsByTagName("dataTKB");
                XmlNodeList nodeFoundByTagName = null;

                #region Xử lý chuẩn hoá dữ liệu
                var list_ThoiKhoaBieu_Join_DangKy1_V1 = new List<ThoiKhoaBieuJoinToDangKy>();
                foreach (XmlElement item in listNode)
                {
                    var tkbJoinDangKy = new ThoiKhoaBieuJoinToDangKy();
                    nodeFoundByTagName = item.GetElementsByTagName("MaDK");
                    tkbJoinDangKy.MaDK = nodeFoundByTagName.Count > 0 ? nodeFoundByTagName[0].InnerText.Trim() : null;
                    
                    nodeFoundByTagName = item.GetElementsByTagName("MaCB");
                    tkbJoinDangKy.MaCB = nodeFoundByTagName.Count > 0 ? nodeFoundByTagName[0].InnerText.Trim() : null;

                    nodeFoundByTagName = item.GetElementsByTagName("Thu");
                    tkbJoinDangKy.Thu = nodeFoundByTagName.Count > 0 ? nodeFoundByTagName[0].InnerText.Trim() : null;

                    nodeFoundByTagName = item.GetElementsByTagName("TietDB");
                    tkbJoinDangKy.TietDB = nodeFoundByTagName.Count > 0 ? nodeFoundByTagName[0].InnerText.Trim() : null;

                    nodeFoundByTagName = item.GetElementsByTagName("SoTiet");
                    tkbJoinDangKy.SoTiet = nodeFoundByTagName.Count > 0 ? nodeFoundByTagName[0].InnerText.Trim() : null;

                    nodeFoundByTagName = item.GetElementsByTagName("MaMH");
                    tkbJoinDangKy.MaMH = nodeFoundByTagName.Count > 0 ? nodeFoundByTagName[0].InnerText.Trim() : null;

                    nodeFoundByTagName = item.GetElementsByTagName("TuanHoc");
                    tkbJoinDangKy.TuanHoc = nodeFoundByTagName.Count > 0 ? nodeFoundByTagName[0].InnerText.Trim() : null;

                    nodeFoundByTagName = item.GetElementsByTagName("MaPH");
                    string MaPH = nodeFoundByTagName.Count > 0 ? nodeFoundByTagName[0].InnerText.Trim() : null;
                    if (!string.IsNullOrWhiteSpace(MaPH))
                    {
                        if (!MaPH.Equals("XMH"))
                        {
                            string[] roomArr = new string[2];

                            if (MaPH.Contains(" "))
                                roomArr = MaPH.Split(' ').ToArray();
                            else if (MaPH.Contains("."))
                                roomArr = MaPH.Split('.').ToArray();
                            else if (MaPH.Contains("-"))
                                roomArr = MaPH.Split('-').ToArray();
                            else if (MaPH.Contains("_"))
                                roomArr = MaPH.Split('_').ToArray();
                            else
                            {
                                roomArr[0] = MaPH;
                                roomArr[1] = "";
                            }
                            //throw new ArgumentException("TKB chứa ký tự không hợp lệ : " + JsonConvert.SerializeObject(sched));

                            if (!roomArr[0].All(Char.IsDigit)) // VD H1.101
                            {
                                MaPH = string.Concat(roomArr[1], ".", roomArr[0]); // => 101.H1
                            }
                        }
                    }
                    tkbJoinDangKy.MaPH = MaPH;
                    list_ThoiKhoaBieu_Join_DangKy1_V1.Add(tkbJoinDangKy);
                }
                #endregion

                #region Xử lý chuẩn hoá dữ liệu điền thông tin thêm vào giảng viên
                var list_ThoiKhoaBieu_Join_DangKy1_V2 = new List<ThoiKhoaBieuJoinToDangKy>();
                foreach (var item in list_ThoiKhoaBieu_Join_DangKy1_V1)
                {
                    var tkbJoinDK = new ThoiKhoaBieuJoinToDangKy();
                    tkbJoinDK.MaDK = item.MaDK;
                    tkbJoinDK.Thu = item.Thu;
                    tkbJoinDK.TietDB = item.TietDB;
                    tkbJoinDK.SoTiet = item.SoTiet;
                    tkbJoinDK.MaPH = item.MaPH;
                    tkbJoinDK.TuanHoc = item.TuanHoc;
                    tkbJoinDK.MaMH = item.MaMH;
                    string strMaCB = item.MaCB;
                    if (string.IsNullOrWhiteSpace(strMaCB))
                    {
                        //Xử lý tìm cán bộ tương ứng và có mã cán bộ khác trắng
                        tkbJoinDK.MaCB = getCanBoTrongLopGhepTuongUng(list_ThoiKhoaBieu_Join_DangKy1_V1, 
                            item.MaMH, item.Thu, item.TietDB, item.MaPH, item.TuanHoc);
                    }
                    else
                        tkbJoinDK.MaCB = strMaCB;
                    list_ThoiKhoaBieu_Join_DangKy1_V2.Add(tkbJoinDK);
                }
                #endregion

                #region xử lý chèn vào csdl
                long lopId = -1;
                long canBoId = -1;
                list_ThoiKhoaBieu_Join_DangKy1_V2 = list_ThoiKhoaBieu_Join_DangKy1_V2.Distinct((x , y) => {
                    if (x.MaDK == y.MaDK && x.MaCB == y.MaCB)
                        return true;
                    return false;
                }).ToList();
                foreach (var item in list_ThoiKhoaBieu_Join_DangKy1_V2)
                {
                    var strMaDK = item.MaDK.Replace(" ", "");
                    var strMaCB = item.MaCB;
                    if (!string.IsNullOrWhiteSpace(strMaDK) && !string.IsNullOrWhiteSpace(strMaCB))
                    {
                        var lop = await _eduDataContext.AsAcademyClassRoom.FirstOrDefaultAsync(c => c.Code == strMaDK);
                        var canBo = await _eduDataContext.AsAcademyLecturer.FirstOrDefaultAsync(c => c.Code == strMaCB);

                        if(lop != null && canBo != null)
                        {
                            lopId = lop.Id;
                            canBoId = canBo.Id;

                            //xoá lớp học phần có mã đăng ký cụ thể vd 010211LOP21
                            var listLecturerClassRoom = await _eduDataContext.AsAcademyLecturerClassRoom.Where(lc => lc.ClassRoomCode == strMaDK).ToListAsync();
                            _eduDataContext.AsAcademyLecturerClassRoom.RemoveRange(listLecturerClassRoom);

                            //thêm mới -> sinh Id mới
                            _eduDataContext.AsAcademyLecturerClassRoom.Add(new AsAcademyLecturerClassRoom
                            {
                                SemesterId = 1,
                                ClassRoomId = lopId,
                                ClassRoomCode = strMaDK,
                                LecturerId = canBoId,
                                LecturerCode = strMaCB
                            });
                        }
                    }
                }
                #endregion

                await _eduDataContext.SaveChangesAsync();
                transaction.Commit();
            }
            catch (DbUpdateException e)
            {
                await transaction.RollbackAsync();
                throw e;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                var message = UtilsException.GetMainMessage(e);
                throw new CallEduWebServiceException(message);
            }
        }

        public async Task SyncDepartment()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            try
            {
                _eduDataContext.Database.ExecuteSqlRaw("TRUNCATE TABLE AS_Academy_Department");
                var result = await srvc.getBoMonAsync();
                XmlNodeList listData = result.Any1.GetElementsByTagName("dataBoMon");
                foreach (XmlElement item in listData)
                {
                    var maBM = item.ChildNodes[0].InnerText;
                    var maKH = item.ChildNodes[1].InnerText;
                    var tenBM = item.ChildNodes[2].InnerText;
                    var truongBM = item.ChildNodes[3].InnerText;

                    var khoa = await _eduDataContext.AsAcademyFaculty.FirstOrDefaultAsync(f => f.Code == maKH);
                    if(khoa == null)
                    {
                        throw new RecordNotFoundException("Không tìm thấy khoa có mã " + maKH);
                    }

                    _eduDataContext.AsAcademyDepartment.Add(new AsAcademyDepartment
                    {
                        SemesterId = 1,
                        Code = maBM,
                        Name = tenBM,
                        FacultyId = khoa.Id,
                        FacultyCode = maKH,
                        ChefsDepartmentCode = truongBM
                    });
                }
                await _eduDataContext.SaveChangesAsync();
                transaction.Commit();
            }
            catch (RecordNotFoundException e)
            {
                await transaction.RollbackAsync();
                throw e;
            }
            catch (DbUpdateException e)
            {
                await transaction.RollbackAsync();
                throw e;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                var message = UtilsException.GetMainMessage(e);
                throw new CallEduWebServiceException(message);
            }
        }

        public async Task SyncFaculty()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            try
            {
                _eduDataContext.Database.ExecuteSqlRaw("TRUNCATE TABLE AS_Academy_Faculty");
                var result = await srvc.getKhoaAsync();
                XmlNodeList listData = result.Any1.GetElementsByTagName("dataKhoa");
                foreach (XmlElement item in listData)
                {
                    var orderString = item.Attributes.GetNamedItem("msdata:rowOrder").InnerText;
                    int order = 1;
                    int.TryParse(orderString, out order);

                    var maKH = item.ChildNodes[0].InnerText;
                    var tenKhoa = item.ChildNodes[1].InnerText;

                    _eduDataContext.AsAcademyFaculty.Add(new AsAcademyFaculty
                    {
                        Code = maKH,
                        Name = tenKhoa,
                        SemesterId = 1,
                        COrder = order + 1
                    });
                }
                await _eduDataContext.SaveChangesAsync();
                transaction.Commit();
            }
            catch (DbUpdateException e)
            {
                await transaction.RollbackAsync();
                throw e;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                var message = UtilsException.GetMainMessage(e);
                throw new CallEduWebServiceException(message);
            }
        }

        public async Task SyncLecturer()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            try
            {
                _eduDataContext.Database.ExecuteSqlRaw("TRUNCATE TABLE AS_Academy_Lecturer");
                var result = await srvc.getAllCanBoAsync();
                XmlNodeList listData = result.Any1.GetElementsByTagName("dataCanBo");
                XmlNodeList temp = null;
                foreach (XmlElement item in listData)
                {
                    temp = item.GetElementsByTagName("MaCB");
                    var MaCB = temp.Count > 0 ? temp[0].InnerText : "";
                    temp = item.GetElementsByTagName("TenCB");
                    var TenCB = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("MaBM");
                    var MaBM = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("CanBoTG");
                    var CanBoTG = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("EmailCB1");
                    var EmailCB1 = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("EmailCB2");
                    var EmailCB2 = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("TelCB1");
                    var TelCB1 = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("TelCB2");
                    var TelCB2 = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("ngsinhcb");
                    var ngsinhcb = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("IsNhanVien");
                    var IsNhanVien = temp.Count > 0 ? temp[0].InnerText : null;

                    var boMonId = -1;
                    var boMon = await _eduDataContext.AsAcademyDepartment.FirstOrDefaultAsync(f => f.Code == MaBM);
                    if (boMon != null)
                    {
                        boMonId = boMon.Id;
                    }

                    _eduDataContext.AsAcademyLecturer.Add(new AsAcademyLecturer
                    {
                        Code = MaCB,
                        FullName = TenCB,
                        DepartmentId = boMonId,
                        DepartmentCode = MaBM,
                        DateOfBirth = ngsinhcb,
                        NameOrder = null,
                        Email = EmailCB1 ?? EmailCB2 ?? null
                    });
                }
                await _eduDataContext.SaveChangesAsync();
                transaction.Commit();
            }
            catch (DbUpdateException e)
            {
                await transaction.RollbackAsync();
                throw e;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                var message = UtilsException.GetMainMessage(e);
                throw new CallEduWebServiceException(message);
            }
        }

        public async Task SyncStudent()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            try
            {
                _eduDataContext.Database.ExecuteSqlRaw("TRUNCATE TABLE AS_Academy_Student");
                var result = await srvc.getSinhVienAsync();
                XmlNodeList listData = result.Any1.GetElementsByTagName("Table");
                XmlNodeList temp = null;
                foreach (XmlElement item in listData)
                {
                    temp = item.GetElementsByTagName("MaSV");
                    var MaSV = temp.Count > 0 ? temp[0].InnerText : "";
                    temp = item.GetElementsByTagName("HoTenSV");
                    var HoTenSV = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("Malop");
                    var Malop = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("NgaySinh");
                    var NgaySinh = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("EmailSV1");
                    var EmailSV1 = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("EmailSV2");
                    var EmailSV2 = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("TelSV1");
                    var TelSV1 = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("TelSV2");
                    var TelSV2 = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("noisinh");
                    var noisinh = temp.Count > 0 ? temp[0].InnerText : null;

                    long lopId = -1;
                    var lop = await _eduDataContext.AsAcademyClass.FirstOrDefaultAsync(f => f.Code == Malop);
                    if (lop != null)
                    {
                        lopId = lop.Id;
                    }

                    _eduDataContext.AsAcademyStudent.Add(new AsAcademyStudent
                    {
                        Code = MaSV,
                        FulName = HoTenSV,
                        ClassId = lopId,
                        ClassCode = Malop,
                        DateOfBirth = NgaySinh,
                        BirthPlace = noisinh,
                        Email = EmailSV1 ?? EmailSV2 ?? null,
                        Mobile = TelSV1 ?? TelSV2 ?? null,
                        KeyAuthorize = Guid.NewGuid(),
                        Status = 1
                    });
                }
                await _eduDataContext.SaveChangesAsync();
                transaction.Commit();
            }
            catch (DbUpdateException e)
            {
                await transaction.RollbackAsync();
                throw e;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                var message = UtilsException.GetMainMessage(e);
                throw new CallEduWebServiceException(message);
            }
        }

        public async Task SyncSubject()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            try
            {
                _eduDataContext.Database.ExecuteSqlRaw("TRUNCATE TABLE AS_Academy_Subject");
                var result = await srvc.getMonHocAsync();
                XmlNodeList listData = result.Any1.GetElementsByTagName("Table");
                XmlNodeList temp = null;
                foreach (XmlElement item in listData)
                {
                    temp = item.GetElementsByTagName("MaMH");
                    var MaMH = temp.Count > 0 ? temp[0].InnerText : "";
                    temp = item.GetElementsByTagName("TenMH");
                    var TenMH = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("MaBM");
                    var MaBM = temp.Count > 0 ? temp[0].InnerText : null;

                    var boMonId = -1;
                    var boMon = await _eduDataContext.AsAcademyDepartment.FirstOrDefaultAsync(d => d.Code == MaBM);
                    if (boMon != null)
                    {
                        boMonId = boMon.Id;
                    }

                    _eduDataContext.AsAcademySubject.Add(new AsAcademySubject
                    {
                        SemesterId = 1,
                        Code = MaMH,
                        Name = TenMH,
                        DepartmentId = boMonId,
                        DepartmentCode = MaBM
                    });
                }
                await _eduDataContext.SaveChangesAsync();
                transaction.Commit();
            }
            catch (RecordNotFoundException e)
            {
                await transaction.RollbackAsync();
                throw e;
            }
            catch (DbUpdateException e)
            {
                await transaction.RollbackAsync();
                throw e;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                var message = UtilsException.GetMainMessage(e);
                throw new CallEduWebServiceException(message);
            }
        }

        public async Task SyncClassRoom()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            try
            {
                //không truncate table class room nên khi đồng bộ dữ liệu mới dữ liệu cũ nếu không có code trùng thì k bị ảnh hưởng
                var result = await srvc.getAllToDK1Async();
                XmlNodeList listData = result.Any1.GetElementsByTagName("dataToDangKy");
                XmlNodeList nodeFoundByTagName = null;
                int monHocId = -1;
                foreach (XmlElement item in listData) //3602
                {
                    nodeFoundByTagName = item.GetElementsByTagName("MaMH");
                    string MaMH = nodeFoundByTagName.Count > 0 ? nodeFoundByTagName[0].InnerText.Trim() : null; //mã môn học vd: 010211 của môn nào đó
                    nodeFoundByTagName = item.GetElementsByTagName("MaDK"); 
                    string MaDK = nodeFoundByTagName.Count > 0 ? nodeFoundByTagName[0].InnerText.Trim() : null; //mã đăng ký vd: 010211LOP21
                    nodeFoundByTagName = item.GetElementsByTagName("MaNh");
                    string MaNh = nodeFoundByTagName.Count > 0 ? nodeFoundByTagName[0].InnerText.Trim() : null; //mã nhóm vd: LOP21
                    nodeFoundByTagName = item.GetElementsByTagName("Malop");
                    string Malop = nodeFoundByTagName.Count > 0 ? nodeFoundByTagName[0].InnerText.Trim() : null; //mã lớp vd: LOP21, 61XD1
                    nodeFoundByTagName = item.GetElementsByTagName("Malop");
                    string ExamAttemptDate = nodeFoundByTagName.Count > 0 ? nodeFoundByTagName[0].InnerText.Trim() : null;

                    var monHoc = await _eduDataContext.AsAcademySubject.FirstOrDefaultAsync(f => f.Code == MaMH);
                    monHocId = monHoc != null ? monHoc.Id : -1;

                    var lopHocPhan = await _eduDataContext.AsAcademyClassRoom.FirstOrDefaultAsync(f => f.Code == MaDK);
                    if(lopHocPhan == null) // thêm vào nếu chưa có
                    {
                        _eduDataContext.AsAcademyClassRoom.Add(new AsAcademyClassRoom
                        {
                            SemesterId = 1,
                            Code = MaDK,
                            GroupCode = MaNh,
                            ClassCode = Malop,
                            SubjectId = monHocId,
                            SubjectCode = MaMH,
                            ExamAttemptDate = ExamAttemptDate
                        });
                    } 
                    else // nếu có rồi thì cập nhật
                    {
                        lopHocPhan.SemesterId = 1;
                        lopHocPhan.GroupCode = MaNh;
                        lopHocPhan.ClassCode = Malop;
                        lopHocPhan.SubjectId = monHocId;
                        lopHocPhan.SubjectCode = MaMH;
                        lopHocPhan.ExamAttemptDate = ExamAttemptDate;
                    }
                }
                await _eduDataContext.SaveChangesAsync();
                transaction.Commit();
            }
            catch (DbUpdateException e)
            {
                await transaction.RollbackAsync();
                throw e;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                var message = UtilsException.GetMainMessage(e);
                throw new CallEduWebServiceException(message);
            }
        }
    }
}
