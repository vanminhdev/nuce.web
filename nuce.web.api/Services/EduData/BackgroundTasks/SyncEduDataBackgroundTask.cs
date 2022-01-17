using EduWebService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Status;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Background;
using nuce.web.api.Services.Status.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using static EduWebService.ServiceSoapClient;

namespace nuce.web.api.Services.EduData.BackgroundTasks
{
    public class SyncEduDataBackgroundTask
    {
        private readonly ILogger<SyncEduDataBackgroundTask> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly BackgroundTaskWorkder _backgroundTaskWorker;
        private readonly StatusService _statusService;

        public SyncEduDataBackgroundTask(ILogger<SyncEduDataBackgroundTask> logger,
            IServiceScopeFactory scopeFactory, BackgroundTaskWorkder backgroundTaskWorker,
            StatusService statusService)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _backgroundTaskWorker = backgroundTaskWorker;
            _statusService = statusService;
        }

        private async Task SyncLastStudentClassroomBG(CancellationToken token)
        {
            var scope = _scopeFactory.CreateScope();
            using var surveyContext = scope.ServiceProvider.GetRequiredService<SurveyContext>();
            using var statusContext = scope.ServiceProvider.GetRequiredService<StatusContext>();
            ServiceSoapClient srvc = new ServiceSoapClient(EndpointConfiguration.ServiceSoap12);
            var status = statusContext.AsStatusTableTask.FirstOrDefault(o => o.TableName == TableNameTask.AsAcademyStudentClassRoom);
            if (status == null)
            {
                throw new RecordNotFoundException("Không tìm thấy bản ghi cập nhật trạng thái cho bảng sinh viên lớp môn học");
            }

            status.Status = (int)TableTaskStatus.Doing;
            statusContext.SaveChanges();

            IDbContextTransaction transaction = null;
            var message = "";
            int page = 1;
            int pageSize = 1000;

            int totalDone = 0;
            try
            {
                XmlNodeList listData = null;
                XmlNodeList temp = null;

                _logger.LogInformation("sync last student classroom is start.");
                //eduDataContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE As_Academy_Student_ClassRoom");
                var semester = await surveyContext.AsAcademySemester.FirstOrDefaultAsync(o => o.Status == (int)SemesterStatus.IsLast);
                var countTrungLap = 0;
                while (true)
                {
                    transaction = surveyContext.Database.BeginTransaction();
                    var result = await srvc.getKQDKKyTruocAsync((page - 1) * pageSize, pageSize);
                    listData = result.Any1.GetElementsByTagName("dataKQDK");
                    if (listData.Count > 0)
                    {
                        message += string.Format("---{0}---{1}", page, listData.Count);
                        foreach (XmlElement item in listData)
                        {
                            temp = item.GetElementsByTagName("MaSV");
                            string strMaSV = temp.Count > 0 ? temp[0].InnerText.Trim() : null;
                            temp = item.GetElementsByTagName("MaDK");
                            string strMaDK = temp.Count > 0 ? temp[0].InnerText.Trim().Replace(" ", "") : null;
                            temp = item.GetElementsByTagName("NHHK");
                            var NHHK = temp.Count > 0 ? temp[0].InnerText.Trim() : null;

                            var studentClassRoom = await surveyContext.AsAcademyStudentClassRoom
                                .FirstOrDefaultAsync(sc => sc.StudentCode == strMaSV && sc.ClassRoomCode == strMaDK && sc.Nhhk == NHHK);

                            if (studentClassRoom == null) //chưa có thì thêm
                            {
                                surveyContext.AsAcademyStudentClassRoom.Add(new AsAcademyStudentClassRoom
                                {
                                    ClassRoomCode = strMaDK,
                                    StudentCode = strMaSV,
                                    Nhhk = NHHK
                                });
                            } 
                            else
                            {
                                countTrungLap++;
                                //_logger.LogInformation($"dong bo sinh vien lop mon hoc bi trung ma sv: {strMaSV}, ma lop: {strMaDK}, nhhk: {NHHK}");
                            }
                        }
                        page++;
                        await surveyContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                    else
                        break;
                    totalDone += listData.Count;
                    _logger.LogInformation($"sync last student classroom {totalDone} record");
                }
                _logger.LogInformation("sync last student classroom is done");
                _logger.LogInformation($"co tat ca {countTrungLap} ban ghi bi trung lap");

                status.Status = (int)TableTaskStatus.Done;
                status.IsSuccess = true;
                status.Message = null;
                statusContext.SaveChanges();
            }
            catch (Exception e)
            {
                if (transaction != null)
                    await transaction.RollbackAsync();

                status.Status = (int)TableTaskStatus.Done;
                status.IsSuccess = false;
                status.Message = UtilsException.GetMainMessage(e);
                statusContext.SaveChanges();
                throw e;
            }
        }

        public async Task SyncLastStudentClassroom()
        {
            var status = await _statusService.GetStatusTableTask(TableNameTask.AsAcademyStudentClassRoom);
            if (status.Status == (int)TableTaskStatus.Doing)
            {
                throw new TableBusyException("Bảng đang làm việc, thao tác bị huỷ");
            }

            _backgroundTaskWorker.StartAction(SyncLastStudentClassroomBG);
        }
    }
}
