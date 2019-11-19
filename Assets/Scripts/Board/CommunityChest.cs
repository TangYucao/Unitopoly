using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityChest : BoardLocation 
{
    public override void PassBy(Player player)
    {
        Debug.Log("Passed by community chest");
    }

    public override IEnumerator LandOn(Player player)
    {
        yield return MessageAlert.instance.DisplayAlert("Car Community Chest! Get M2000.", Color.green);
        player.AdjustBalanceBy(2000);
        yield return null;
    }
}
