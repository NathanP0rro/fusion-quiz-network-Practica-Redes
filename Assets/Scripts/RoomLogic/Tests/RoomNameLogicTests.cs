using NUnit.Framework;
using RoomLogic;

public class RoomNameLogicTests
{
    [TestCase("Sala1", "sala1")]
    [TestCase("SaLa1", "sala1")]
    [TestCase("  sala1 ", "sala1")]
    [TestCase("sala1", "sala1")]
    [TestCase("", "")]
    [TestCase("   ", "")]
    [TestCase(null, "")]
    public void Normalize_ReturnsExpected(string input, string expected)
    {
        Assert.AreEqual(expected, RoomNameLogic.Normalize(input));
    }

    [Test]
    public void DifferentCasing_ProducesSameRoom()
    {
        Assert.AreEqual(RoomNameLogic.Normalize("Sala1"), RoomNameLogic.Normalize("SALA1"));
    }
}
