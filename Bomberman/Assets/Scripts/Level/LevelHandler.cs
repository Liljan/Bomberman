using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public int m_AmountOfPlayers = 2;

    public ExplosionHandler m_ExplosionHandler;

    public GameObject m_PlayerPefab;
    private List<Character> m_Players;

    public Transform m_SpawnPointBase = null;
    private List<Transform> m_SpawnPoints = new List<Transform>();

    private void Awake()
    {
        SetupSpawnPoints();
        SpawnPlayers();
    }

    private void SetupSpawnPoints()
    {
        Debug.Assert(m_SpawnPointBase);

        foreach (Transform transform in m_SpawnPointBase)
            m_SpawnPoints.Add(transform); 
    }

    private void SpawnPlayers()
    {
        Debug.Assert(m_PlayerPefab);

        for (int i = 0; i < m_AmountOfPlayers; i++)
        {
            GameObject spawnedPlayer = Instantiate(m_PlayerPefab, m_SpawnPoints[i].position, Quaternion.identity);
            Character character = spawnedPlayer.GetComponent<Character>();
            character.SetID(i + 1);
        }
    }

    // Use this for initialization
    void Start()
    {

    }


    
}
