using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomLogic
{
    public static class RoomNameLogic
    {
        public static string Normalize(string roomName)
        {
            if (string.IsNullOrWhiteSpace(roomName))
                return string.Empty;

            return roomName.Trim().ToLowerInvariant();
        }
    }
}