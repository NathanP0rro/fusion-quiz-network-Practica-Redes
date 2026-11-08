using Fusion;
using RoomLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class FusionConnector : MonoBehaviour
{
    public string LocalPlayerName { get; set; }

    public string LocalRoomName { get; set; }

    [SerializeField, Tooltip("The network runner prefab that will be instantiated when looking starting the game.")]
    private NetworkRunner _networkRunnerPrefab;

    [Tooltip("The canvas group that handles interactivity for the game.")]
    public CanvasGroup canvasGroup;

    [Tooltip("The GameObject that contains the main menu.")]
    public GameObject mainMenuObject;

    [Tooltip("The Game Object that handles the game itself")]
    public GameObject mainGameObject;

    [Tooltip("GameObject that appears if there is a network error when trying to join a room.")]
    public GameObject errorMessageObject;

    [Tooltip("The GameObject that displays the button to start the game.")]
    public GameObject showGameButton;

    [Tooltip("Text object that displays the room name.")]
    public TextMeshProUGUI roomName;

    [Tooltip("Prefab for the trivia game itself.")]
    public NetworkObject triviaGamePrefab;

    public Transform playerContainer;

    [Tooltip("The message shown before starting the game.")]
    public TextMeshProUGUI preGameMessage;

    public static FusionConnector Instance { get; private set; }

    private string _currentRoomName = string.Empty;
    private string _pregameKey;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDestroy()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        Instance = null;
    }

    private void OnLocaleChanged(UnityEngine.Localization.Locale locale)
    {
        RefreshRoomName();
        if (!string.IsNullOrEmpty(_pregameKey))
        {
            preGameMessage.text = UiText.Get(_pregameKey);
        }
    }

    private void SetRoomName(string name)
    {
        _currentRoomName = name;
        RefreshRoomName();
    }

    private void RefreshRoomName()
    {
        roomName.text = string.IsNullOrEmpty(_currentRoomName)
            ? string.Empty
            : UiText.Get("Room_Label", _currentRoomName);
    }

    public async void StartGame(bool createRoom)
    {
        canvasGroup.interactable = false;

        string normalizedRoomName = RoomNameLogic.Normalize(LocalRoomName);

        // Crear y Unirse necesitan siempre un nombre de sala.
        if (RoomRequestLogic.ValidateName(normalizedRoomName) != RoomRequestResult.Ok)
        {
            ShowMenuError(UiText.Get("Error_RoomNameMissing"));
            canvasGroup.interactable = true;
            return;
        }

        StartGameArgs startGameArgs = new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = normalizedRoomName,
            PlayerCount = 20,
        };

        NetworkRunner newRunner = Instantiate(_networkRunnerPrefab);

        StartGameResult result = await newRunner.StartGame(startGameArgs);

        if (result.Ok)
        {
 
            bool roomAlreadyExisted = RoomRequestLogic.DidRoomAlreadyExist(
                newRunner.IsSharedModeMasterClient, newRunner.ActivePlayers.Count());

            RoomRequestResult outcome = RoomRequestLogic.ValidateOutcome(createRoom, roomAlreadyExisted);

            if (outcome == RoomRequestResult.Ok)
            {
                SetRoomName(newRunner.SessionInfo.Name);
                GoToGame();
            }
            else
            {
                await newRunner.Shutdown(true, ShutdownReason.Ok);

                SetRoomName(string.Empty);
                GoToMainMenu();

                ShowMenuError(UiText.Get(outcome == RoomRequestResult.RoomAlreadyExists
                    ? "Error_RoomExists"
                    : "Error_RoomNotFound"));
            }
        }
        else
        {
            SetRoomName(string.Empty);
            GoToMainMenu();
            ShowMenuError(result.ErrorMessage);
            Debug.LogError(result.ErrorMessage);
        }

        canvasGroup.interactable = true;
    }

    private void ShowMenuError(string message)
    {
        errorMessageObject.SetActive(true);
        TextMeshProUGUI gui = errorMessageObject.GetComponentInChildren<TextMeshProUGUI>();
        if (gui)
            gui.text = message;
    }

    public void GoToMainMenu()
    {
        mainMenuObject.SetActive(true);
        mainGameObject.SetActive(false);
    }

    public async void LeaveWithError(NetworkRunner runner, string message)
    {
        await runner.Shutdown(true, ShutdownReason.Ok);

        SetRoomName(string.Empty);
        GoToMainMenu();

        errorMessageObject.SetActive(true);
        TextMeshProUGUI gui = errorMessageObject.GetComponentInChildren<TextMeshProUGUI>();
        if (gui)
            gui.text = message;
    }

    public void GoToGame()
    {
        mainMenuObject.SetActive(false);
        mainGameObject.SetActive(true);
        
    }

    internal void OnPlayerJoin(NetworkRunner runner)
    {
        // Only set pregame messages if the game hasn't started.
        if (TriviaManager.TriviaManagerPresent)
        {
            return;
        }

        SetPregameKey(runner.IsSharedModeMasterClient ? "Pregame_Ready" : "Pregame_Waiting");
    }
    public void SetPregameKey(string key)
    {
        _pregameKey = key;
        preGameMessage.text = UiText.Get(key);
    }
    public void SetPregameMessage(string message)
    {
        _pregameKey = null;
        preGameMessage.text = message;
    }
    public void StartTriviaGame()
    {
        NetworkRunner runner = null;
        // If no runner has been assigned, we cannot start the game
        if (NetworkRunner.Instances.Count > 0)
        {
            runner = NetworkRunner.Instances[0];
        }

        if (runner == null)
        {
            Debug.Log("No runner found.");
            return;
        }



        // If no trivia manager has been made and we are the master mode client.
        // Redundant but being safe.
        if (runner.IsSharedModeMasterClient && !TriviaManager.TriviaManagerPresent)
        {
            runner.Spawn(triviaGamePrefab);
            showGameButton.SetActive(false);
        }
    }
}
