using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Common;
using nuce.web.api.HandleException;
using nuce.web.api.Models.EduData;
using nuce.web.api.Services.EduData.BackgroundTasks;
using nuce.web.api.Services.EduData.Interfaces;
using nuce.web.api.Services.Status.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.EduData;

namespace nuce.web.api.Controllers.EduData
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AppAuthorize(RoleList.Admin)]
    public class SyncEduDataController : ControllerBase
    {
        private readonly ILogger<SyncEduDataController> _logger;
        private readonly ISyncEduDatabaseService _syncEduDatabaseService;
        private readonly SyncEduDataBackgroundTask _syncEduDataBackgroundTask;
        private readonly IStatusService _statusService;

        public SyncEduDataController(ILogger<SyncEduDataController> logger, ISyncEduDatabaseService syncEduDatabaseService, SyncEduDataBackgroundTask syncEduDataBackgroundTask, IStatusService statusService)
        {
            _logger = logger;
            _syncEduDatabaseService = syncEduDatabaseService;
            _syncEduDataBackgroundTask = syncEduDataBackgroundTask;
            _statusService = statusService;
        }

        #region đồng bộ cơ bản
        [HttpPut]
        public async Task<IActionResult> SyncFaculty()
        {
            try
            {
                await _syncEduDatabaseService.SyncFaculty();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ Khoa" });
            }
            catch (CallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> SyncDepartment()
        {
            try
            {
                await _syncEduDatabaseService.SyncDepartment();
                return Ok();
            }
            catch (RecordNotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ bộ môn" });
            }
            catch (CallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> SyncAcademics()
        {
            try
            {
                await _syncEduDatabaseService.SyncAcademics();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ ngành học" });
            }
            catch (CallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> SyncSubject()
        {
            try
            {
                await _syncEduDatabaseService.SyncSubject();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ môn học" });
            }
            catch (CallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> SyncClass()
        {
            try
            {
                await _syncEduDatabaseService.SyncClass();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ lớp" });
            }
            catch (CallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> SyncLecturer()
        {
            try
            {
                await _syncEduDatabaseService.SyncLecturer();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ giảng viên" });
            }
            catch (CallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> SyncStudent()
        {
            try
            {
                await _syncEduDatabaseService.SyncStudent();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ sinh viên" });
            }
            catch (CallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }
        #endregion

        #region xem data cơ bản
        [HttpGet]
        public async Task<IActionResult> GetAllFaculties()
        {
            var result = await _syncEduDatabaseService.GetAllFaculties();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var result = await _syncEduDatabaseService.GetAllDepartments();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetAcademics([FromBody] DataTableRequest request)
        {
            var filter = new AcademicsFilter();
            if (request.Columns != null)
            {
                filter.Code = request.Columns.FirstOrDefault(c => c.Data == "code")?.Search.Value ?? null;
                filter.Name = request.Columns.FirstOrDefault(c => c.Data == "name")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _syncEduDatabaseService.GetAcademics(filter, skip, take);
            return Ok(
                new DataTableResponse<AsAcademyAcademics>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> GetSubject([FromBody] DataTableRequest request)
        {
            var filter = new SubjectFilter();
            if (request.Columns != null)
            {
                filter.Code = request.Columns.FirstOrDefault(c => c.Data == "code")?.Search.Value ?? null;
                filter.Name = request.Columns.FirstOrDefault(c => c.Data == "name")?.Search.Value ?? null;
                filter.DepartmentCode = request.Columns.FirstOrDefault(c => c.Data == "departmentCode")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _syncEduDatabaseService.GetSubject(filter, skip, take);
            return Ok(
                new DataTableResponse<AsAcademySubject>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> GetClass([FromBody] DataTableRequest request)
        {
            var filter = new ClassFilter();
            if (request.Columns != null)
            {
                filter.Code = request.Columns.FirstOrDefault(c => c.Data == "code")?.Search.Value ?? null;
                filter.Name = request.Columns.FirstOrDefault(c => c.Data == "name")?.Search.Value ?? null;
                filter.FacultyCode = request.Columns.FirstOrDefault(c => c.Data == "facultyCode")?.Search.Value ?? null;
                filter.AcademicsCode = request.Columns.FirstOrDefault(c => c.Data == "academicsCode")?.Search.Value ?? null;
                filter.SchoolYear = request.Columns.FirstOrDefault(c => c.Data == "schoolYear")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _syncEduDatabaseService.GetClass(filter, skip, take);
            return Ok(
                new DataTableResponse<AsAcademyClass>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> GetLecturer([FromBody] DataTableRequest request)
        {
            var filter = new LecturerFilter();
            if (request.Columns != null)
            {
                filter.Code = request.Columns.FirstOrDefault(c => c.Data == "code")?.Search.Value ?? null;
                filter.FullName = request.Columns.FirstOrDefault(c => c.Data == "fullName")?.Search.Value ?? null;
                filter.DepartmentCode = request.Columns.FirstOrDefault(c => c.Data == "departmentCode")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _syncEduDatabaseService.GetLecturer(filter, skip, take);
            return Ok(
                new DataTableResponse<AsAcademyLecturer>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> GetStudent([FromBody] DataTableRequest request)
        {
            var filter = new StudentFilter();
            if (request.Columns != null)
            {
                filter.Code = request.Columns.FirstOrDefault(c => c.Data == "code")?.Search.Value ?? null;
                filter.FullName = request.Columns.FirstOrDefault(c => c.Data == "fullName")?.Search.Value ?? null;
                filter.ClassCode = request.Columns.FirstOrDefault(c => c.Data == "classCode")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _syncEduDatabaseService.GetStudent(filter, skip, take);
            return Ok(
                new DataTableResponse<AsAcademyStudent>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }
        #endregion

        #region đồng bộ kỳ trước
        [HttpPut]
        public async Task<IActionResult> SyncLastClassRoom()
        {
            try
            {
                await _syncEduDatabaseService.SyncLastClassRoom();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ lớp môn học kỳ trước" });
            }
            catch (CallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> SyncLastLecturerClassRoom()
        {
            try
            {
                await _syncEduDatabaseService.SyncLastLecturerClassRoom();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ lớp môn học giảng viên kỳ trước" });
            }
            catch (CallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> SyncLastStudentClassRoom()
        {
            try
            {
                await _syncEduDataBackgroundTask.SyncLastStudentClassroom();
                return Ok();
            }
            catch (TableBusyException e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
            catch (CallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStatusSyncLastStudentClassroomTask()
        {
            try
            {
                var status = await _statusService.GetStatusTableTask(TableNameTask.AsAcademyStudentClassRoom);
                return Ok(new { status.Status, status.IsSuccess, status.Message });
            }
            catch (RecordNotFoundException e)
            {
                _logger.LogError(e, e.Message);
                return NotFound(new { message = "Không lấy được trạng thái", detailMessage = e.Message });
            }
        }
        #endregion

        #region xem data kỳ trước
        [HttpPost]
        public async Task<IActionResult> GetLastClassRoom([FromBody] DataTableRequest request)
        {
            var filter = new ClassRoomFilter();
            if (request.Columns != null)
            {
                filter.Code = request.Columns.FirstOrDefault(c => c.Data == "code")?.Search.Value ?? null;
                filter.GroupCode = request.Columns.FirstOrDefault(c => c.Data == "groupCode")?.Search.Value ?? null;
                filter.ClassCode = request.Columns.FirstOrDefault(c => c.Data == "classCode")?.Search.Value ?? null;
                filter.SubjectCode = request.Columns.FirstOrDefault(c => c.Data == "subjectCode")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _syncEduDatabaseService.GetLastClassRoom(filter, skip, take);
            return Ok(
                new DataTableResponse<AsAcademyClassRoom>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> GetLastLecturerClassRoom([FromBody] DataTableRequest request)
        {
            var filter = new LecturerClassRoomFilter();
            if (request.Columns != null)
            {
                filter.ClassRoomCode = request.Columns.FirstOrDefault(c => c.Data == "classRoomCode")?.Search.Value ?? null;
                filter.LecturerCode = request.Columns.FirstOrDefault(c => c.Data == "lecturerCode")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _syncEduDatabaseService.GetLastLecturerClassRoom(filter, skip, take);
            return Ok(
                new DataTableResponse<AsAcademyLecturerClassRoom>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> GetLastStudentClassRoom([FromBody] DataTableRequest request)
        {
            var filter = new StudentClassRoomFilter();
            if (request.Columns != null)
            {
                filter.StudentCode = request.Columns.FirstOrDefault(c => c.Data == "studentCode")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _syncEduDatabaseService.GetLastStudentClassRoom(filter, skip, take);
            return Ok(
                new DataTableResponse<AsAcademyStudentClassRoom>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }
        #endregion

        #region đồng bộ kỳ hiện tại
        [HttpPut]
        public async Task<IActionResult> SyncCurrentClassRoom()
        {
            try
            {
                await _syncEduDatabaseService.SyncCurrentClassRoom();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ lớp môn học kỳ hiện tại", detailMessage = e.Message });
            }
            catch (CallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> SyncCurrentLecturerClassRoom()
        {
            try
            {
                await _syncEduDatabaseService.SyncCurrentLecturerClassRoom();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ lớp môn học giảng viên kỳ hiện tại", detailMessage = e.Message });
            }
            catch (CallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> SyncCurrentStudentClassRoom()
        {
            try
            {
                var message = await _syncEduDatabaseService.SyncCurrentStudentClassRoom();
                return Ok(new { message });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ lớp môn học sinh viên kỳ hiện tại", detailMessage = e.Message });
            }
            catch (CallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> SyncUpdateFromDateEndDateCurrentClassRoom()
        {
            try
            {
                var message = await _syncEduDatabaseService.SyncUpdateFromDateEndDateCurrentClassRoom();
                return Ok(new { message });
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể cập nhật dữ liệu đồng bộ thời gian bắt đầu và kết thúc tuần của lớp môn học kỳ hiện tại", detailMessage = e.Message });
            }
            catch (CallEduWebServiceException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể lấy dữ liệu từ đào tạo", detailMessage = e.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }
        #endregion

        #region xem data kỳ hiện tại
        [HttpPost]
        public async Task<IActionResult> GetCurrentClassRoom([FromBody] DataTableRequest request)
        {
            var filter = new ClassRoomFilter();
            if(request.Columns != null)
            {
                filter.Code = request.Columns.FirstOrDefault(c => c.Data == "code")?.Search.Value ?? null;
                filter.GroupCode = request.Columns.FirstOrDefault(c => c.Data == "groupCode")?.Search.Value ?? null;
                filter.ClassCode = request.Columns.FirstOrDefault(c => c.Data == "classCode")?.Search.Value ?? null;
                filter.SubjectCode = request.Columns.FirstOrDefault(c => c.Data == "subjectCode")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _syncEduDatabaseService.GetCurrentClassRoom(filter, skip, take);
            return Ok(
                new DataTableResponse<AsAcademyCClassRoom>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> GetCurrentLecturerClassRoom([FromBody] DataTableRequest request)
        {
            var filter = new LecturerClassRoomFilter();
            if (request.Columns != null)
            {
                filter.ClassRoomCode = request.Columns.FirstOrDefault(c => c.Data == "classRoomCode")?.Search.Value ?? null;
                filter.LecturerCode = request.Columns.FirstOrDefault(c => c.Data == "lecturerCode")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _syncEduDatabaseService.GetCurrentLecturerClassRoom(filter, skip, take);
            return Ok(
                new DataTableResponse<AsAcademyCLecturerClassRoom>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> GetCurrentStudentClassRoom([FromBody] DataTableRequest request)
        {
            var filter = new StudentClassRoomFilter();
            if (request.Columns != null)
            {
                filter.StudentCode = request.Columns.FirstOrDefault(c => c.Data == "studentCode")?.Search.Value ?? null;
            }
            var skip = request.Start;
            var take = request.Length;
            var result = await _syncEduDatabaseService.GetCurrentStudentClassRoom(filter, skip, take);
            return Ok(
                new DataTableResponse<AsAcademyCStudentClassRoom>
                {
                    Draw = ++request.Draw,
                    RecordsTotal = result.RecordsTotal,
                    RecordsFiltered = result.RecordsFiltered,
                    Data = result.Data
                }
            );
        }
        #endregion

        #region xoá bỏ dữ liệu
        [HttpDelete]
        public async Task<IActionResult> TruncateTable(
            [Required(AllowEmptyStrings = false)]
            [NotContainWhiteSpace]
            string tableName)
        {
            switch (tableName)
            {
                case "AS_Academy_Faculty":
                case "AS_Academy_Department":
                case "AS_Academy_Academics":
                case "AS_Academy_Subject":
                case "AS_Academy_Class":
                case "AS_Academy_Lecturer":
                case "AS_Academy_Student":
                case "AS_Academy_ClassRoom":
                case "AS_Academy_Lecturer_ClassRoom":
                case "AS_Academy_Student_ClassRoom":
                case "AS_Academy_C_ClassRoom":
                case "AS_Academy_C_Lecturer_ClassRoom":
                case "AS_Academy_C_Student_ClassRoom":
                    break;
                default:
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "Tên bảng không hợp lệ" });
            }
            try
            {
                await _syncEduDatabaseService.TruncateTable(tableName);
                return Ok();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Không thể xoá bỏ dữ liệu trong bảng" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = e.Message });
            }
        }
        #endregion
    }
}
