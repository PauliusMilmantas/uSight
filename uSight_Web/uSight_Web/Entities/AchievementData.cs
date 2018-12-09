using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uSight_Web.Models;

namespace uSight_Web.Entities
{
    public class AchievementData
    {
        public int GetCurrentTier (string groupName, int count)
        {
            if (count == 0) return 0;
            else if (count < 0) return -1;
            else groupName = groupName.ToLower();

            switch (groupName)
            {
                case "text searcher":
                {
                    int a = 25, b = 100, c = 300;
                    if (count < a) return 1;
                    else if (count >= a && count < b) return 2;
                    else if (count >= b && count < c) return 3;
                    else return 4;
                }

                case "image searcher":
                {
                    int a = 10, b = 50, c = 100;
                    if (count < a) return 1;
                    else if (count >= a && count < b) return 2;
                    else if (count >= b && count < c) return 3;
                    else return 4;
                }
                case "commenter":
                {
                    int a = 10, b = 30, c = 100;
                    if (count < a) return 1;
                    else if (count >= a && count < b) return 2;
                    else if (count >= b && count < c) return 3;
                    else return 4;
                }
                case "wanted plates finder":        // by both images and text searches
                {
                    int a = 5, b = 20, c = 50;
                    if (count < a) return 1;
                    else if (count >= a && count < b) return 2;
                    else if (count >= b && count < c) return 3;
                    else return 4;
                }
                case "wanted images finder":
                {
                    int a = 2, b = 10, c = 30;
                    if (count < a) return 1;
                    else if (count >= a && count < b) return 2;
                    else if (count >= b && count < c) return 3;
                    else return 4;
                }
                case "overall achiever": 
                {
                    int a = 5, b = 10, c = 20;
                    if (count < a) return 1;
                    else if (count >= a && count < b) return 2;
                    else if (count >= b && count < c) return 3;
                    else return 4;
                }
                default:
                    return -1;
            }
        }

        public string GetTierName (string groupName, int tier)
        {
            if (tier < 0 || tier > 4) return "";
            else if (tier == 0) return "Unranked";
            else groupName = groupName.ToLower();

            switch (groupName)
            {
                case "text searcher":
                {
                    switch (tier)
                    {
                        case 1: return "Novice Text Searcher";
                        case 2: return "Skilled Text Searcher";
                        case 3: return "Advanced Text Searcher";
                        case 4: return "Expert Text Searcher";
                        default: return "";
                    }
                }
                case "image searcher":
                {
                    switch (tier)
                    {
                        case 1: return "Novice Image Searcher";
                        case 2: return "Skilled Image Searcher";
                        case 3: return "Advanced Image Searcher";
                        case 4: return "Expert Image Searcher";
                        default: return "";
                    }
                }
                case "commenter":
                {
                    switch (tier)
                    {
                        case 1: return "Newbie Commenter";
                        case 2: return "Intermediate Commenter";
                        case 3: return "Experienced Commenter";
                        case 4: return "Senior Commenter";
                        default: return "";
                    }
                }
                case "wanted plates finder":        // by both images and text searches
                {
                    switch (tier)
                    {
                        case 1: return "Rookie Spotter";
                        case 2: return "Skilled Spotter";
                        case 3: return "Expert Spotter";
                        case 4: return "Phenomenal Spotter";
                        default: return "";
                    }
                }
                case "wanted images finder":
                {
                    switch (tier)
                    {
                        case 1: return "Newbie Tracker";
                        case 2: return "Hobbyist";
                        case 3: return "Hunter";
                        case 4: return "Eagle Eye";
                        default: return "";
                    }
                }
                case "overall achiever":
                {
                    switch (tier)
                    {
                        case 1: return "Rookie";
                        case 2: return "Experienced";
                        case 3: return "Tryhard";
                        case 4: return "uSight Senior";
                        default: return "";
                    }
                }
                default:
                    return "";
            }
        }

        public int GetOverallAchieverTier (string userID)
        {
            ApplicationDbContext db = ApplicationDbContext.Create();
            var query =
                from i in db.Achievements
                where i.UserId == userID && i.GroupName != "Overall Achiever"
                select i.Tier;

            int count = query.Sum();
            return GetCurrentTier("Overall Achiever", count);
        }
    }
}