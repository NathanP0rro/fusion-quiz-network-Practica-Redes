using System.Collections.Generic;
using NUnit.Framework;
using RoomLogic;

public class ResultsLogicTests
{
    [Test]
    public void BuildRanking_Null_ReturnsEmptyList()
    {
        Assert.AreEqual(0, ResultsLogic.BuildRanking(null).Count);
    }

    [Test]
    public void BuildRanking_NoPlayers_ReturnsEmptyList()
    {
        Assert.AreEqual(0, ResultsLogic.BuildRanking(new List<PlayerResult>()).Count);
    }

    [Test]
    public void BuildRanking_SortsByScoreDescending()
    {
        var players = new List<PlayerResult>
        {
            new PlayerResult("oli", 734),
            new PlayerResult("nathan", 1688),
            new PlayerResult("lol", 185),
        };

        var ranking = ResultsLogic.BuildRanking(players);

        Assert.AreEqual("nathan", ranking[0].Name);
        Assert.AreEqual("oli", ranking[1].Name);
        Assert.AreEqual("lol", ranking[2].Name);
        Assert.AreEqual(1, ranking[0].Rank);
        Assert.AreEqual(2, ranking[1].Rank);
        Assert.AreEqual(3, ranking[2].Rank);
    }

    [Test]
    public void BuildRanking_IncludesPlayersWithZeroScore()
    {
        var players = new List<PlayerResult>
        {
            new PlayerResult("a", 0),
            new PlayerResult("b", 10),
        };

        var ranking = ResultsLogic.BuildRanking(players);

        Assert.AreEqual(2, ranking.Count);
        Assert.AreEqual("a", ranking[1].Name);
        Assert.AreEqual(0, ranking[1].Score);
        Assert.AreEqual(2, ranking[1].Rank);
    }

    [Test]
    public void BuildRanking_TiesShareRank()
    {
        var players = new List<PlayerResult>
        {
            new PlayerResult("a", 100),
            new PlayerResult("b", 50),
            new PlayerResult("c", 50),
            new PlayerResult("d", 10),
        };

        var ranking = ResultsLogic.BuildRanking(players);

        Assert.AreEqual(1, ranking[0].Rank);
        Assert.AreEqual(2, ranking[1].Rank);
        Assert.AreEqual(2, ranking[2].Rank);
        Assert.AreEqual(4, ranking[3].Rank);
    }

    [Test]
    public void BuildRanking_TiedPlayersAreOrderedByName()
    {
        var players = new List<PlayerResult>
        {
            new PlayerResult("Zoe", 50),
            new PlayerResult("ana", 50),
        };

        var ranking = ResultsLogic.BuildRanking(players);

        Assert.AreEqual("ana", ranking[0].Name);
        Assert.AreEqual("Zoe", ranking[1].Name);
    }

    [Test]
    public void FormatRanking_TwoPlayers_OneLineEach()
    {
        var players = new List<PlayerResult>
        {
            new PlayerResult("oli", 734),
            new PlayerResult("nathan", 1688),
        };

        Assert.AreEqual("1. nathan - 1688\n2. oli - 734", ResultsLogic.FormatRanking(players));
    }

    [Test]
    public void FormatRanking_NoPlayers_ReturnsEmptyString()
    {
        Assert.AreEqual(string.Empty, ResultsLogic.FormatRanking(new List<PlayerResult>()));
    }
}
