﻿using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
using nuce.web.api.ViewModel.Ctsv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static nuce.web.api.Common.Ctsv;

namespace nuce.web.api.Repositories.Ctsv.Implements
{
    public class BaseStudentServiceRepository<Entity> : IBaseStudentServiceRepository<Entity> where Entity : class
    {
        private readonly CTSVNUCE_DATAContext _context;
        public BaseStudentServiceRepository(CTSVNUCE_DATAContext _context)
        {
            this._context = _context;
        }

        public async Task AddAsync(Entity model)
        {
            await _context.Set<Entity>().AddAsync(model);
        }

        public IQueryable<Entity> GetAll(long studentId)
        {
            return _context.Set<Entity>().AsNoTracking().ToList()
                    .Where(item => Convert.ToInt32(item.GetType().GetProperty("StudentId").GetValue(item, null)) == studentId &&
                            !Convert.ToBoolean(item.GetType().GetProperty("Deleted").GetValue(item, null)))
                    .OrderByDescending(item => item.GetType().GetProperty("LastModifiedTime").GetValue(item, null))
                    .AsQueryable();
        }

        public async Task<IQueryable<Entity>> GetAllForAdmin(QuanLyDichVuDetailModel model)
        {
            DateTime dtCompare = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            dtCompare = dtCompare.AddDays(-1 * model.DayRange);
            model.SearchText = model.SearchText?.Trim()?.ToLower();

            var tmp = (await _context.Set<Entity>().AsNoTracking().ToListAsync())
                        .Where(item => (int)getValue(item, "Status") > 1 && !Convert.ToBoolean(getValue(item, "Deleted")) &&
                                        (
                                            string.IsNullOrEmpty(model.SearchText) || 
                                            getValueString(item, "Id").Contains(model.SearchText) || 
                                            getValueString(item, "StudentCode").Contains(model.SearchText) || 
                                            getValueString(item, "StudentName").Contains(model.SearchText) || 
                                            getValueString(item, "PhanHoi").Contains(model.SearchText)
                                        ) &&
                                        DateTime.Parse(getValueString(item, "LastModifiedTime")) >= dtCompare)
                        .OrderBy(r => getValue(r, "Status"))
                        .ThenByDescending(r => getValue(r, "LastModifiedTime"));
            return tmp.AsQueryable();
        }

        public bool IsDuplicated(long studentId, string reason = null)
        {
            return _context.Set<Entity>().AsNoTracking().ToList()
                    .Any(item => Convert.ToInt32(item.GetType().GetProperty("StudentId").GetValue(item, null)) == studentId &&
                            Convert.ToInt32(item.GetType().GetProperty("Status").GetValue(item, null)) < (int)TrangThaiYeuCau.DaXuLyVaCoLichHen &&
                           (string.IsNullOrEmpty(reason) || item.GetType().GetProperty("LyDo").GetValue(item, null)?.ToString() == reason));
        }

        public AllTypeDichVuModel GetRequestInfo()
        {
            var allRequest = _context.Set<Entity>().AsNoTracking().ToList().Where(r => getValue(r, "Status") != null);
            var result = new AllTypeDichVuModel
            {
                TongSo = 0,
                MoiGui = 0,
                DangXuLy = 0,
                DaXuLy = 0,
            };
            foreach (var request in allRequest)
            {
                result.TongSo++;
                switch (getValue(request, "Status"))
                {
                    case (int)TrangThaiYeuCau.DaGuiLenNhaTruong:
                        result.MoiGui++;
                        break;
                    case (int)TrangThaiYeuCau.DangXuLy:
                        result.DangXuLy++;
                        break;
                    case (int)Common.Ctsv.TrangThaiYeuCau.HoanThanh:
                    case (int)Common.Ctsv.TrangThaiYeuCau.TuChoi:
                    case (int)Common.Ctsv.TrangThaiYeuCau.DaXuLyVaCoLichHen:
                        result.DaXuLy++;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        private object getValue(Entity entity, string field)
        {
            return entity.GetType().GetProperty(field).GetValue(entity, null);
        }

        private string getValueString(Entity entity, string field)
        {
            var value = entity.GetType().GetProperty(field).GetValue(entity, null);
            return value != null ? value.ToString().ToLower() : "";
        }

        public async Task<Entity> FindByIdAsync(long id)
        {
            return await _context.Set<Entity>().FindAsync(id);
        }
    }
}
