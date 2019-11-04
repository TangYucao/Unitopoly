// This is for selection UI. Data should be retrieved from database or static data like txt or csv.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombUI : MonoBehaviour
{
    public static BombUI instance;
    void Awake()
    {
        instance = this;
    }

    [SerializeField] private Text player1;
    [SerializeField] private Text player2;
    [SerializeField] private Text player3;
    [SerializeField] private Text player4;

    [HideInInspector] public bool selection_made;
    [HideInInspector] public int result_selection;

    public IEnumerator Bomb(int round)
    {
        selection_made = false;
        int cnt = 0;
        foreach (var player in Gameplay.instance.GetPlayers())
        {
            switch (cnt++)
            {
                case 0:
                    player1.text = player.GetPlayerName();
                    break;
                case 1:
                    player2.text = player.GetPlayerName();
                    break;
                case 2:
                    player3.text = player.GetPlayerName();
                    break;
                case 3:
                    player4.text = player.GetPlayerName();
                    break;
            }
        }

        transform.GetChild(0).gameObject.SetActive(true);
        // Magic logic code.
        bool valid = false;
        while (!selection_made || !valid)
        {
            if (result_selection > Gameplay.instance.GetPlayers().Count)
            {
                selection_made = false;
            }
            else
            {
                valid = true;
            }
            yield return null;
        }
        transform.GetChild(0).gameObject.SetActive(false);
        int index = result_selection - 1;
        //If shield detected, bomb is no use.
        if (!Gameplay.instance.GetPlayers()[index].shield)
        {
            yield return MessageAlert.instance.DisplayAlert(Gameplay.instance.GetPlayers()[index].GetPlayerName() + " is bombed!", Color.green);
            Gameplay.instance.GetPlayers()[index].remaining_stays = round;
        }
        else
        {
            yield return MessageAlert.instance.DisplayAlert(Gameplay.instance.GetPlayers()[index].GetPlayerName() + " has sheild.", Color.yellow);
        }
    }

    public void Select1()
    {
        result_selection = 1;
        selection_made = true;
    }

    public void Select2()
    {
        result_selection = 2;
        selection_made = true;
    }

    public void Select3()
    {
        result_selection = 3;
        selection_made = true;
    }

    public void Select4()
    {
        result_selection = 4;
        selection_made = true;
    }
}
