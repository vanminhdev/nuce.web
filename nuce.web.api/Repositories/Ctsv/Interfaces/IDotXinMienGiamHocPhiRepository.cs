﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Ctsv;

namespace nuce.web.api.Repositories.Ctsv.Interfaces
{
    public interface IDotXinMienGiamHocPhiRepository : IBaseStudentServiceRepository<AsAcademyStudentSvXinMienGiamHocPhiDot>
    {
        Task<AsAcademyStudentSvXinMienGiamHocPhiDot> GetDotActive();
        Task<PaginationModel<AsAcademyStudentSvXinMienGiamHocPhiDot>> GetAll(int skip = 0, int take = 20);
        Task Add(AddDotXinMienGiamHocPhi model);
        Task Update(int id, AddDotXinMienGiamHocPhi model);
        Task Delete(int id);
    }
}
