using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomLogic
{
    public static class PlayerNameLogic
    {
        public static string NormalizeForComparison(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            return name.Trim().ToLowerInvariant();
        }
        public static bool IsNameTaken(string candidate, IEnumerable<string> existingNames)
        {
            string wanted = NormalizeForComparison(candidate);

            if (wanted.Length == 0 || existingNames == null)
                return false;

            foreach (string existing in existingNames)
            {
                if (NormalizeForComparison(existing) == wanted)
                    return true;
            }

            return false;
        }
    }
}
