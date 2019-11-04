using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class Shop : BoardLocation
{

    [SerializeField] public TextAsset text_asset;//for image
    public Sprite deed;//for image
    public override void PassBy(Player player)
    { }
    public override IEnumerator LandOn(Player player)
    {
        if (!player.IsAI())
        {
            // yield return LerpCameraViewToThisLocation();
            // // yield return TrueFalseUI.instance.GetSelection(this);
            // //TODO :Add skip button, congratulations and 
            // // if (TrueFalseUI.instance.result_selection == this.result)
            // {
            //     // player.AdjustBalanceBy(question_value);
            //     yield return MessageAlert.instance.DisplayAlert(player.GetPlayerName() + " is correct. Get "+question_value+" point", Color.green);
            // }
            // // else
            // {
            //     yield return MessageAlert.instance.DisplayAlert(player.GetPlayerName() + " is wrong." , Color.red);

            // } 

            yield return LerpCameraViewToThisLocationWhenPass();
        }
        else
        {
            // TODO more complex AI logic.  
            // player.AdjustBalanceBy(question_value);
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
