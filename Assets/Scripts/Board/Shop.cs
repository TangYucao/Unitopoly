using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class Shop : BoardLocation
{

    // [SerializeField] public TextAsset text_asset;//for image
    public Sprite deed;//for image
    public override void PassBy(Player player)
    { }
    public override IEnumerator LandOn(Player player)
    {
        if (!player.IsAI())
        {
            yield return LerpCameraViewToThisLocation();
            Debug.Log("shop");
            // yield return TrueFalseUI.instance.GetSelection(this);
            yield return ShopScrollList.instance.PurchaseInShop(player);
            
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
