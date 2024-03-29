﻿using nuce.web.api.Models.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static nuce.web.api.Common.Ctsv;

namespace nuce.web.api.Services.Ctsv.Interfaces
{
    public interface IThamSoDichVuService
    {
        public IQueryable<AsAcademyStudentSvThietLapThamSoDichVu> GetParameters(DichVu dichu);
        public IQueryable<AsAcademyStudentSvThietLapThamSoDichVu> GetParameters(IEnumerable<long> idList);
        public IQueryable<AsAcademyStudentSvLoaiDichVu> GetLoaiDichVu();
    }
}
