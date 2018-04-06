﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardLocation : MonoBehaviour
{
    private BoardLocation previousSpace, nextSpace;
    
    private void Start()
    {
        int currentSpace = Int32.Parse(gameObject.name);
        
        nextSpace = currentSpace < 39 ? 
            gameObject.transform.parent.Find((currentSpace + 1).ToString()).GetComponent<BoardLocation>() : 
            gameObject.transform.parent.Find("0").GetComponent<BoardLocation>();
        
        previousSpace = currentSpace > 0 ? 
            gameObject.transform.parent.Find((currentSpace - 1).ToString()).GetComponent<BoardLocation>() : 
            gameObject.transform.parent.Find("39").GetComponent<BoardLocation>();
    }
    
    // Player instances call this when they pass by this space.  
    public abstract void PassBy(Player player);
    
    // Player instances call this when they land on this space.  
    public abstract void InteractWith(Player player);
}