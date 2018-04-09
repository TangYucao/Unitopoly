﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeTax : BoardLocation 
{
    public override void PassBy(Player player)
    {
    }

    protected override IEnumerator PropertySpecificActions(Player player)
    {
        yield return MessageAlert.instance.DisplayAlert("Income tax!  Pay M200.", Color.red);
        player.AdjustBalanceBy(-200);
    }
}
