using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nuce.web.api.Models.Ctsv;
using nuce.web.api.Services.Ctsv.Interfaces;
using nuce.web.api.ViewModel;
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
    }
}