﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public int amountOfPlayers = 2;

    public ExplosionHandler explosionHandler;

    public GameObject playerObject;
    public Transform[] spawnPoints;
    private List<Character> _players;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < amountOfPlayers; i++)
        {
            GameObject spawnedPlayer = Instantiate(playerObject, spawnPoints[i].position, Quaternion.identity);
            Character character = spawnedPlayer.GetComponent<Character>();
            character.SetID(i + 1);
           // _players.Add(character);
        }
    }


    
}