using System.Collections;
using System.Collections.Generic;
using Consts;
using Managers.Abstract;
using Photon.Pun;
using UnityEngine;

public class GameOverBehaviour : MonoBehaviourPun
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

    [PunRPC]
    public static void GameOver(UnitTeam winnerTeam, UnitTeam localTeam, int soldiersKilled)
    {
        var otherTeam = localTeam == UnitTeam.Green ? UnitTeam.Red : UnitTeam.Green;
        Instance.photonView.RPC("GameOver", RpcTarget.Others, winnerTeam, otherTeam,
            otherTeam == UnitTeam.Green ? PlayerManager.red_manager.killCount : PlayerManager.green_manager.killCount, 
            soldiersKilled);
        
        foreach (var other in Instance.others)
        {
            other.SetActive(true);
        }        
        
        Instance.scoreText.SetActive(true);
        Instance.background.SetActive(true);
        Instance.winText.SetActive(true);
        Instance.scoreText.GetComponent<TMPro.TextMeshProUGUI>().text = $"You killed {soldiersKilled} enemies";
        Instance.winText.GetComponent<TMPro.TextMeshProUGUI>().text = winnerTeam == localTeam ? "You win!" : "You lose!";
    }
}
