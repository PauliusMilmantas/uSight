using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uSight_Web.Models;

namespace uSight_Web.Entities
{
    public class AchievementData
    {
        private int nAchievs = 6;
        private int nValues = 3;
        private int nTiers = 4;

        private string[] data = { "Default",

            "Text Searcher", "25", "100", "300",
            "Novice Text Searcher", "Skilled Text Searcher",
            "Advanced Text Searcher", "Expert Text Searcher",

            "Image Searcher", "10", "50", "100",
            "Novice Image Searcher", "Skilled Image Searcher",
            "Advanced Image Searcher", "Expert Image Searcher",

            "Commenter", "10", "30", "100",
            "Newbie Commenter", "Intermediate Commenter",
            "Experienced Commenter", "Senior Commenter",

            "Wanted Plates Finder", "5", "20", "50",
            "Rookie Spotter", "Skilled Spotter",
            "Expert Spotter", "Phenomenal Spotter",

            "Wanted Images Finder", "2", "10", "30",
            "Newbie Tracker", "Hobbyist",
            "Hunter", "Eagle Eye",

            "Overall Achiever", "5", "10", "20",
            "Rookie", "Experienced",
            "Tryhard", "uSight Senior"
             };

        private string[] dataDesc = { "Default",
            "This user has made ", " text searches! ",
            "This user has searched by image ", " times! ",
            "This user has made ", " comments! ",
            "This user has found ", " wanted plates! ",
            "This user has uploaded images with wanted license plates ", " times! ",
            "This user has more than ", " uSight achievement points! "
            };

        public int GetTier (string groupName, int count)
        {
            if (count == 0) return 0;
            else if (count < 0) return -1;

            int i = FindGroupIndex(groupName);
            if (i == -1) return -1;

            i++;
            for (int k = i; k < i + nValues; k++) if (count < Int32.Parse(data[k])) return k - i + 1;
            return nValues + 1;
        }

        public string GetTierName (string groupName, int tier)
        {
            if (tier < 0 || tier > nTiers) return "";
            else if (tier == 0) return "Unranked";

            int i = FindGroupIndex(groupName);
            if (i == -1) return "";

            i += 4;
            for (int k = i; k < i + nTiers; k++) if (tier == k - i + 1) return data[k];
            return "";
        }

        private int GetTierLevelUpValues (string groupName, int tier)
        {
            if (tier <= 0 || tier > nTiers) return -1;
            else if (tier == 1) return 1;
            tier--;

            int i = FindGroupIndex(groupName);
            if (i == -1) return -1;

            i++;
            for (int k = i; k < i + nValues; k++) if (tier == k - i + 1) return Int32.Parse(data[k]);
            return -1;
        }

        public string GetTierDescription (string groupName, int tier)
        {
            if (tier <= 0 || tier > nTiers) return "";

            int i = FindGroupIndex(groupName);
            if (i == -1) return "";

            int groupNo = ((i - 1) / (nValues + nTiers + 1)) + 1;
            int count = GetTierLevelUpValues(groupName, tier);

            for (int k = 1; k <= nAchievs * 2; k += 2)
                if ((k + 1) / 2 == groupNo)
                    return dataDesc[k] + count + dataDesc[k + 1];

            return "";
        }

        private int FindGroupIndex(string groupName)
        {
            for (int k = 1; k <= nAchievs * 8; k += 8) if (data[k] == groupName) return k;
            return -1;
        }

        public int GetOverallAchieverCount(string userID)
        {
            ApplicationDbContext db = ApplicationDbContext.Create();
            var query =
                from i in db.Achievements
                where i.UserId == userID && i.GroupName != "Overall Achiever"
                select i.Tier;
            int count = query.Sum();

            return count;
        }

        public void RefreshUserAchievements(string userID, string groupName, int count)
        {
            ApplicationDbContext db = ApplicationDbContext.Create();

            var adQuery =
                from i in db.Achievements
                where i.UserId == userID && i.GroupName == groupName
                select i.Tier;
            int currentTier = adQuery.ToList()[0];

            int trueTier = GetTier(groupName, count);
            if (currentTier < trueTier)
            {
                Achievement existing = db.Achievements.Find(new object[] { userID, groupName, currentTier });
                Achievement newAchievement = new Achievement();
                newAchievement.UserId = existing.UserId;
                newAchievement.GroupName = existing.GroupName;
                newAchievement.Tier = existing.Tier;
                newAchievement.Name = existing.Name;
                newAchievement.Description = existing.Description;
                db.Achievements.Remove(existing);
                db.SaveChanges();

                existing = newAchievement;
                existing.Tier = trueTier;
                existing.Name = GetTierName(groupName, trueTier);
                existing.Description = GetTierDescription(groupName, trueTier);
                db.Achievements.Add(existing);
                db.SaveChanges();
            }
        }
    }
}