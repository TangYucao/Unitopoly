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
        int index = random.Next(1, size);
        // Debug.Log(index);
        while (lines[index].Length < 5 || lines[index].Substring(0,4) != "test")
        {
            index--;
        }
        this.result = (int)char.GetNumericValue(lines[index + 5][0]);
        this.content = lines[index];
        choice1 = lines[index + 1];
        choice2 = lines[index + 2];
        choice3 = lines[index + 3];
        choice4 = lines[index + 4];
        this.question_value = Int32.Parse(lines[index + 5].Substring(lines[index + 5].IndexOf(" ", 0)));
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
