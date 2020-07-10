using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public int m_AmountOfPlayers = 2;

    public ExplosionHandler m_ExplosionHandler;

    public GameObject m_PlayerPefab;
    public Transform[] m_SpawnPoints;
    private List<Character> m_Players;

    // Use this for initialization
    void Start()
    {
        for(int i = 0; i < m_AmountOfPlayers; i++)
        {
            GameObject spawnedPlayer = Instantiate(m_PlayerPefab, m_SpawnPoints[i].position, Quaternion.identity);
            Character character = spawnedPlayer.GetComponent<Character>();
            character.SetID(i + 1);
        }
    }


    
}
