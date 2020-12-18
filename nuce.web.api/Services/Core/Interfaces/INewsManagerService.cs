﻿using Microsoft.AspNetCore.Http;
using nuce.web.api.Models.Core;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Services.Core.Interfaces
{
    public interface INewsManagerService
    {
        public IQueryable GetAllCategoryByRole(string role, int? status);
        public Task<DataTableResponse<NewsItems>> FindItemsByCatId(int catId, int seen, int size, int? status);
        public Task<NewsItems> FindNewsItemById(int id, int? status);
        public Task<int> CreateNewsItems(CreateNewsItemModel model);
        public Task UpdateNewsItems(NewsItems model);
        public Task<string> UploadNewsItemAvatar(IFormFile formFile, int id);
        public Task<ItemAvatarModel> GetNewsItemAvatar(int id, int? width, int? height);
    }
}
