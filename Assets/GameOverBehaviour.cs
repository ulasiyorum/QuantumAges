using System.Collections;
using System.Collections.Generic;
using Consts;
using UnityEngine;

public class GameOverBehaviour : MonoBehaviour
{
    private static GameOverBehaviour Instance { set; get; }
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject scoreText;
    [SerializeField] private GameObject winText;

    [SerializeField] private GameObject[] others;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public static void GameOver(UnitTeam winnerTeam, UnitTeam localTeam, int soldiersKilled)
    {
        foreach (var other in Instance.others)
        {
            other.SetActive(true);
        }        
        
        Instance.scoreText.SetActive(true);
        Instance.background.SetActive(true);
        Instance.winText.SetActive(true);
        Instance.scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = $"You killed {soldiersKilled} enemies";
        if (winnerTeam == localTeam)
        {
            Instance.winText.GetComponent<TMPro.TextMeshProUGUI>().text = "You win!";
        }
        else
        {
            Instance.winText.GetComponent<TMPro.TextMeshProUGUI>().text = "You lose!";
        }
    }
}
