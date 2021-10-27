using Microsoft.EntityFrameworkCore;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Status.Implements
{
    public class StatusService
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
                throw new RecordNotFoundException("Không tìm thấy bản ghi trạng thái của bảng");
            }

            var result = new AsStatusTableTask
            {
                Id = task.Id,
                Status = task.Status,
                IsSuccess = task.IsSuccess,
                TableName = task.TableName,
                Message = task.Message
            };

            //nếu là hoàn thành thì chuyển về chưa làm, chỉ hiển thị 1 lần message hoàn thành
            if (task.Status == (int)TableTaskStatus.Done)
            {
                task.Status = (int)TableTaskStatus.DoNot;
                task.IsSuccess = true;
                task.Message = null;
                await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<AsStatusTableTask> GetStatusTableTaskNotResetMessage(string tableName)
        {
            var task = await _context.AsStatusTableTask.FirstOrDefaultAsync(o => o.TableName == tableName);
            if (task == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bản ghi trạng thái của bảng");
            }

            var result = new AsStatusTableTask
            {
                Id = task.Id,
                Status = task.Status,
                IsSuccess = task.IsSuccess,
                TableName = task.TableName,
                Message = task.Message
            };

            //nếu là hoàn thành thì chuyển về chưa làm, chỉ hiển thị 1 lần message hoàn thành
            if (task.Status == (int)TableTaskStatus.Done)
            {
                task.Status = (int)TableTaskStatus.DoNot;
                task.IsSuccess = true;
                await _context.SaveChangesAsync();
            }
            return result;
        }
    }
}
