using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Services.Ctsv.Interfaces;
using nuce.web.api.ViewModel;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Ctsv;

namespace nuce.web.api.Controllers.Ctsv
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService _studentService)
        {
            this._studentService = _studentService;
        }
        /// <summary>
        /// Thông tin sv
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("{code}")]
        [HttpGet]
        public IActionResult GetStudent(string code)
        {
            return Ok(_studentService.GetStudentByCode(code));
        }

        [Route("full-student/{code}")]
        [HttpGet]
        public async Task<IActionResult> GetFullStudent(string code)
        {
            return Ok(await _studentService.GetFullStudentByCode(code));
        }

        [Route("allow-update-student/{code}")]
        [HttpGet]
        public IActionResult GetAllowUpdateStudent(string code)
        {
            return Ok(_studentService.GetStudentByCodeAllowUpdate(code));
        }

        [AllowAnonymous]
        [Route("avatar/{code}")]
        [HttpGet]
        public async Task<IActionResult> GetImage(string code, [FromQuery] int? width, [FromQuery] int? height)
        {
            try
            {
                var result = await _studentService.GetStudentAvatar(code, width, height);
                return File(result, "image/jpg");
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        [Route("basic-update")]
        [HttpPost]
        public async Task<IActionResult> BasicUpdate([FromBody] StudentUpdateModel model)
        {
            var result = await _studentService.UpdateStudentBasic(model);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok();
        }

        [Route("update")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AsAcademyStudent student)
        {
            var result = await _studentService.UpdateStudent(student);
            if (result != null)
            {
                return BadRequest(result);
            }
            return Ok();
        }

        [Route("upload-avatar/{studentCode}")]
        [HttpPost]
        public async Task<IActionResult> UploadAvatar([FromForm] IFormFile file, string studentCode)
        {
            try
            {
                string imgPath = await _studentService.UpdateStudentImage(file, studentCode);
                return Ok(imgPath);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Message = ex.Message, Data = ex });
            }
        }

        [Route("search-students")]
        [HttpPost]
        public async Task<IActionResult> SearchStudents([FromBody] DataTableRequest request)
        {
            string searchText = request.Search.Value;
            try
            {
                var rs = await _studentService.FindStudent(searchText, request.Start, request.Length);
                rs.Draw = ++request.Draw;

                return Ok(rs);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseBody { Data = ex, Message = "Lỗi hệ thống khi tìm kiếm sinh viên" });
            }
        }
    }
}