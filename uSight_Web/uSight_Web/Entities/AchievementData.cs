using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSight_Web.Entities
{
    public class AchievementData
    {
        public int DetermineTier (string userID, string groupName, int count)
        {
            if (count == 0) return 0;
            else if (count < 0) return -1;
            else groupName = groupName.ToLower();

            switch (groupName)
            {
                case "searcher by text":
                {
                    int a = 25, b = 100, c = 300;
                    if (count < a) return 1;
                    else if (count >= a && count < b) return 2;
                    else if (count >= b && count < c) return 3;
                    else if (count >= c) return 4;
                    break;
                }

                case "searcher by image":
                {
                    int a = 10, b = 50, c = 100;
                    if (count < a) return 1;
                    else if (count >= a && count < b) return 2;
                    else if (count >= b && count < c) return 3;
                    else if (count >= c) return 4;
                    break;
                }
                case "comment writer":
                {
                    int a = 10, b = 30, c = 100;
                    if (count < a) return 1;
                    else if (count >= a && count < b) return 2;
                    else if (count >= b && count < c) return 3;
                    else if (count >= c) return 4;
                    break;
                }
                case "wanted plates finder":        // by both images and text searches
                {
                    int a = 5, b = 20, c = 50;
                    if (count < a) return 1;
                    else if (count >= a && count < b) return 2;
                    else if (count >= b && count < c) return 3;
                    else if (count >= c) return 4;
                    break;
                }
                case "wanted images finder":
                {
                    int a = 2, b = 10, c = 30;
                    if (count < a) return 1;
                    else if (count >= a && count < b) return 2;
                    else if (count >= b && count < c) return 3;
                    else if (count >= c) return 4;
                    break;
                }
                /*case "overall achiever":      // pridesiu jungimasi i db
                {

                    break;
                }*/

                default:
                    return -1;
            }
            return -1;
        }

    }
}