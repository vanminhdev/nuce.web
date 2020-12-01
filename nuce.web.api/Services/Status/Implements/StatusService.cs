using Microsoft.EntityFrameworkCore;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Status;
using nuce.web.api.Services.Status.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Status.Implements
{
    class StatusService : IStatusService
    {
        private readonly StatusContext _context;

        public StatusService(StatusContext context)
        {
            _context = context;
        }

        public async Task<AsStatusTableTask> GetStatusTableTask(string tableName)
        {
            var task = await _context.AsStatusTableTask.FirstOrDefaultAsync(o => o.TableName == tableName);
            if(task == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bản ghi trạng thái");
            }
            return task;
        }

        public async Task<int> GetCurrentSemesterId()
        {
            var semesterId = -1;
            var semester = await _context.AsAcademySemester.FirstOrDefaultAsync(s => s.IsCurrent == true && s.Enabled == true && s.Deleted == false);
            if (semester != null)
            {
                semesterId = semester.Id;
            }
            return semesterId;
        }

        public async Task<int> GetLastSemesterId()
        {
            var semesterId = -1;
            var semester = await _context.AsAcademySemester.FirstOrDefaultAsync(s => s.IsCurrent == false && s.Enabled == true && s.Deleted == false);
            if (semester != null)
            {
                semesterId = semester.Id;
            }
            return semesterId;
        }

        public async Task DoNotStatusTableTask(string tableName, string messageNotFound)
        {
            var status = await _context.AsStatusTableTask.FirstOrDefaultAsync(o => o.TableName == tableName);
            if (status == null)
            {
                throw new RecordNotFoundException(messageNotFound);
            }
            status.Status = (int)TableTaskStatus.DoNot;
            await _context.SaveChangesAsync();
        }

        public async Task DoingStatusTableTask(string tableName, string messageNotFound, string messageBusy) 
        {
            var status = await _context.AsStatusTableTask.FirstOrDefaultAsync(o => o.TableName == tableName);
            if (status == null)
            {
                throw new RecordNotFoundException(messageNotFound);
            }
            //bảng đang làm việc
            if (status.Status == (int)TableTaskStatus.Doing)
            {
                throw new TableBusyException(messageBusy);
            }
            status.Status = (int)TableTaskStatus.Doing;
            await _context.SaveChangesAsync();
        }

        public async Task DoneStatusTableTask(string tableName, string messageNotFound, string messageIfError = null)
        {
            var status = await _context.AsStatusTableTask.FirstOrDefaultAsync(o => o.TableName == tableName);
            if (status == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bản ghi cập nhật trạng thái cho bảng thống kê bài khảo sát");
            }
            status.Status = (int)TableTaskStatus.Done;
            if(messageIfError != null)
            {
                status.IsSuccess = false;
                status.Message = messageIfError;
            } 
            else
            {
                status.IsSuccess = true;
            }
            await _context.SaveChangesAsync();
        }
    }
}
