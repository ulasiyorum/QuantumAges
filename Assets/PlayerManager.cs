using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Team
{

}

public class PlayerManager : MonoBehaviour
{
    private float balance;
    private float base_health;
    private float spawner_health;
    private string playerName;
    private Team team;  

    public float Balance
    {
        get { return balance; }
        set { balance = value; }
    }   

    public float BaseHealth
    {
        get { return base_health; }
        set { base_health = value;
            CheckGameOver();
        }
    }   

    public float SpawnerHealth
    {
        get { return spawner_health; }
        set { spawner_health = value;
            CheckGameOver();
        }
    }

    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }

    public Team Team
    {
        get { return team; }
        set { team = value; }
    }


    private void Start()
    {
        {


        }
    }

    private void CheckGameOver()
    {
        if (base_health < 0 || spawner_health < 0)
        {
            // Oyun bitti, GameOver ekranýný açabilir veya baþka bir iþlem yapabilirsiniz
            GameOver();
        }
    }
    private void GameOver()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}




