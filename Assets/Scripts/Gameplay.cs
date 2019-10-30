#define DEVELOP
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
static class Globals
{
    public static float DEVELOP_TIME = 0.1f;
}
public class Gameplay : MonoBehaviour
{
    public static Gameplay instance;

    private List<Player> players;
    private HashSet<string> module_used;


    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private BalanceTracker[] balanceTrackers;

    void Awake()
    {
        instance = this;

        players = new List<Player>();
        module_used = new HashSet<string>();
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
        System.Random random = new System.Random();
        int index = random.Next(1, 9);
        string car_module_name = "Car" + index.ToString();
        while (module_used.Contains(car_module_name))
        {
            index = random.Next(1, 8);
            car_module_name = "Car" + index.ToString();
        }
        module_used.Add(car_module_name);
        Debug.Log(car_module_name);
        GameObject module = playerPrefab.transform.Find(car_module_name).gameObject;
        Player newPlayer = ((GameObject)(Instantiate(module, new Vector3(0, module.GetComponent<Player>().yOffsetToTheGround, 0) + PassGo.instance.transform.position + placementOffsetVector, module.transform.rotation))).GetComponent<Player>();

        newPlayer.SetPlayerName(playerName);
        newPlayer.SetIsAI(ai);

        players.Add(newPlayer);
        newPlayer.Initialize();

        // Give this player a balance tracker.  
        BalanceTracker balanceTracker = balanceTrackers[players.Count - 1];
        balanceTracker.gameObject.SetActive(true);
        newPlayer.SetBalanceTracker(balanceTracker);
    }
    private void HandlePlayersCollision()
    {
        List<Vector3> player_positions = new List<Vector3>();
        foreach (Player player in players)
        {
            player_positions.Add(player.transform.position);
        }
        if (player_positions.Distinct().Count() == player_positions.Count)
            return;
        int idx = 0;
        foreach (Player player in players)
        {
            idx++;
            if (!player.rotated)
            {
                switch (idx)
                {
                    case 1:
                        player.transform.position = new Vector3(0, 0, 0);
                        break;
                    case 2:
                        player.transform.position = new Vector3(.25f, 0, 0);

                        break;
                    case 3:
                        player.transform.position = new Vector3(0.5f, 0, 0);
                        break;
                    case 4:
                        player.transform.position = new Vector3(-0.25f, 0, 0);
                        break;
                }
            }
            else
            {
                Debug.Log("[107]");
                switch (idx)
                {
                    case 1:
                        player.transform.position = new Vector3(0, 0, 0);
                        break;
                    case 2:
                        player.transform.position = new Vector3(0, 0.25f, 0);

                        break;
                    case 3:
                        player.transform.position = new Vector3(0, 0.5f, 0);
                        break;
                    case 4:
                        player.transform.position = new Vector3(0, -0.25f, 0);
                        break;
                }
            }
            player.transform.position += player.currentSpace.transform.position + player.costumeOffset;

        }
    }
    public void StartGame()
    {
        StartCoroutine(PlayGame());
    }

    private IEnumerator PlayGame()
    {
#if DEVELOP
        yield return CameraController.instance.LerpToViewBoardTarget(Globals.DEVELOP_TIME);
#else
        yield return CameraController.instance.LerpToViewBoardTarget(5f);
#endif
        // Simulate taking turns.  
        for (int i = 0; i < 35; i++)
        {
            foreach (Player player in players)
            {
                bool doubles = true;
                int doubleRolls = 0;
                yield return TurnActions.instance.DisplayPlayerName(player.GetPlayerName());
                if (i > 0 || players.IndexOf(player) > 0) yield return player.currentSpace.LerpCameraViewToThisLocationWhenPass();
                if(i > 0)
                if(player.remaining_stays-->0)
                {
                    Debug.Log(player.GetPlayerName()+" is blocked for 1 round.");
                    continue;
                }
                while (doubles)
                {
                    if (!player.IsAI())
                    {
                        TurnActions.UserAction chosenAction = TurnActions.UserAction.UNDECIDED;
                        //TODO MARK: to implement other buttons other than ROLL button
                        while (chosenAction != TurnActions.UserAction.ROLL)
                        {
                            yield return TurnActions.instance.GetUserInput(true);
                            chosenAction = TurnActions.instance.GetChosenAction();

                            if (chosenAction != TurnActions.UserAction.ROLL)
                            {
                                Debug.LogError("Not implemented >:(");
                                yield return new WaitForSeconds(2);
                            }
                        }
                    }

                    // Roll dies.  
                    yield return DieRoller.instance.RollDie();
                    int[] dieRollResults = DieRoller.instance.GetDieRollResults();

                    // If roll a double, continue rolling
                    if (dieRollResults.Length != dieRollResults.Distinct().Count())
                    {
                        doubleRolls++;
                        // Too many doubles 
                        if (doubleRolls >= 3)
                        {
                            yield return player.JumpToSpace(InJail.instance, 1f);
                            break;
                        }
                    }
                    else
                    {
                        doubles = false;
                    }
#if DEVELOP
                    yield return player.MoveSpaces(3);
#else
                    yield return player.MoveSpaces(dieRollResults.Sum());
#endif
                    HandlePlayersCollision();
                }

                if (!player.IsAI())
                {
                    yield return TurnActions.instance.GetUserInput(false);
                }

            }
        }
    }
}
