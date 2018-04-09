﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperTax : BoardLocation 
{
    public override void PassBy(Player player)
    {
    }

    protected override IEnumerator PropertySpecificActions(Player player)
    {
        yield return MessageAlert.instance.DisplayAlert("Super Tax!  Pay M100.", Color.red);
        player.AdjustBalanceBy(-100);
    }
}
