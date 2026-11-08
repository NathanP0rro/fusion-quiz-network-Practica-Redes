using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RoomLogic;

/// <summary>
/// Basic player spawn based on the main shared mode sample.
/// Before spawning the local player it checks that the chosen name is not already in use.
/// </summary>
public class PlayerSpawner : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    public GameObject PlayerPrefab;

    [Tooltip("Maximum seconds to wait for the other players to appear before checking the name.")]
    public float nameCheckTimeout = 3f;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            StartCoroutine(SpawnLocalPlayer());
        }

        FusionConnector.Instance?.OnPlayerJoin(Runner);
    }

    private IEnumerator SpawnLocalPlayer()
    {
        FusionConnector connector = GameObject.FindObjectOfType<FusionConnector>();

        string playerName = connector != null ? connector.LocalPlayerName : null;
        if (string.IsNullOrEmpty(playerName))
            playerName = UiText.Get("Default_PlayerName", Runner.LocalPlayer.PlayerId);

        // Waits until the players already in the room have appeared in this client (or until the timeout).
        float elapsed = 0f;
        while (elapsed < nameCheckTimeout &&
               TriviaPlayer.TriviaPlayerRefs.Count < Runner.ActivePlayers.Count() - 1)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (Runner == null)
            yield break;

        // Names already used by the other players in the room.
        List<string> existingNames = TriviaPlayer.TriviaPlayerRefs
            .Where(p => p != null)
            .Select(p => p.PlayerName.Value)
            .ToList();

        if (PlayerNameLogic.IsNameTaken(playerName, existingNames))
        {
            if (connector != null)
                connector.LeaveWithError(Runner, UiText.Get("Error_NameTaken"));
            yield break;
        }

        var resultingPlayer = Runner.Spawn(PlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        var testPlayer = resultingPlayer.GetComponent<TriviaPlayer>();

        testPlayer.PlayerName = playerName;

        // Assigns a random avatar
        testPlayer.ChosenAvatar = Random.Range(0, testPlayer.avatarSprites.Length);
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (TriviaPlayer.LocalPlayer != null)
            TriviaPlayer.LocalPlayer.IsMasterClient = Runner.IsSharedModeMasterClient;
    }
}
