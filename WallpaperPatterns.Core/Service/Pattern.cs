using System;
using System.Collections.Generic;

namespace WallpaperPatterns.Core.Service
{
    public class Pattern : IEquatable<Pattern>
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

        public bool Equals(Pattern other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Pattern)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}