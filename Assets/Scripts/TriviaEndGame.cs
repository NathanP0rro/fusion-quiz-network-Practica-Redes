using RoomLogic;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles displaying the winners of the game
/// </summary>
public class TriviaEndGame : MonoBehaviour
{
    [Tooltip("Displays the top three winners.")]
    public TriviaEndGamePlayer[] winnerDisplays;

    /// <summary>
    /// Shows the list of winners.
    /// </summary>
    /// <param name="winners">The list of winning players</param>
    /// [Tooltip("Text that displays the results of all the players.")]
    public TextMeshProUGUI allResultsText;
    public void Show(List<TriviaPlayer> winners)
    {
        // Iterates through the winner displayers and shows them if a winner is available.
        for (int i =0, len = winnerDisplays.Length; i < len;  i++)
        {
            if (i < winners.Count)
            {
                winnerDisplays[i].Show(winners[i]);
            }
            else
            {
                winnerDisplays[i].Hide();
            }
        }

        gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the end game display.
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void ShowAllResults(List<TriviaPlayer> allPlayers)
    {
        if (allResultsText == null || allPlayers == null)
            return;

        var results = allPlayers
            .Where(p => p != null)
            .Select(p => new PlayerResult(p.PlayerName.Value, p.Score));

        allResultsText.text = ResultsLogic.FormatRanking(results);
    }

}