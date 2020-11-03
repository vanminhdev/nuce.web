﻿using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Repositories.Ctsv.Interfaces;
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

        public bool IsDuplicated(long studentId, string reason = null)
        {
            return _context.Set<Entity>().AsNoTracking().ToList()
                    .Any(item => Convert.ToInt32(item.GetType().GetProperty("StudentId").GetValue(item, null)) == studentId &&
                            Convert.ToInt32(item.GetType().GetProperty("Status").GetValue(item, null)) < (int)TrangThaiYeuCau.DaXuLyVaCoLichHen &&
                           (string.IsNullOrEmpty(reason) || item.GetType().GetProperty("LyDo").GetValue(item, null)?.ToString() == reason));
        }
    }
}
