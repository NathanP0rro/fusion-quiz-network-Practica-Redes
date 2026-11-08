using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomLogic
{
    public struct RoundPlayerResult
    {
        public string Name;
        public int RoundScore;
        public int TotalScore;
        public RoundPlayerResult(string name, int roundScore, int totalScore)
        {
            Name = name;
            RoundScore = roundScore;
            TotalScore = totalScore;
        }
    }
    public static class RoundLogic
    {
        public static int AddToTotal(int totalSoFar, int roundScore)
        {
            return totalSoFar + Math.Max(0, roundScore);
        }

        public static int NextRound(int currentRound)
        {
            return Math.Max(1, currentRound) + 1;
        }
        public static string FormatRoundText(int round)
        {
            return "Round: " + Math.Max(1, round);
        }
        public static string FormatTotalRanking(IEnumerable<RoundPlayerResult> players)
        {
            if (players == null)
                return string.Empty;

            var sorted = players
                .OrderByDescending(p => p.TotalScore)
                .ThenBy(p => p.Name ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                .ToList();

            var sb = new StringBuilder();
            int rank = 0;

            for (int i = 0; i < sorted.Count; i++)
            {
                if (i == 0 || sorted[i].TotalScore != sorted[i - 1].TotalScore)
                    rank = i + 1;

                if (i > 0)
                    sb.Append('\n');

                sb.Append(rank).Append(". ")
                  .Append(sorted[i].Name ?? string.Empty).Append(" - ")
                  .Append(sorted[i].TotalScore)
                  .Append(" (round: ").Append(sorted[i].RoundScore).Append(")");
            }

            return sb.ToString();
        }
    }
}