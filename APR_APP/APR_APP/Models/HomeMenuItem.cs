﻿using System;
using System.Collections.Generic;
using System.Text;

namespace APR_APP.Models
{
    public enum MenuItemType
    {
        Browse,
        Camera,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}