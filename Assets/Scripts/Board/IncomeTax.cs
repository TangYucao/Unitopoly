﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeTax : BoardLocation 
{
    public override void PassBy(Player player)
    {
    }

    public override IEnumerator LandOn(Player player)
    {
        yield return MessageAlert.instance.DisplayAlert("Fill gas!  Pay M2000.", Color.red);
        player.AdjustBalanceBy(-2000);
    }
}
