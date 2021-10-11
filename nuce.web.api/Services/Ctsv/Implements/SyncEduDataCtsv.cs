using EduWebService;
using Microsoft.EntityFrameworkCore;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Services.Ctsv.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using static EduWebService.ServiceSoapClient;

namespace nuce.web.api.Services.Ctsv.Implements
{
    public class SyncEduDataCtsv : ISyncEduDataCtsv
    {
        private readonly CTSVNUCE_DATAContext _eduDataContext;
        private readonly ServiceSoapClient srvc = new ServiceSoapClient(EndpointConfiguration.ServiceSoap12);

        public SyncEduDataCtsv(CTSVNUCE_DATAContext eduDataContext)
        {
            _eduDataContext = eduDataContext;
        }
        public async Task SyncAcademics()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            try
            {
                var result = await srvc.getNganhAsync();
                XmlNodeList listData = result.Any1.GetElementsByTagName("Table");
                XmlNodeList temp = null;
                foreach (XmlElement item in listData)
                {
                    temp = item.GetElementsByTagName("MaNg");
                    var maNganh = temp.Count > 0 ? temp[0].InnerText : "";
                    temp = item.GetElementsByTagName("TenNg");
                    var tenNg = temp.Count > 0 ? temp[0].InnerText : null;
                    temp = item.GetElementsByTagName("loainganh");
                    var loainganh = temp.Count > 0 ? temp[0].InnerText : null;

                    var nganh = await _eduDataContext.AsAcademyAcademics.FirstOrDefaultAsync(a => a.Code == maNganh);
                    if (nganh == null)
                    {
                        _eduDataContext.AsAcademyAcademics.Add(new AsAcademyAcademics
                        {
                            SemesterId = await GetCurrentSemesterId(),
                            Code = maNganh,
                            Name = tenNg
                        });
                    }
                    else
                    {
                        nganh.SemesterId = await GetCurrentSemesterId();
                        nganh.Code = maNganh;
                        nganh.Name = tenNg;
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

        public async Task SyncClass()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            try
            {
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
                    if (khoa != null)
                    {
                        khoaId = khoa.Id;
                    }

                    var nganhId = -1;
                    var nganh = await _eduDataContext.AsAcademyAcademics.FirstOrDefaultAsync(a => a.Code == MaNg);
                    if (nganh != null)
                    {
                        nganhId = nganh.Id;
                    }

                    var lop = await _eduDataContext.AsAcademyClass.FirstOrDefaultAsync(a => a.Code == MaLop);
                    if (lop == null)
                    {
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
                    else
                    {
                        lop.Code = MaLop;
                        lop.Name = TenLop;
                        lop.FacultyId = khoaId;
                        lop.FacultyCode = MaKH;
                        lop.AcademicsId = nganhId;
                        lop.AcademicsCode = MaNg;
                        lop.SchoolYear = NienKhoa;
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

        public async Task SyncFaculty()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            try
            {
                var result = await srvc.getKhoaAsync();
                XmlNodeList listData = result.Any1.GetElementsByTagName("dataKhoa");
                foreach (XmlElement item in listData)
                {
                    var orderString = item.Attributes.GetNamedItem("msdata:rowOrder").InnerText;
                    int order = 1;
                    int.TryParse(orderString, out order);

                    var maKH = item.ChildNodes[0].InnerText;
                    var tenKhoa = item.ChildNodes[1].InnerText;

                    var khoa = await _eduDataContext.AsAcademyFaculty.FirstOrDefaultAsync(a => a.Code == maKH);
                    if (khoa == null)
                    {
                        _eduDataContext.AsAcademyFaculty.Add(new AsAcademyFaculty
                        {
                            Code = maKH,
                            Name = tenKhoa,
                            SemesterId = await GetCurrentSemesterId(),
                            COrder = order + 1
                        });
                    }
                    else
                    {
                        khoa.Code = maKH;
                        khoa.Name = tenKhoa;
                        khoa.SemesterId = await GetCurrentSemesterId();
                        khoa.COrder = order + 1;
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

        public async Task SyncStudent()
        {
            var transaction = _eduDataContext.Database.BeginTransaction();
            try
            {
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

                    string emailEdu = null;
                    if ((EmailSV1 ?? "").Contains(".edu."))
                    {
                        emailEdu = EmailSV1;
                    } else if ((EmailSV2 ?? "").Contains(".edu."))
                    {
                        emailEdu = EmailSV2;
                    }

                    long lopId = -1;
                    var lop = await _eduDataContext.AsAcademyClass.FirstOrDefaultAsync(f => f.Code == Malop);
                    if (lop != null)
                    {
                        lopId = lop.Id;
                    }

                    var sinhVien = await _eduDataContext.AsAcademyStudent.FirstOrDefaultAsync(a => a.Code == MaSV);
                    if (sinhVien == null)
                    {
                        _eduDataContext.AsAcademyStudent.Add(new AsAcademyStudent
                        {
                            Code = MaSV,
                            FulName = HoTenSV,
                            ClassId = lopId,
                            ClassCode = Malop,
                            DateOfBirth = NgaySinh,
                            BirthPlace = noisinh,
                            Email = EmailSV1 ?? EmailSV2 ?? null,
                            EmailNhaTruong = emailEdu,
                            Mobile = TelSV1 ?? TelSV2 ?? null,
                            KeyAuthorize = Guid.NewGuid(),
                            Status = 1,
                            DaXacThucEmailNhaTruong = true,
                        });
                    }
                    else
                    {
                        sinhVien.Code = MaSV;
                        sinhVien.FulName = HoTenSV;
                        sinhVien.ClassId = lopId;
                        sinhVien.ClassCode = Malop;
                        sinhVien.DateOfBirth = NgaySinh;
                        sinhVien.BirthPlace = noisinh;
                        sinhVien.Email = EmailSV1 ?? EmailSV2 ?? null;
                        sinhVien.EmailNhaTruong = !string.IsNullOrEmpty(emailEdu) ? emailEdu : sinhVien.EmailNhaTruong;
                        sinhVien.Mobile = TelSV1 ?? TelSV2 ?? null;
                        sinhVien.Status = 1;
                        sinhVien.DaXacThucEmailNhaTruong = true;
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

        private async Task<int> GetCurrentSemesterId()
        {
            var semesterId = -1;
            var semester = await _eduDataContext.AsAcademySemester.FirstOrDefaultAsync(s => s.IsCurrent ?? false);
            if (semester != null)
            {
                semesterId = semester.Id;
            }
            return semesterId;
        }
    }
}
