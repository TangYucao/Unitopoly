using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class Selection : BoardLocation
{

    [SerializeField] public TextAsset text_asset;//for image

    public Sprite deed;//for image
    public int question_value;
    public string content;
    public string choice1;
    public string choice2;
    public string choice3;
    public string choice4;
    public int result;
    public override void PassBy(Player player)
    { }
    private void GetRandomQuestion(string[] lines)
    {
        int size = lines.Length;
        System.Random random = new System.Random();
        int index = random.Next(10, size-5);
        Debug.Log(index);
        while (true)
        {
            int index_of_space_in_line=lines[index+5].IndexOf(" ",0);
            if(index_of_space_in_line<0) 
            {
                index--;
                continue;
            }
            string answer=lines[index + 5].Substring(0,index_of_space_in_line);
            Debug.Log("[31]:"+answer);
            string score=lines[index + 5].Substring(index_of_space_in_line);

            if(int.TryParse(lines[index + 5].Substring(0,index_of_space_in_line), out this.result)&&
            int.TryParse(lines[index + 5].Substring(index_of_space_in_line), out this.question_value))  
                break;
            index--;
        }
        this.content = lines[index];
        choice1 = lines[index + 1];
        choice2 = lines[index + 2];
        choice3 = lines[index + 3];
        choice4 = lines[index + 4];
    }
    public override IEnumerator LandOn(Player player)
    {

        string[] lines = text_asset.text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        GetRandomQuestion(lines);
        if (!player.IsAI())
        {
            yield return LerpCameraViewToThisLocation();
            yield return SelectionUI.instance.GetSelection(this);
            //TODO :Add skip button, congratulations and 
            if (SelectionUI.instance.result_selection == this.result)
            {
                player.AdjustBalanceBy(question_value);
                yield return MessageAlert.instance.DisplayAlert(player.GetPlayerName() + " is correct. Get "+question_value+" point", Color.green);
            }
            else
            {
                yield return MessageAlert.instance.DisplayAlert(player.GetPlayerName() + " is wrong. Answer shoud be choice "+this.result, Color.red);

            }

            yield return LerpCameraViewToThisLocationWhenPass();
        }
        else
        {
            // TODO more complex AI logic.  
            player.AdjustBalanceBy(question_value);
        }
        // else
        // {
        //     if (owner != player)
        //     {
        //         int toCharge = ChargePlayer();
        //         player.AdjustBalanceBy(-toCharge);
        //         owner.AdjustBalanceBy(toCharge);
        //     }
        // }

    }
}
