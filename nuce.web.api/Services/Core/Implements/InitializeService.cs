﻿using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.EduData;
using nuce.web.api.Models.Survey;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel.Core.NuceIdentity;
using nuce.web.shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace nuce.web.api.Services.Core.Implements
{
    public class InitializeService : IInitializeService
    {
        private readonly IUserService _userService;
        private readonly EduDataContext _eduDataContext;
        public InitializeService(IUserService _userService, EduDataContext _eduDataContext)
        {
            this._userService = _userService;
            this._eduDataContext = _eduDataContext;
        }
        
        public async Task CreateFacultyDepartmentUsers()
        {
            string password = "123456@aA";
            var facultyCodeList = (await _eduDataContext.AsAcademyFaculty.ToListAsync()).Select(f => f.Code);
            var facultyList = (await _eduDataContext.AsAcademyDepartment
                                    .Where(d => (d.Code != "**" && 
                                                facultyCodeList.Contains(d.Code) && 
                                                d.Code == d.FacultyCode) || d.Code == "KMT")
                                    .ToListAsync()
                                )
                                .Select(f => new UserCreateModel
                                {
                                    Email = "",
                                    Password = password,
                                    Username = f.Code,
                                    Roles = new List<string> { RoleNames.KhaoThi_Survey_KhoaBan },
                                });
            var finalFacultyCodeList = facultyList.Select(f => f.Username);
            var deparmentList = (await _eduDataContext.AsAcademyDepartment
                                .Where(d => d.Code != "**" && !finalFacultyCodeList.Contains(d.Code)).ToListAsync())
                                .Select(d => new UserCreateModel
                                {
                                    Email = "",
                                    Password = password,
                                    Username = d.Code,
                                    Roles = new List<string> { RoleNames.KhaoThi_Survey_Department },
                                });
            var allAccountModels = facultyList.Concat(deparmentList);

            foreach (var model in allAccountModels)
            {
                try
                {
                    await _userService.CreateUser(model);
                }
                catch (System.Exception ex)
                {
                    
                }
            }

        }
    }
}
