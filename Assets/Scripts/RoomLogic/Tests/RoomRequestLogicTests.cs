using NUnit.Framework;
using RoomLogic;

public class RoomRequestLogicTests
{
    [Test]
    public void ValidateName_Empty_ReturnsMissingRoomName()
    {
        Assert.AreEqual(RoomRequestResult.MissingRoomName, RoomRequestLogic.ValidateName(""));
    }

    [Test]
    public void ValidateName_Null_ReturnsMissingRoomName()
    {
        Assert.AreEqual(RoomRequestResult.MissingRoomName, RoomRequestLogic.ValidateName(null));
    }

    [Test]
    public void ValidateName_WithName_ReturnsOk()
    {
        Assert.AreEqual(RoomRequestResult.Ok, RoomRequestLogic.ValidateName("sala1"));
    }

    [Test]
    public void Create_WhenRoomDidNotExist_ReturnsOk()
    {
        Assert.AreEqual(RoomRequestResult.Ok, RoomRequestLogic.ValidateOutcome(true, false));
    }

    [Test]
    public void Create_WhenRoomAlreadyExisted_ReturnsRoomAlreadyExists()
    {
        Assert.AreEqual(RoomRequestResult.RoomAlreadyExists, RoomRequestLogic.ValidateOutcome(true, true));
    }

    [Test]
    public void Join_WhenRoomAlreadyExisted_ReturnsOk()
    {
        Assert.AreEqual(RoomRequestResult.Ok, RoomRequestLogic.ValidateOutcome(false, true));
    }

    [Test]
    public void Join_WhenRoomDidNotExist_ReturnsRoomDoesNotExist()
    {
        Assert.AreEqual(RoomRequestResult.RoomDoesNotExist, RoomRequestLogic.ValidateOutcome(false, false));
    }

    [Test]
    public void DidRoomAlreadyExist_MasterAndAlone_ReturnsFalse()
    {
        Assert.IsFalse(RoomRequestLogic.DidRoomAlreadyExist(true, 1));
    }

    [Test]
    public void DidRoomAlreadyExist_NotMaster_ReturnsTrue()
    {
        Assert.IsTrue(RoomRequestLogic.DidRoomAlreadyExist(false, 1));
    }

    [Test]
    public void DidRoomAlreadyExist_MasterButOthersPresent_ReturnsTrue()
    {
        Assert.IsTrue(RoomRequestLogic.DidRoomAlreadyExist(true, 2));
    }
}