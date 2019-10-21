using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class TrueFalse : BoardLocation
{

    [SerializeField] public TextAsset text_asset;//for image

    public Sprite deed;//for image
    public int question_value;
    public string content;
    public bool result;
    public override void PassBy(Player player)
    { }
    private void GetRandomQuestion(string[] lines)
    {
        int size = lines.Length;
        System.Random random = new System.Random();
        int index = random.Next(1, size);
        // Debug.Log(index);
        if (lines[index].Contains("TRUE") || lines[index].Contains("FALSE"))
        {
            index--;
        }
        if (lines[index + 1].Contains("TRUE"))
            this.result = true;
        else
            this.result = false;
        this.content = lines[index];
        this.question_value = Int32.Parse(lines[index + 1].Substring(lines[index + 1].IndexOf(" ", 0)));
    }
    public override IEnumerator LandOn(Player player)
    {

        string[] lines = text_asset.text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        GetRandomQuestion(lines);
        if (!player.IsAI())
        {
            yield return LerpCameraViewToThisLocation();
            yield return TrueFalseUI.instance.GetSelection(this);
            //TODO :Add skip button, congratulations and 
            if (TrueFalseUI.instance.result_selection == this.result)
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
