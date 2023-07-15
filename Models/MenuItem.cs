﻿namespace ArtHive.Models
{
    public class MenuItem
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Label { get; set; }
        public bool? Authorized { get; set; }
        public List<string>? AllowedRoles { get; set; }
        public List<MenuItem> DropdownItems { get; set; }
    }
}
