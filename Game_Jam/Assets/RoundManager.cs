using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{

    public static int CurrentRound = 1;
    public int ZombiesAlive = 4;

    public static RoundManager instancia;

    public GameObject Enemy;

    public GameObject[] SpawnPoints;


    public Text RoundsText;

    public int BasicSpawnNumber;
    public int SpawnsPerRound = 4;


    private void Awake()
    {
        instancia = this;
        BasicSpawnNumber = SpawnsPerRound;
        ZombiesAlive = SpawnsPerRound;
        RoundsText.text = "Rounds: 1";
    }


    private void Update()
    {

        if(SpawnsPerRound > 0)
        {

            int PositionP = Random.Range(0, SpawnPoints.Length);

            Instantiate(Enemy, SpawnPoints[PositionP].transform.position, Quaternion.identity);

            SpawnsPerRound--;

        }

        if(ZombiesAlive == 0)
        {
            EndRound();
        }


    }


    void EndRound()
    {
      
        SpawnsPerRound = BasicSpawnNumber + 2;
        BasicSpawnNumber = SpawnsPerRound;
        CurrentRound++;
        RoundsText.text = "Round: " + CurrentRound;

        Player_Stats.instancia.Health = Player_Stats.instancia.InitialHealth;

        ZombiesAlive = SpawnsPerRound;
    }



}
