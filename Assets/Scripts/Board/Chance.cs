using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Chance : BoardLocation 
{
    public override void PassBy(Player player)
    {
        Debug.Log("Passed by Chance");
    }

    public override IEnumerator LandOn(Player player)
    {
        int toCharge = DieRoller.instance.GetDieRollResults().Sum();
        System.Random random = new System.Random();
        int negative = random.Next(1, int.MaxValue)%2;
		toCharge *= 2000*(negative == 1 ? -1 : 1);
        string output="";
        if(toCharge>0)
            output="Congratulation!";
            else
        output="Bad Luck!";
        output+=player.GetPlayerName()+" get "+toCharge.ToString();
        yield return null;
    }
}
