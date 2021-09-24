using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class XacNhanRepository : BaseStudentServiceRepository<AsAcademyStudentSvXacNhan>, IXacNhanRepository
    {
        public XacNhanRepository(CTSVNUCE_DATAContext _context) : base(_context)
        {

        }

        public GetAllForAdminResponseRepo<AsAcademyStudentSvXacNhan> GetAllForAdminCustom(QuanLyDichVuDetailModel model)
        {
            DateTime? dtCompare = null;
            if (model.DayRange != null)
            {
                dtCompare = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                dtCompare = dtCompare?.AddDays(-1 * model.DayRange ?? 0);
            }
            model.SearchText = model.SearchText?.Trim()?.ToLower();

            var beforeFilteredData = _context.AsAcademyStudentSvXacNhan
                                        .Where(item => (item.Status ?? 0) > 1 && !(item.Deleted ?? false));

            var finalData = beforeFilteredData
                        .Where(item => (
                                            (model.SearchText ?? "") == "" ||
                                            item.Id.ToString().Contains(model.SearchText ?? "") ||
                                            (item.StudentCode ?? "").Contains(model.SearchText ?? "") ||
                                            (item.StudentName ?? "").Contains(model.SearchText ?? "") ||
                                            (item.PhanHoi ?? "").Contains(model.SearchText ?? "")
                                        ) &&
                                        (dtCompare == null || item.LastModifiedTime >= dtCompare))
                        .OrderBy(r => r.Status)
                        .ThenByDescending(r => r.LastModifiedTime);
            return new GetAllForAdminResponseRepo<AsAcademyStudentSvXacNhan>
            {
                FinalData = finalData
            };
        }

    }
}
