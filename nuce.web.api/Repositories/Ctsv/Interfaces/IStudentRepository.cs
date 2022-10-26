using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.ViewModel.CDSConnect;
using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Repositories.Ctsv.Interfaces
{
    public interface IStudentRepository
    {
        public AsAcademyStudent FindByCode(string studentCode);
        public void Update(AsAcademyStudent student);
        public AsAcademyStudent FindByEmailNhaTruong(string email);
        public DbSet<AsAcademyStudent> GetAll();
        public Task<StudentDichVuModel> GetStudentDichVuInfoAsync(string studentCode);
        Task<ViewGetSvDto> GetSinhVienByCodeCDS(string masv);
    }
}
