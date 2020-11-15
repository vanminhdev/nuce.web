﻿using EduWebService;
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

        /// <summary>
        /// Nếu có kì học là IsCurrent và Enabled thì trả về Id kì học đó nếu không thì trả về -1
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetCurrentSemesterId()
        {
            var semesterId = -1;
            var semester = await _eduDataContext.AsAcademySemester.FirstOrDefaultAsync(s => s.IsCurrent == true && s.Enabled == true);
            if (semester != null)
            {
                semesterId = semester.Id;
            }
            return semesterId;
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
                        SemesterId = await GetCurrentSemesterId(),
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

        public async Task SyncLastLecturerClassRoom()
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
                                SemesterId = await GetCurrentSemesterId(),
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

        public async Task SyncCurrentLecturerClassRoom()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            try
            {
                var result = await srvc.getAllTKB1JoinToDkAsync();
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
                var list_ThoiKhoaBieu_Join_DangKy_V2 = new List<ThoiKhoaBieuJoinToDangKy>();
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
                    list_ThoiKhoaBieu_Join_DangKy_V2.Add(tkbJoinDK);
                }
                #endregion

                #region xử lý chèn vào csdl
                long lopId = -1;
                long canBoId = -1;
                list_ThoiKhoaBieu_Join_DangKy_V2 = list_ThoiKhoaBieu_Join_DangKy_V2.Distinct((x, y) => {
                    if (x.MaDK == y.MaDK && x.MaCB == y.MaCB)
                        return true;
                    return false;
                }).ToList();
                foreach (var item in list_ThoiKhoaBieu_Join_DangKy_V2)
                {
                    var strMaDK = item.MaDK.Replace(" ", "");
                    var strMaCB = item.MaCB;
                    if (!string.IsNullOrWhiteSpace(strMaDK) && !string.IsNullOrWhiteSpace(strMaCB))
                    {
                        var lop = await _eduDataContext.AsAcademyCClassRoom.FirstOrDefaultAsync(c => c.Code == strMaDK);
                        var canBo = await _eduDataContext.AsAcademyLecturer.FirstOrDefaultAsync(c => c.Code == strMaCB);

                        if (lop != null && canBo != null)
                        {
                            lopId = lop.Id;
                            canBoId = canBo.Id;

                            //xoá lớp học phần có mã đăng ký cụ thể vd 010211LOP21
                            var listLecturerClassRoom = await _eduDataContext.AsAcademyCLecturerClassRoom.Where(lc => lc.ClassRoomCode == strMaDK).ToListAsync();
                            _eduDataContext.AsAcademyCLecturerClassRoom.RemoveRange(listLecturerClassRoom);

                            //thêm mới -> sinh Id mới
                            _eduDataContext.AsAcademyCLecturerClassRoom.Add(new AsAcademyCLecturerClassRoom
                            {
                                SemesterId = await GetCurrentSemesterId(),
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
                        SemesterId = await GetCurrentSemesterId(),
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
                        SemesterId = await GetCurrentSemesterId(),
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
                        SemesterId = await GetCurrentSemesterId(),
                        Code = MaMH,
                        Name = TenMH,
                        DepartmentId = boMonId,
                        DepartmentCode = MaBM
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

        public async Task SyncLastClassRoom()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            try
            {
                //nếu không truncate table class room nên khi đồng bộ dữ liệu mới dữ liệu cũ nếu không có code trùng thì k bị ảnh hưởng
                //_eduDataContext.Database.ExecuteSqlRaw("TRUNCATE TABLE AS_Academy_ClassRoom");
                var result = await srvc.getAllToDKKyTruocAsync();
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
                            SemesterId = await GetCurrentSemesterId(),
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

        public async Task SyncCurrentClassRoom()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            try
            {
                //_eduDataContext.Database.ExecuteSqlRaw("TRUNCATE TABLE AS_Academy_C_ClassRoom");
                var result = await srvc.getAllToDKKyHienTaiAsync();
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

                    var lopHocPhan = await _eduDataContext.AsAcademyCClassRoom.FirstOrDefaultAsync(f => f.Code == MaDK);
                    if (lopHocPhan == null) // thêm vào nếu chưa có
                    {
                        _eduDataContext.AsAcademyCClassRoom.Add(new AsAcademyCClassRoom
                        {
                            SemesterId = await GetCurrentSemesterId(),
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

        public async Task<string> SyncCurrentStudentClassRoom()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            var message = "";
            int page = 1;
            int pageSize = 500;
            try
            {
                //_eduDataContext.Database.ExecuteSqlRaw("TRUNCATE TABLE AS_Academy_Student_ClassRoom");
                //_eduDataContext.Database.ExecuteSqlRaw("TRUNCATE TABLE AS_Academy_C_Student_ClassRoom");
                XmlNodeList listData = null;
                XmlNodeList temp = null;
                while (true)
                {
                    var result = await srvc.getKQDKKyHienTaiAsync((page - 1) * pageSize, pageSize);
                    listData = result.Any1.GetElementsByTagName("dataKQDK");
                    if (listData.Count > 0)
                    {
                        message += string.Format("---{0}---{1}", page, listData.Count);
                        foreach (XmlElement item in listData)
                        {
                            temp = item.GetElementsByTagName("MaSV");
                            string strMaSV = temp.Count > 0 ? temp[0].InnerText.Trim() : null;
                            temp = item.GetElementsByTagName("MaDK");
                            string strMaDK = temp.Count > 0 ? temp[0].InnerText.Trim() : null;

                            var classRoom = await _eduDataContext.AsAcademyClassRoom.FirstOrDefaultAsync(c => c.Code == strMaDK);
                            var classRoomId = classRoom != null ? classRoom.Id : -1;

                            var student = await _eduDataContext.AsAcademyStudent.FirstOrDefaultAsync(c => c.Code == strMaSV);
                            var studentId = student != null ? student.Id : -1;

                            var studentClassRoom = await _eduDataContext.AsAcademyStudentClassRoom
                                .FirstOrDefaultAsync(sc => sc.StudentCode == strMaSV && sc.ClassRoomId == classRoomId);

                            // nuce.web.data.Nuce_DanhGiaGiangVien.InsertAcademy_C_Student_ClassRoom(1, strMaSV, strMaDK);
                            // nuce.web.data.Nuce_DanhGiaGiangVien.InsertAcademy_Student_ClassRoom(1, strMaSV, strMaDK);
                            if (studentClassRoom == null)
                            {
                                _eduDataContext.AsAcademyCStudentClassRoom.Add(new AsAcademyCStudentClassRoom
                                {
                                    ClassRoomId = classRoomId,
                                    StudentId = studentId,
                                    StudentCode = strMaSV,
                                    SemesterId = await GetCurrentSemesterId()
                                });
                            }
                        }
                        page++;
                    }
                    else
                        break;
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
                var errMessage = UtilsException.GetMainMessage(e);
                throw new CallEduWebServiceException(errMessage);
            }
            return message;
        }

        public async Task<string> SyncUpdateFromDateEndDateCurrentClassRoom()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            var message = "";
            try
            {
                var result = await srvc.getAllTKB1JoinToDkAsync();
                XmlNodeList listData = result.Any1.GetElementsByTagName("dataTKB");
                XmlNodeList nodeFoundByTagName = null;

                DateTime dtNgayBatDau = new DateTime(2019, 8, 5);
                foreach (XmlElement item in listData)
                {
                    nodeFoundByTagName = item.GetElementsByTagName("MaDK");
                    string MaDK = nodeFoundByTagName.Count > 0 ? nodeFoundByTagName[0].InnerText.Trim() : null; //mã đăng ký vd: 010211LOP21

                    nodeFoundByTagName = item.GetElementsByTagName("TuanHoc");
                    string TuanHoc = nodeFoundByTagName.Count > 0 ? nodeFoundByTagName[0].InnerText : "";

                    DateTime Ngaytg = DateTime.Now.AddYears(1);
                    DateTime NgayBD = DateTime.Now.AddYears(1);
                    DateTime NgayKT = DateTime.Now.AddYears(-1);
                    for (int j = 0; j < TuanHoc.Length; j++)
                    {
                        if (TuanHoc[j].ToString().Trim().Replace(" ", "") != "")
                        {
                            //Ngaytg = dtNgayBatDau.AddDays((j + 20) * 7);
                            Ngaytg = dtNgayBatDau.AddDays(j * 7);
                            if (Ngaytg < NgayBD)
                                NgayBD = Ngaytg;
                            NgayKT = Ngaytg;
                        }
                    }

                    DateTime CFromDate = new DateTime(2026, 12, 30);
                    DateTime CEndDate = new DateTime(2006, 12, 30);

                    var CurrClassRoom = await _eduDataContext.AsAcademyCClassRoom.FirstOrDefaultAsync(cc => cc.Code == MaDK);
                    if (CurrClassRoom != null)
                    {
                        CFromDate = CurrClassRoom.FromDate ?? NgayBD;
                        CEndDate = CurrClassRoom.EndDate ?? NgayKT;
                    }

                    if (CFromDate > NgayBD)
                        CFromDate = NgayBD;
                    if (CEndDate < NgayKT)
                        CEndDate = NgayKT;

                    CurrClassRoom.FromDate = CFromDate;
                    CurrClassRoom.EndDate = CEndDate;
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
                var errMessage = UtilsException.GetMainMessage(e);
                throw new CallEduWebServiceException(errMessage);
            }
            return message;
        }

        public async Task<string> SyncQAWeek()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            var message = "";
            try
            {
                _eduDataContext.Database.ExecuteSqlRaw("TRUNCATE TABLE AS_Edu_QA_Week");
                var listCurrClassRoom = await _eduDataContext.AsAcademyCClassRoom.Where(cc => cc.FromDate != null).ToListAsync();
                foreach(var currClassRoom in listCurrClassRoom)
                {
                    var iId = currClassRoom.Id;
                    DateTime FromDate = currClassRoom.FromDate.Value;
                    DateTime EndDate = currClassRoom.EndDate.Value;
                    int total = ((EndDate - FromDate).Days / 7) + 1; //sô tuần
                    for (int j = 1; j < total + 1; j++)
                    {

                        //nuce.web.data.Nuce_DanhGiaGiangVien.UpdateAS_Edu_QA_Week(iID, j, total, FromDate, FromDate.AddDays(7));
                        FromDate = FromDate.AddDays(7);
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
                var errMessage = UtilsException.GetMainMessage(e);
                throw new CallEduWebServiceException(errMessage);
            }
            return message;
        }

        public async Task<string> SyncLastStudentClassRoom()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            var message = "";
            int page = 1;
            int pageSize = 500;
            try
            {
                //_eduDataContext.Database.ExecuteSqlRaw("TRUNCATE TABLE AS_Academy_Student_ClassRoom");
                //_eduDataContext.Database.ExecuteSqlRaw("TRUNCATE TABLE AS_Academy_C_Student_ClassRoom");
                XmlNodeList listData = null;
                XmlNodeList temp = null;
                while (true)
                {
                    var result = await srvc.getKQDKKyTruocAsync((page - 1) * pageSize, pageSize);
                    listData = result.Any1.GetElementsByTagName("dataKQDK");
                    if (listData.Count > 0)
                    {
                        message += string.Format("---{0}---{1}", page, listData.Count);
                        foreach (XmlElement item in listData)
                        {
                            temp = item.GetElementsByTagName("MaSV");
                            string strMaSV = temp.Count > 0 ? temp[0].InnerText.Trim() : null;
                            temp = item.GetElementsByTagName("MaDK");
                            string strMaDK = temp.Count > 0 ? temp[0].InnerText.Trim() : null;

                            var classRoom = await _eduDataContext.AsAcademyClassRoom.FirstOrDefaultAsync(c => c.Code == strMaDK);
                            var classRoomId = classRoom != null ? classRoom.Id : -1;

                            var student = await _eduDataContext.AsAcademyStudent.FirstOrDefaultAsync(c => c.Code == strMaSV);
                            var studentId = student != null ? student.Id : -1;

                            var studentClassRoom = await _eduDataContext.AsAcademyStudentClassRoom
                                .FirstOrDefaultAsync(sc => sc.StudentCode == strMaSV && sc.ClassRoomId == classRoomId);

                            if (studentClassRoom == null)
                            {
                                _eduDataContext.AsAcademyStudentClassRoom.Add(new AsAcademyStudentClassRoom
                                {
                                    ClassRoomId = classRoomId,
                                    StudentId = studentId,
                                    StudentCode = strMaSV,
                                    SemesterId = await GetCurrentSemesterId()
                                });
                            }
                        }
                        page++;
                    }
                    else
                        break;
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
                var errMessage = UtilsException.GetMainMessage(e);
                throw new CallEduWebServiceException(errMessage);
            }
            return message;
        }
    }
}
