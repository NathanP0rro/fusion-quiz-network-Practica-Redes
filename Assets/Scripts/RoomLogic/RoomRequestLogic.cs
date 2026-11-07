namespace RoomLogic
{
    public enum RoomRequestResult
    {
        Ok = 0,
        MissingRoomName = 1,
        RoomAlreadyExists = 2,
        RoomDoesNotExist = 3,
    }

    public static class RoomRequestLogic
    {
        /// Antes de conectar: Crear y Unirse necesitan un nombre de sala.
        public static RoomRequestResult ValidateName(string normalizedRoomName)
        {
            return string.IsNullOrEmpty(normalizedRoomName)
                ? RoomRequestResult.MissingRoomName
                : RoomRequestResult.Ok;
        }

        /// Después de conectar: decide si el resultado es válido según el botón pulsado.
        public static RoomRequestResult ValidateOutcome(bool wantsToCreate, bool roomAlreadyExisted)
        {
            if (wantsToCreate && roomAlreadyExisted)
                return RoomRequestResult.RoomAlreadyExists;

            if (!wantsToCreate && !roomAlreadyExisted)
                return RoomRequestResult.RoomDoesNotExist;

            return RoomRequestResult.Ok;
        }

        /// Deduce si la sala ya existía antes de entrar nosotros.
        public static bool DidRoomAlreadyExist(bool isSharedModeMasterClient, int activePlayerCount)
        {
            return !isSharedModeMasterClient || activePlayerCount > 1;
        }
    }
}
