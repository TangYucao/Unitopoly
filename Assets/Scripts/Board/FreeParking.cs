using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeParking : BoardLocation 
{
    public override void PassBy(Player player)
    {
    }

    public override IEnumerator LandOn(Player player)
    {       
        yield return MessageAlert.instance.DisplayAlert("You got M10000 for landing on Free Parking!", Color.cyan);
        player.remaining_stays++;
        player.AdjustBalanceBy(10000);
    }
}
