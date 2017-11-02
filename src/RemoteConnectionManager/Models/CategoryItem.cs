﻿using RemoteConnectionManager.Core;
using System.Collections.Generic;

namespace RemoteConnectionManager.Models
{
    public class CategoryItem
    {
        public CategoryItem()
        {
            Items = new List<CategoryItem>();
        }

        public string DisplayName { get; set; }

        public ConnectionSettings ConnectionSettings { get; set; }
        public Credentials Credentials { get; set; }
        public List<CategoryItem> Items { get; set; }
    }
}