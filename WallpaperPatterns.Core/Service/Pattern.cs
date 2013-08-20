using System;
using System.Collections.Generic;

namespace WallpaperPatterns.Core.Service
{
    public class Pattern
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
        public int NumViews { get; set; }
        public int NumVotes { get; set; }
        public int NumComments { get; set; }
        public int NumHearts { get; set; }
        public int Rank { get; set; }
        public DateTime DateCreated { get; set; }
        public List<string> Colors { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string BadgeUrl { get; set; }
        public string ApiUrl { get; set; }

        public string ByUserName
        {
            get { return "By " + UserName; }
        }
    }
}