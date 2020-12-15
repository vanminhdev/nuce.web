﻿using nuce.web.api.Models.Core;
using System.Collections.Generic;

namespace nuce.web.api.ViewModel.Core
{
    public class CreateNewsItemModel
    {
        public int CatId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Avatar { get; set; }
        public string File { get; set; }
        //public List<NewsCatItem> OtherCategory { get; set; }
    }

    public class CreateNewsCategoryModel
    {
        public string Role { get; set; }
        public List<NewsCategoryParent> CategoryList { get; set; }
    }

    public class NewsCategoryParent : NewsCats
    {
        public List<NewsCats> Children { get; set; }
    }

    public class ItemAvatarModel
    {
        public byte[] Data { get; set; }
        public string Extension { get; set; }
    }

}
