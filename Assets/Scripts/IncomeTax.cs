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
        yield return null;
    }
}