using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public int m_AmountOfPlayers = 2;

    public ExplosionHandler m_ExplosionHandler;

    public GameObject m_PlayerPefab;
    private List<Character> m_Players = new List<Character>();

    public Transform m_SpawnPointBase = null;
    private List<Transform> m_SpawnPoints = new List<Transform>();

    private void OnEnable()
    {
        LevelEvents.Instance().Reset += Reset;
    }

    private void OnDisable()
    {
        LevelEvents.Instance().Reset -= Reset;
    }

    private void Awake()
    {
        SetupSpawnPoints();
        CreatePlayers();
    }

    private void Reset()
    {
        SpawnPlayers();
    }

    private void SetupSpawnPoints()
    {
        Debug.Assert(m_SpawnPointBase);

        foreach(Transform transform in m_SpawnPointBase)
            m_SpawnPoints.Add(transform); 
    }

    private void CreatePlayers()
    {
        Debug.Assert(m_PlayerPefab);

        for(int i = 0; i < m_AmountOfPlayers; i++)
        {
            GameObject spawnedPlayer = Instantiate(m_PlayerPefab, m_SpawnPoints[i].position, Quaternion.identity);
            Character character = spawnedPlayer.GetComponent<Character>();
            character.SetID(i + 1);

            m_Players.Add(character);
        }
    }

    private void SpawnPlayers()
    {
        for(int i = 0; i < m_Players.Count; i++)
        {
            Character character = m_Players[i];
            character.transform.position = m_SpawnPoints[i].position;
            character.transform.rotation = Quaternion.identity;

            // Todo: Reset the character's health
        }
    }
}
