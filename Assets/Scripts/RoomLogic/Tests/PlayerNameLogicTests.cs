using NUnit.Framework;
using RoomLogic;

public class PlayerNameLogicTests
{
    [TestCase("Nathan", "nathan")]
    [TestCase("  NATHAN ", "nathan")]
    [TestCase("", "")]
    [TestCase("   ", "")]
    [TestCase(null, "")]
    public void NormalizeForComparison_ReturnsExpected(string input, string expected)
    {
        Assert.AreEqual(expected, PlayerNameLogic.NormalizeForComparison(input));
    }

    [Test]
    public void IsNameTaken_ExactSameName_ReturnsTrue()
    {
        var existing = new[] { "oli", "nathan" };
        Assert.IsTrue(PlayerNameLogic.IsNameTaken("nathan", existing));
    }

    [Test]
    public void IsNameTaken_DifferentCasing_ReturnsTrue()
    {
        var existing = new[] { "oli", "Nathan" };
        Assert.IsTrue(PlayerNameLogic.IsNameTaken("NATHAN", existing));
    }

    [Test]
    public void IsNameTaken_ExtraSpaces_ReturnsTrue()
    {
        var existing = new[] { "nathan" };
        Assert.IsTrue(PlayerNameLogic.IsNameTaken("  nathan ", existing));
    }

    [Test]
    public void IsNameTaken_NewName_ReturnsFalse()
    {
        var existing = new[] { "oli", "nathan" };
        Assert.IsFalse(PlayerNameLogic.IsNameTaken("lol", existing));
    }

    [Test]
    public void IsNameTaken_EmptyList_ReturnsFalse()
    {
        Assert.IsFalse(PlayerNameLogic.IsNameTaken("nathan", new string[0]));
    }

    [Test]
    public void IsNameTaken_NullList_ReturnsFalse()
    {
        Assert.IsFalse(PlayerNameLogic.IsNameTaken("nathan", null));
    }

    [Test]
    public void IsNameTaken_EmptyCandidate_ReturnsFalse()
    {
        var existing = new[] { "", "nathan" };
        Assert.IsFalse(PlayerNameLogic.IsNameTaken("", existing));
    }
}
