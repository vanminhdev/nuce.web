using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using nuce.web.api.HandleException;
using nuce.web.api.Models.Core;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace nuce.web.api.Services.Core.Implements
{
    public class NewsManagerService : INewsManagerService
    {
        private readonly NuceCoreIdentityContext _context;
        private readonly IUserService _userService;
        private readonly IUploadFile _uploadFile;
        private readonly IConfiguration _configuration;
        public NewsManagerService(NuceCoreIdentityContext _nuceCoreContext, IUserService _userService,
                                IUploadFile _uploadFile, IConfiguration _configuration)
        {
            this._context = _nuceCoreContext;
            this._userService = _userService;
            this._uploadFile = _uploadFile;
            this._configuration = _configuration;
        }
        

        #region admin
        public async Task<int> CreateNewsItems(CreateNewsItemModel model)
        {
            var username = _userService.GetUserName();
            var now = DateTime.Now;

            var cat = await _context.NewsCats.FindAsync(model.CatId);
            if (cat == null)
            {
                throw new Exception("Mã danh mục không chính xác");
            }
            if (string.IsNullOrEmpty(model.Title?.Trim()))
            {
                throw new Exception("Tiêu đề không được để trống");
            }

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var newsItems = new NewsItems
                    {
                        CatId = model.CatId,
                        NewContent = model.Content,
                        Title = model.Title,
                        Description = model.Description,
                        EntryUsername = username,
                        UpdateUsername = username,
                        EntryDatetime = now,
                        UpdateDatetime = now,
                        TotalView = 1,
                        Status = 1,
                    };

                    await _context.NewsItems.AddAsync(newsItems);
                    await _context.SaveChangesAsync();
                    scope.Complete();
                    return newsItems.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> UploadNewsItemAvatar(IFormFile formFile, int id)
        {
            if (!_uploadFile.isValidImageUpload(formFile))
            {
                throw new Exception("Ảnh không hợp lệ");
            }
            // 1mb
            long maxSize = 1024 * 1024;
            if (!_uploadFile.isValidSize(formFile, maxSize))
            {
                throw new Exception("Dung lượng phải nhỏ hơn 1MB");
            }

            var newsItem = await _context.NewsItems.FindAsync(id);
            if (newsItem == null)
            {
                throw new Exception("Bài tin không tồn tại");
            }
            var baseDir = @$"{_configuration.GetValue<string>("FolderResources")}";
            var dir = Path.Combine(baseDir, "Images");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string newFileName = $"news_item_ava_{id}{Path.GetExtension(formFile.FileName).ToLower()}";
            string filePath = $"{dir}/{newFileName}";

            try
            {
                await _uploadFile.SaveFileAsync(formFile, filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            newsItem.Avatar = filePath;
            await _context.SaveChangesAsync();
            return newsItem.Avatar;
        }

        public async Task UpdateNewsItems(NewsItems model)
        {
            var newsItems = await FindNewsItemById(model.Id, null);

            if (string.IsNullOrEmpty(model.Title?.Trim()))
            {
                throw new Exception("Tiêu đề không được để trống");
            }

            newsItems.NewContent = model.NewContent;
            newsItems.CatId = model.CatId;
            newsItems.Title = model.Title;
            newsItems.Description = model.Description;
            newsItems.UpdateDatetime = DateTime.Now;
            newsItems.UpdateUsername = _userService.GetUserName();

            await _context.SaveChangesAsync();
        }

        public IQueryable<NewsCats> GetAllCategoryByRoleAdmin(string role)
        {
            return _context.NewsCats.AsNoTracking()
                                .Where(c => c.Role == role)
                                .OrderBy(c => c.Count);
        }
        /// <summary>
        /// Tạo mới danh mục
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<NewsCats> CreateNewsCatsAdmin(List<string> roles, CreateNewsCategoryModel model)
        {
            var parent = new NewsCats
            {
                Role = model.Role,
            };
            if (model.Parent != -1)
            {
                parent = await _context.NewsCats.FindAsync(model.Parent);
                if (parent == null)
                {
                    throw new RecordNotFoundException("Danh mục cha không tồn tại");
                }
            }
            if (!roles.Contains(parent.Role))
            {
                throw new UnauthorizedAccessException("Bạn không có quyền");
            }
            var modelRole = parent.Role;

            var modelCount = _context.NewsCats.Count() + 1;

            var modelId = await _context.NewsCats.MaxAsync(c => c.Id) + 1;

            var newsCat = new NewsCats
            {
                Id = modelId,
                Name = model.Name?.Trim(),
                Description = model.Name?.Trim(),
                Parent = model.Parent,
                OnMenu = true,
                DivideAfter = false,
                Status = 1,
                Role = modelRole,
                Count = modelCount
            };

            try
            {
                await _context.NewsCats.AddAsync(newsCat);
                await _context.SaveChangesAsync();
                return newsCat;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Sửa danh mục
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateNewsCatsAdmin(List<string> roles, NewsCats model)
        {
            var recordNewsCat = await _context.NewsCats.FindAsync(model.Id);
            if (recordNewsCat == null)
            {
                throw new RecordNotFoundException("Danh mục này không tồn tại");
            }
            if (!roles.Contains(model.Role))
            {
                throw new UnauthorizedAccessException("Bạn không có quyền");
            }
            recordNewsCat.Name = model.Name?.Trim();
            recordNewsCat.Description = model.Description?.Trim();
            recordNewsCat.OnMenu = model.OnMenu;
            recordNewsCat.Status = model.Status;
            recordNewsCat.DivideAfter = model.DivideAfter;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region client
        public async Task<ItemAvatarModel> GetNewsItemAvatar(int id, int? width, int? height)
        {
            var newsItemData = await FindNewsItemById(id, null);

            if (newsItemData == null)
            {
                return null;
            }

            var imgPath = newsItemData.Avatar;
            string extension = Path.GetExtension(imgPath);
            extension = extension.Substring(1, extension.Length - 1);

            if (width == null || height == null)
            {
                var data = await File.ReadAllBytesAsync(imgPath);
                return new ItemAvatarModel { Data = data, Extension = extension };
            }
            Image img = Image.FromFile(imgPath);

            Image resizedNewImg = _uploadFile.ResizeImage(img, width ?? 0, 2000, false);
            var newImg = _uploadFile.CropImage(resizedNewImg, width ?? 0, height ?? 0);

            var result = _uploadFile.ImageToByte(newImg);
            return new ItemAvatarModel { Data = result, Extension = extension };
        }
        #endregion

        #region common
        public IQueryable<NewsCats> GetAllCategoryByRole(string role, int? status)
        {
            return _context.NewsCats.AsNoTracking()
                                .Where(c => c.Role == role && (status == null || c.Status == status))
                                .OrderBy(c => c.Count);
        }

        public IQueryable<NewsCats> GetAllCategoryByRole(string role, int? status, bool onMenu)
        {
            return _context.NewsCats
                                .Where(c => c.Role == role && (status == null || c.Status == status) && (c.OnMenu ?? false) == onMenu)
                                .OrderBy(c => c.Count);
        }
        /// <summary>
        /// Danh sách bài tin theo danh mục
        /// </summary>
        /// <param name="catId"></param>
        /// <param name="seen"></param>
        /// <param name="size"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<DataTableResponse<NewsItems>> FindItemsByCatId(int catId, int seen, int size, int? status)
        {
            var catChildren = _context.NewsCats.Where(cat => (cat.Parent == catId || cat.Id == catId) && (status == null || cat.Status == status));
            IQueryable<NewsItems> data = null;

            data = catChildren.AsNoTracking().GroupJoin(
                        _context.NewsItems.AsNoTracking(),
                        cat => cat.Id,
                        newsItem => newsItem.CatId,
                        (cat, newsItem) => new { cat, newsItem }
                    ).SelectMany(
                        left => left.newsItem.DefaultIfEmpty(),
                        (left, newsitem) => newsitem
                    ).Where(ni => ni != null)
                    .OrderByDescending(ni => ni.EntryDatetime);

            var takedData = await data.Skip(seen).Take(size).ToListAsync();
            return new DataTableResponse<NewsItems>
            {
                Data = takedData,
                RecordsFiltered = takedData.Count(),
                RecordsTotal = data.Count()
            };
        }

        public async Task<IQueryable<NewsItems>> GetCousinNewsItemsById(int id)
        {
            var newsItem = await _context.NewsItems.FindAsync(id);
            if (newsItem == null)
            {
                throw new RecordNotFoundException("Bài tin không tồn tại");
            }
            var lesserList = _context.NewsItems.Where(ni => ni.EntryDatetime < newsItem.EntryDatetime && ni.CatId == newsItem.CatId)
                                                .OrderByDescending(ni => ni.EntryDatetime)
                                                .Take(2);
            var greaterList = _context.NewsItems.Where(ni => ni.EntryDatetime > newsItem.EntryDatetime && ni.CatId == newsItem.CatId)
                                                .OrderBy(ni => ni.EntryDatetime)
                                                .Take(2);
            return (lesserList.Concat(greaterList)).OrderByDescending(ni => ni.EntryDatetime);
        }

        public async Task<NewsItems> FindNewsItemById(int id, int? status)
        {
            return await _context.NewsItems
                            .FirstOrDefaultAsync(ni => ni.Id == id && (status == null || ni.Status == status));
        }
        #endregion
    }
}
