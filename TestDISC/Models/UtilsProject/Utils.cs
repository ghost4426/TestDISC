using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestDISC.Models.ResultDISC;

namespace TestDISC.Models.UtilsProject
{
    public class Utils
    {
        //public static readonly string SelfLink = "https://localhost:5001/";
        //public static readonly string SelfLink = "http://testdisc.topskills.com.vn/";
        //public static readonly string SelfLink = "http://test-disc.navu.vn/";

        //public static readonly string ResultLink = $"{SelfLink}Assets/ResultImage/";

        public static readonly string SavePath = Path.Combine("wwwroot", "Assets", "ResultImage");

        public static readonly string NameSession = "TestDISC_Session";
        public static readonly string NameCookie = "TestDISC_Cookie";
        public static readonly string NameRefreshCookie = "TestDISC_Refresh_Cookie";

        public static readonly string ClaimUId = "uid";
        public static readonly string ClaimPartnerId = "partnerid";

        public static readonly ulong TopSkillId = 1;

        public static DateTime DateNow()
        {
            return DateTime.Now;
        }

        public static int FindPointNumber(IList<PointAnswerModel> points, int value)
        {
            var pointValue = points.Where(a => a.point == value).SingleOrDefault();

            if (pointValue == null)
            {
                return 0;
            }
            else
            {
                return pointValue.number;
            }
        }

        public static string FindPointNumberWithType(IList<PointAnswerModel> points, int value, string typeGraph)
        {
            string[] types = new string[] { "", "D", "I", "S", "C" };
            var pointValue = points.Where(a => a.point == value).SingleOrDefault();

            if (pointValue == null)
            {
                return typeGraph + types[value] + 0;
            }
            else
            {
                return typeGraph + types[value] + pointValue.number;
            }
        }

        public static IList<MappingOnTable> BuildTableGraphMost()
        {
            var listMapping = new List<MappingOnTable>
            {
                new MappingOnTable(20, 17, 19, 15),
                new MappingOnTable(16, -1, 12, 9),
                new MappingOnTable(15, 10, 11, 8),
                new MappingOnTable(14, 9, -1, 7),
                new MappingOnTable(13, 8, 10, -1),
                new MappingOnTable(12, -1, -1, -1),
                new MappingOnTable(11, 7, 9, 6),
                new MappingOnTable(10, -1, 8, -1),
                new MappingOnTable(9, -1, -1, -1),
                new MappingOnTable(-1, 6, 7, 5),
                new MappingOnTable(8, -1, -1, -1),
                new MappingOnTable(-1, 5, 6, -1),
                new MappingOnTable(7, -1, 5, 4),
                //Middle
                new MappingOnTable(-1, 4, -1, -1),
                new MappingOnTable(6, -1, 4, -1),
                new MappingOnTable(-1, -1, -1, 3),
                new MappingOnTable(5, 3, 3, -1),
                new MappingOnTable(-1, -1, -1, -1),
                new MappingOnTable(4, -1, -1, 2),
                new MappingOnTable(3, 2, 2, -1),
                new MappingOnTable(-1, -1, -1, -1),
                new MappingOnTable(2, -1, 1, -1),
                new MappingOnTable(-1, -1, -1, -1),
                new MappingOnTable(-1, 1, -1, 1),
                new MappingOnTable(1, -1, 0, -1),
                new MappingOnTable(-1, -1, -1, -1),
                new MappingOnTable(0, 0, -1, 0),
            };

            return listMapping;
        }

        public static IList<MappingOnTable> BuildTableGraphLeast()
        {
            var listMapping = new List<MappingOnTable>
            {
                new MappingOnTable(0, 0, 0, 0),
                new MappingOnTable(-1, -1, 1, 1),
                new MappingOnTable(1, -1, -1, -1),
                new MappingOnTable(-1, 1, 2, -1),
                new MappingOnTable(-1, -1, -1, 2),
                new MappingOnTable(2, -1, -1, -1),
                new MappingOnTable(-1, 2, 3, 3),
                new MappingOnTable(3, -1, -1, -1),
                new MappingOnTable(-1, 3, 4, 4),
                new MappingOnTable(4, -1, -1, -1),
                new MappingOnTable(-1, -1, 5, 5),
                new MappingOnTable(5, 4, -1, -1),
                new MappingOnTable(-1, -1, 6, 6),
                //Middle
                new MappingOnTable(6, 5, -1, 7),
                new MappingOnTable(-1, -1, 7, -1),
                new MappingOnTable(7, 6, -1, -1),
                new MappingOnTable(8, -1, 8, 8),
                new MappingOnTable(9, 7, -1, 9),
                new MappingOnTable(10, -1, 9, -1),
                new MappingOnTable(11, 8, -1, -1),
                new MappingOnTable(3, -1, -1, -1),
                new MappingOnTable(12, -1, 10, 10),
                new MappingOnTable(13, 10, 11, 11),
                new MappingOnTable(14, 11, -1, -1),
                new MappingOnTable(15, -1, 12, 12),
                new MappingOnTable(16, 15, 13, 13),
                new MappingOnTable(21, 19, 19, 16),
            };

            return listMapping;
        }

        public static string ConvertTitleUser(ulong titleid)
        {
            if (titleid == 10)
            {
                return "Mr";
            }
            else
            {
                return "Ms";
            }
        }

        public static string ConvertTitleUserVN(ulong titleid)
        {
            if (titleid == 10)
            {
                return "Anh";
            }
            else
            {
                return "Chị";
            }
        }

        public static string BuildLinkUserAnswer(string seftLink, ulong useranswerid)
        {
            return seftLink + "Home/Result?useranswerid=" + useranswerid;
        }

        public static string BuildResultLink(string seftLink)
        {
            return $"{seftLink}Assets/ResultImage/";
        }

        public static bool isTopSkills(ulong partnerId)
        {
            return partnerId == TopSkillId;
        }
    }
}
