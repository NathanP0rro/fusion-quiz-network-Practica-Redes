using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomLogic
{
    public struct PlayerResult
    {
        public string Name;
        public int Score;

        public PlayerResult(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }

    public struct RankedResult
    {
        public int Rank;
        public string Name;
        public int Score;
    }

    public static class ResultsLogic
    {
        public static List<RankedResult> BuildRanking(IEnumerable<PlayerResult> players)
        {
            var result = new List<RankedResult>();
            if (players == null)
                return result;

            var sorted = players
                .OrderByDescending(p => p.Score)
                .ThenBy(p => p.Name ?? string.Empty, System.StringComparer.OrdinalIgnoreCase)
                .ToList();

            int rank = 0;
            for (int i = 0; i < sorted.Count; i++)
            {
                if (i == 0 || sorted[i].Score != sorted[i - 1].Score)
                    rank = i + 1;

                result.Add(new RankedResult
                {
                    Rank = rank,
                    Name = sorted[i].Name ?? string.Empty,
                    Score = sorted[i].Score
                });
            }

            return result;
        }
        public static string FormatRanking(IEnumerable<PlayerResult> players)
        {
            var ranking = BuildRanking(players);
            var sb = new StringBuilder();

            for (int i = 0; i < ranking.Count; i++)
            {
                if (i > 0)
                    sb.Append('\n');

                sb.Append(ranking[i].Rank).Append(". ")
                  .Append(ranking[i].Name).Append(" - ")
                  .Append(ranking[i].Score);
            }

            return sb.ToString();
        }
    }
}
