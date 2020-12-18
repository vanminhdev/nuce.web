using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using nuce.web.api.Models.Core;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Core;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace nuce.web.api.Services.Core.Implements
{
    public class NewsManagerService : INewsManagerService
    {
        private readonly NuceCoreIdentityContext _nuceCoreContext;
        private readonly IUserService _userService;
        private readonly IUploadFile _uploadFile;
        private readonly IPathProvider _pathProvider;
        private readonly IConfiguration _configuration;
        public NewsManagerService(NuceCoreIdentityContext _nuceCoreContext, IUserService _userService,
                                IUploadFile _uploadFile, IPathProvider _pathProvider, IConfiguration _configuration)
        {
            this._nuceCoreContext = _nuceCoreContext;
            this._userService = _userService;
            this._uploadFile = _uploadFile;
            this._pathProvider = _pathProvider;
            this._configuration = _configuration;
        }
        
        public IQueryable GetAllActiveCategoryByRole(string role)
        {
            return _nuceCoreContext.NewsCats.AsNoTracking()
                                .Where(c => c.Role == role && c.Status == 1)
                                .OrderBy(c => c.Count);
        }

        public async Task<DataTableResponse<NewsItems>> FindItemsByCatId(int catId, int seen, int size)
        {
            var catChildren = _nuceCoreContext.NewsCats.Where(cat => cat.Parent == catId && cat.Parent != -1 && cat.Status == 1);
            bool isParent = catChildren != null && catChildren.Any();
            IQueryable<NewsItems> data = null;

            if (isParent)
            {
                data = catChildren.AsNoTracking().GroupJoin(
                            _nuceCoreContext.NewsItems.AsNoTracking(),
                            cat => cat.Id,
                            newsItem => newsItem.CatId,
                            (cat, newsItem) => new { cat, newsItem }
                        ).SelectMany(
                            left => left.newsItem.DefaultIfEmpty(),
                            (left, newsitem) => newsitem
                        ).Where(ni => ni != null)
                        .OrderByDescending(ni => ni.EntryDatetime);
            }
            else
            {
                data = _nuceCoreContext.NewsItems.AsNoTracking()
                            .Where(ni => ni.CatId == catId)
                            .OrderByDescending(ni => ni.EntryDatetime);
            }

            var takedData = await data.Skip(seen).Take(size).ToListAsync();
            return new DataTableResponse<NewsItems>
            {
                Data = takedData,
                RecordsFiltered = takedData.Count(),
                RecordsTotal = data.Count()
            };
        }

        public async Task<NewsItems> FindNewsItemById(int id)
        {
            return await _nuceCoreContext.NewsItems.FindAsync(id);
        }

        public async Task<int> CreateNewsItems(CreateNewsItemModel model)
        {
            var username = _userService.GetUserName();
            var now = DateTime.Now;

            var cat = await _nuceCoreContext.NewsCats.FindAsync(model.CatId);
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

                    await _nuceCoreContext.NewsItems.AddAsync(newsItems);
                    await _nuceCoreContext.SaveChangesAsync();
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

            var newsItem = await _nuceCoreContext.NewsItems.FindAsync(id);
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
            await _nuceCoreContext.SaveChangesAsync();
            return newsItem.Avatar;
        }

        public async Task<ItemAvatarModel> GetNewsItemAvatar(int id, int? width, int? height)
        {
            var newsItemData = await FindNewsItemById(id);

            if (newsItemData == null)
            {
                return null;
            }

            var imgPath = newsItemData.Avatar;
            string extension = Path.GetExtension(imgPath);
            extension = extension.Substring(1, extension.Length -1);

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

        public async Task UpdateNewsItems(NewsItems model)
        {
            var newsItems = await FindNewsItemById(model.Id);

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

            await _nuceCoreContext.SaveChangesAsync();
        }

        private IQueryable GetAllActiveCategory()
        {
            return _nuceCoreContext.NewsCats.AsNoTracking()
                                .Where(c => c.Status == 1);
        }

    }
}
