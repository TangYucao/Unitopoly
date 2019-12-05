using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quit : MonoBehaviour
{
    public static Quit instance;
    void Awake()
    {
        instance = this;
    }


    public void OnClick()
    {
        Application.Quit();
    }

}
