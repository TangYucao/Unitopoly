#define DEVELOP
#define DEMO
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
static class Globals
{
#if DEMO
    public static float DEVELOP_TIME = 0.3f;
#else
    public static float DEVELOP_TIME = 0.1f;
#endif
}
public class Gameplay : MonoBehaviour
{
    public static Gameplay instance;

    private List<Player> players;
    private HashSet<string> module_used;


    [SerializeField] public GameObject playerPrefab;
    [SerializeField] public GameObject carPrefab;


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
        int index = random.Next(1, 7);
        string car_module_name = "Car" + index.ToString();
        while (module_used.Contains(car_module_name))
        {
            index = random.Next(1, 7);
            car_module_name = "Car" + index.ToString();
        }
        module_used.Add(car_module_name);
        Debug.Log(car_module_name);
        GameObject module = carPrefab.transform.Find(car_module_name).gameObject;
        GameObject current_module = ((GameObject)(Instantiate(module, new Vector3(0, 0, 0), module.transform.rotation)));
        Player newPlayer = ((GameObject)(Instantiate(playerPrefab,
         new Vector3(0, current_module.GetComponent<Module>().yOffsetToTheGround + 0.3f, 0) + PassGo.instance.transform.position + placementOffsetVector,
         current_module.transform.rotation))).GetComponent<Player>();
        newPlayer.SetPlayerName(playerName);
        newPlayer.SetIsAI(ai);
        newPlayer.current_module = current_module;
        current_module.transform.parent = newPlayer.transform;
        current_module.transform.position = newPlayer.transform.position;
        newPlayer.transform.rotation = current_module.transform.rotation;
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
                if (player.IsWinner())
                {
                    yield return GameOver.instance.DisplayAlert(player.GetPlayerName() + " win.", Color.red);
                }
                bool doubles = true;
                int doubleRolls = 0;
                yield return TurnActions.instance.DisplayPlayerName(player.GetPlayerName());
                if (i > 0 || players.IndexOf(player) > 0) yield return player.currentSpace.LerpCameraViewToThisLocationWhenPass();
                if (player.remaining_stays-- > 0)
                {
                    yield return MessageAlert.instance.DisplayAlert(player.GetPlayerName() + " is blocked for " + (player.remaining_stays + 1) + " round.", Color.red);
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
#if DEVELOP
                                if (chosenAction == TurnActions.UserAction.BUILD)
                                {
                                    if (player.bombs > 0)
                                    {
                                        player.bombs--;
                                        yield return BombUI.instance.Bomb(3);
                                    }
                                    else
                                    {
                                        yield return MessageAlert.instance.DisplayAlert(player.GetPlayerName() + " doesn't have any bomb. ", Color.yellow);
                                    }
                                }
                                else if (chosenAction == TurnActions.UserAction.TRADE)
                                {
                                    if (player.thieves > 0)
                                    {
                                        player.thieves--;
                                        yield return ThiefUI.instance.Steal(players.IndexOf(player));

                                    }
                                    else
                                    {
                                        yield return MessageAlert.instance.DisplayAlert(player.GetPlayerName() + " doesn't have any thieves. ", Color.yellow);
                                    }
                                }
                                else if (chosenAction == TurnActions.UserAction.MORTGAGE)
                                {
                                    yield return SwitchCarUI.instance.SwitchCar(players.IndexOf(player));
                                    player.PrintPlayerInfo();
                                }
#else
                                Debug.LogError("Not implemented >:(");
                                yield return new WaitForSeconds(2);
#endif
                            }
                        }
                    }

                    // Roll dies.  
                    yield return DieRoller.instance.RollDie(player.dice_number);
                    int[] dieRollResults = DieRoller.instance.GetDieRollResults();
                    // If roll a double, continue rolling
                    if (dieRollResults.Length != dieRollResults.Distinct().Count() && dieRollResults.Distinct().Count() == 1)
                    {
                        doubleRolls++;
                        // Too many doubles 
                        if (doubleRolls >= 3)
                        {
                            yield return player.remaining_stays = 3;
                            break;
                        }
                    }
                    else
                    {
                        doubles = false;
                    }
#if DEVELOP && DEMO
                    if (i == 0 || i == 1)
                        yield return player.MoveSpaces(3);
                    else
                        yield return player.MoveSpaces(4);

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
    public List<Player> GetPlayers()
    {
        return players;
    }
}
