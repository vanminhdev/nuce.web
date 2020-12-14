using Microsoft.EntityFrameworkCore;
using nuce.web.api.Models.Core;
using nuce.web.api.Services.Core.Interfaces;
using nuce.web.api.ViewModel.Base;
using nuce.web.api.ViewModel.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace nuce.web.api.Services.Core.Implements
{
    public class NewsManagerService : INewsManagerService
    {
        private readonly NuceCoreIdentityContext _nuceCoreContext;
        private readonly IUserService _userService;
        public NewsManagerService(NuceCoreIdentityContext _nuceCoreContext, IUserService _userService)
        {
            this._nuceCoreContext = _nuceCoreContext;
            this._userService = _userService;
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

        public async Task CreateNewsItems(CreateNewsItemModel model)
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
                        Status = 1
                    };
                    await _nuceCoreContext.NewsItems.AddAsync(newsItems);

                    await _nuceCoreContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
