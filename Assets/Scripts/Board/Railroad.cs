using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railroad : Ownable
{
    [SerializeField] private Railroad[] otherRailroads;

    protected override int ChargePlayer()
    {
        // Determine how much to charge.  
        int toCharge = this.charge_value;
        foreach (Railroad railroad in otherRailroads)
            if (railroad.owner == owner)
                toCharge *= 2;

        return toCharge;
    }
}
