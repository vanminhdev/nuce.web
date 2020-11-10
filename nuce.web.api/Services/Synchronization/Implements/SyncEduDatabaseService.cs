using EduWebService;
using Microsoft.EntityFrameworkCore;
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
    }
}
