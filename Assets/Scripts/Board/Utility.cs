using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Utility : Ownable
{
	[SerializeField] private Utility otherUtility;

	protected override int ChargePlayer()
	{
		// int toCharge = DieRoller.instance.GetDieRollResults().Sum();
		int toCharge = (int)(charge_value*(otherUtility.owner == owner ? 1 : 0.5));

		return toCharge;
	}
}
