using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FootballServices.Domain
{
    public static class PlayerIdsExtensions
    {
        public static List<int> ParseIds(this string ids)
        {
            if (string.IsNullOrEmpty(ids))
                return new List<int>();
            
            return ids.Trim(new char[] { '[', ']' })
                      .Split(',')
                      .Select(int.Parse)
                      .ToList();
        }
        public static string ParseIds(this int[] ids)
        {
            var str = string.Join(',', ids);
            return $"[{str}]";
        }
    }
}
