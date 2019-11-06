﻿#define DEVELOP
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static int instantiatedPlayers = 0;
    // Shield protect player from bombing.
    public bool shield;
    // After been bombed, remain to the current location.
    public int remaining_stays;
    // After upgrade, dice can be 3 or more. Init is 2. 
    public int dice_number;
    // Income ratio when they answer correctly.
    public int income_ratio=1;

    [HideInInspector] public List<Ownable> currentOwnables = new List<Ownable>();
    [SerializeField] public float yOffsetToTheGround;

    [SerializeField] private Material[] playerColors;

    private string playerName;
    public bool rotated;
    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;
    }
    public string GetPlayerName()
    {
        return this.playerName;
    }

    // Incremented by PassGo.  
    public int timesPastGo = 0;

    private bool isAI = false;
    public void SetIsAI(bool isAI)
    {
        this.isAI = isAI;
    }
    public bool IsAI()
    {
        return isAI;
    }

    private BalanceTracker balanceTracker;
    public void SetBalanceTracker(BalanceTracker balanceTracker)
    {
        this.balanceTracker = balanceTracker;

        this.balanceTracker.UpdateName(playerName);
    }

    // Money
    private int accountBalance = 1500;
    public Vector3 costumeOffset;

    public void AdjustBalanceBy(int balance)
    {
        accountBalance += balance;
        balanceTracker.UpdateBalance(accountBalance);
    }

    public int GetBalance()
    {
        return accountBalance;
    }

    public BoardLocation currentSpace;

    public void Initialize()
    {
        // transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial = playerColors[instantiatedPlayers];
        currentSpace = PassGo.instance;
        instantiatedPlayers++;
        costumeOffset = new Vector3(0, yOffsetToTheGround, 0);
        rotated = false;
        remaining_stays = 0;
        shield = false;
        dice_number=2;
    }

    public IEnumerator RotateAdditionalDegrees(float additionalDegrees, float timeForRotate)
    {
        float progressionCoefficient = 0;
        float startTime = Time.time;
        float startAngle = transform.eulerAngles.y;

        while (progressionCoefficient <= .98f)
        {
            progressionCoefficient = (Time.time - startTime) / timeForRotate;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, startAngle + additionalDegrees * progressionCoefficient, transform.eulerAngles.z);

            yield return null;
        }

        // Finalize rotation.  
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, startAngle + additionalDegrees, transform.eulerAngles.z);
    }

    public IEnumerator JumpToSpace(BoardLocation space, float timeForJump)
    {
        float startTime = Time.time;

        Vector3 startPosition = transform.position; // not current space position because we might start abnormally.  
        Vector3 endPosition = space.gameObject.transform.position;

        Vector3 desiredDisplacement = endPosition - startPosition;
        desiredDisplacement.y = 0;

        float progressionCoefficient = 0;
        while (progressionCoefficient <= .98f)
        {
            progressionCoefficient = (Time.time - startTime) / timeForJump;

            Vector3 newPosition = startPosition + desiredDisplacement * progressionCoefficient;
            newPosition.y = -1 * Mathf.Pow(progressionCoefficient - 0.5f, 2) + 0.25f;

            transform.position = newPosition;

            yield return null;
        }

        // Onto the next space!
        currentSpace = space;
        transform.position = currentSpace.transform.position + this.costumeOffset;
    }

    public IEnumerator MoveSpaces(int spaces)
    {
        bool movingForward = spaces > 0;
        spaces = Math.Abs(spaces);

        for (int i = 0; i < spaces; i++)
        {
            BoardLocation targetSpace = movingForward ? currentSpace.next : currentSpace.preceding;

            currentSpace.PassBy(this);
#if DEVELOP
            float timeForJump = Globals.DEVELOP_TIME;

#else
            float timeForJump = .9f * (Mathf.Sqrt((i * 1.0f) / spaces + .8f) - .35f);
#endif
            //TODO MARK:CHANGE CAMERA VIEW
            yield return currentSpace.LerpCameraViewToThisLocationWhenPass();

            yield return JumpToSpace(targetSpace, timeForJump);

            // Rotate if we're on a corner space.  
            if (currentSpace is PassGo || currentSpace is GoToJail || currentSpace is InJail ||
                currentSpace is FreeParking)
            {
                rotated = !rotated;
                Debug.Log("[146}" + rotated);
#if DEVELOP
                yield return RotateAdditionalDegrees(movingForward ? 90 : -90, Globals.DEVELOP_TIME);
#else
                yield return RotateAdditionalDegrees(movingForward ? 90 : -90, 1f);
#endif
            }
        }

        // Tell the space we ended on that we landed on it.  
        yield return currentSpace.LandOn(this);
    }
}
