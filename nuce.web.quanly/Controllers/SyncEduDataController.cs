using Newtonsoft.Json;
using nuce.web.quanly.Attributes.ActionFilter;
using nuce.web.quanly.Models;
using nuce.web.quanly.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace nuce.web.quanly.Controllers
{
    [AuthorizeActionFilter("Admin")]
    public class SyncEduDataController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        #region đồng bộ db khảo thí
        [HttpPost]
        public async Task<ActionResult> SyncFromEduData(string action)
        {
            switch(action)
            {
                case "SyncFaculty":
                case "SyncDepartment":
                case "SyncAcademics":
                case "SyncSubject":
                case "SyncClass":
                case "SyncLecturer":
                case "SyncStudent":
                case "SyncLastClassRoom":
                case "SyncLastLecturerClassRoom":
                case "SyncLastStudentClassRoom":
                case "SyncCurrentClassRoom":
                case "SyncCurrentLecturerClassRoom":
                case "SyncCurrentStudentClassRoom":
                case "SyncUpdateFromDateEndDateCurrentClassRoom":
                    break;
                default:
                    return Json(new { type = "error", message = "Hành động không hợp lệ", detailMessage = "" },JsonRequestBehavior.AllowGet);
            }

            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/SyncEduData/{action}");
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    return Json(new { type = "success", message = "Đồng bộ thành công" }, JsonRequestBehavior.AllowGet);
                },
                action500Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    return Json(new { type = "error", message = "Đồng bộ không thành công", detailMessage = resMess.message }, JsonRequestBehavior.AllowGet);
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> SyncLastStudentClassRoom()
        {
            var response = await base.MakeRequestAuthorizedAsync("Put", $"/api/SyncEduData/SyncLastStudentClassRoom");
            return Json(new { statusCode = response.StatusCode, content = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> TruncateTable(string action)
        {
            string tableName = "";
            switch (action)
            {
                case "SyncFaculty":
                    tableName = "AS_Academy_Faculty";
                    break;
                case "SyncDepartment":
                    tableName = "AS_Academy_Department";
                    break;
                case "SyncAcademics":
                    tableName = "AS_Academy_Academics";
                    break;
                case "SyncSubject":
                    tableName = "AS_Academy_Subject";
                    break;
                case "SyncClass":
                    tableName = "AS_Academy_Class";
                    break;
                case "SyncLecturer":
                    tableName = "AS_Academy_Lecturer";
                    break;
                case "SyncStudent":
                    tableName = "AS_Academy_Student";
                    break;
                case "SyncLastClassRoom":
                    tableName = "AS_Academy_ClassRoom";
                    break;
                case "SyncLastLecturerClassRoom":
                    tableName = "AS_Academy_Lecturer_ClassRoom";
                    break;
                case "SyncLastStudentClassRoom":
                    tableName = "AS_Academy_Student_ClassRoom";
                    break;
                case "SyncCurrentClassRoom":
                    tableName = "AS_Academy_C_ClassRoom";
                    break;
                case "SyncCurrentLecturerClassRoom":
                    tableName = "AS_Academy_C_Lecturer_ClassRoom";
                    break;
                case "SyncCurrentStudentClassRoom":
                    tableName = "AS_Academy_C_Student_ClassRoom";
                    break;
                default:
                    return Json(new { type = "error", message = "Hành động không hợp lệ", detailMessage = "" }, JsonRequestBehavior.AllowGet);
            }

            var response = await base.MakeRequestAuthorizedAsync("Delete", $"/api/SyncEduData/TruncateTable?tableName={tableName}");
            return await base.HandleResponseAsync(response,
                action200: res =>
                {
                    return Json(new { type = "success", message = "Xoá thành công" }, JsonRequestBehavior.AllowGet);
                },
                action400Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    return Json(new { type = "error", message = "Xoá không thành công", detailMessage = resMess.message }, JsonRequestBehavior.AllowGet);
                },
                action500Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resMess = JsonConvert.DeserializeObject<ResponseMessage>(jsonString);
                    return Json(new { type = "error", message = "Xoá không thành công", detailMessage = resMess.message }, JsonRequestBehavior.AllowGet);
                }
            );
        }
        #endregion

        #region xem dữ liệu
        [HttpPost]
        public async Task<ActionResult> GetAllFaculties()
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", "/api/SyncEduData/GetAllFaculties");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<AsAcademyFaculty>>(jsonString);
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                },
                action500: res =>
                {
                    return Json(new { type = "error", message = "không lấy được dữ liệu" }, JsonRequestBehavior.AllowGet);
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> GetAllDepartments()
        {
            var response = await base.MakeRequestAuthorizedAsync("Get", "/api/SyncEduData/GetAllDepartments");
            return await base.HandleResponseAsync(response,
                action200Async: async res =>
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<AsAcademyDepartment>>(jsonString);
                    return Json(new { data }, JsonRequestBehavior.AllowGet);
                },
                action500: res =>
                {
                    return Json(new { type = "error", message = "không lấy được dữ liệu" }, JsonRequestBehavior.AllowGet);
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> GetAcademics(DataTableRequest request)
        {
            return await GetDataTabeFromApi<AsAcademyAcademics>(request, "/api/SyncEduData/GetAcademics");
        }

        [HttpPost]
        public async Task<ActionResult> GetSubject(DataTableRequest request)
        {
            return await GetDataTabeFromApi<AsAcademySubject>(request, "/api/SyncEduData/GetSubject");
        }

        [HttpPost]
        public async Task<ActionResult> GetClass(DataTableRequest request)
        {
            return await GetDataTabeFromApi<AsAcademyClass>(request, "/api/SyncEduData/GetClass");
        }

        [HttpPost]
        public async Task<ActionResult> GetLecturer(DataTableRequest request)
        {
            return await GetDataTabeFromApi<AsAcademyLecturer>(request, "/api/SyncEduData/GetLecturer");
        }

        [HttpPost]
        public async Task<ActionResult> GetStudent(DataTableRequest request)
        {
            return await GetDataTabeFromApi<AsAcademyStudent>(request, "/api/SyncEduData/GetStudent");
        }

        [HttpPost]
        public async Task<ActionResult> GetLastClassRoom(DataTableRequest request)
        {
            return await GetDataTabeFromApi<AsAcademyClassRoom>(request, "/api/SyncEduData/GetLastClassRoom");
        }

        [HttpPost]
        public async Task<ActionResult> GetLastLecturerClassRoom(DataTableRequest request)
        {
            return await GetDataTabeFromApi<AsAcademyLecturerClassRoom>(request, "/api/SyncEduData/GetLastLecturerClassRoom");
        }

        [HttpPost]
        public async Task<ActionResult> GetLastStudentClassRoom(DataTableRequest request)
        {
            return await GetDataTabeFromApi<AsAcademyStudentClassRoom>(request, "/api/SyncEduData/GetLastStudentClassRoom");
        }

        [HttpPost]
        public async Task<ActionResult> GetCurrentClassRoom(DataTableRequest request)
        {
            return await GetDataTabeFromApi<AsAcademyCClassRoom>(request, "/api/SyncEduData/GetCurrentClassRoom");
        }

        [HttpPost]
        public async Task<ActionResult> GetCurrentLecturerClassRoom(DataTableRequest request)
        {
            return await GetDataTabeFromApi<AsAcademyCLecturerClassRoom>(request, "/api/SyncEduData/GetCurrentLecturerClassRoom");
        }

        [HttpPost]
        public async Task<ActionResult> GetCurrentStudentClassRoom(DataTableRequest request)
        {
            return await GetDataTabeFromApi<AsAcademyCStudentClassRoom>(request, "/api/SyncEduData/GetCurrentStudentClassRoom");
        }
        #endregion
    }
}