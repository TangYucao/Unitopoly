﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    public static Gameplay instance;

    private List<Player> players;

    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private BalanceTracker[] balanceTrackers;

    void Awake()
    {
        instance = this;
        
        players = new List<Player>();
    }

    public void RegisterNewPlayer(string playerName, bool ai)
    {
        // Decide an offset vector so they don't overlap.  
        Vector3 placementOffsetVector = Vector3.zero;
        switch (players.Count)
        {
            case 1: 
                placementOffsetVector = new Vector3(.5f, 0, 0);
                break;
            
            case 2: 
                placementOffsetVector = new Vector3(-.5f, 0, 0);
                break;
            
            case 3: 
                placementOffsetVector = new Vector3(0, 0, -.5f);
                break;
        }
        
        Player newPlayer = ((GameObject)(Instantiate(playerPrefab, PassGo.instance.transform.position + placementOffsetVector, playerPrefab.transform.rotation))).GetComponent<Player>();
        
        newPlayer.SetPlayerName(playerName);
        newPlayer.SetIsAI(ai);
        
        players.Add(newPlayer);
        
        newPlayer.Initialize();
        
        // Give this player a balance tracker.  
        BalanceTracker balanceTracker = balanceTrackers[players.Count - 1];
        balanceTracker.gameObject.SetActive(true);
        newPlayer.SetBalanceTracker(balanceTracker);
    }

    public void StartGame()
    {
        StartCoroutine(PlayGame());
    }

    private IEnumerator PlayGame()
    {
        yield return CameraController.instance.LerpToViewBoardTarget();
        
        // Simulate taking 16 turns.  
        for (int i = 0; i < 16; i++)
        {
            foreach (Player player in players)
            {
                yield return DieRoller.instance.RollDie();
                int[] dieRollResults = DieRoller.instance.GetDieRollResults();
                yield return player.MoveSpaces(dieRollResults.Sum());
            }
        }
    }
}
