using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public static GameOver instance;
    void Awake()
    {
        instance = this;
    }

    [SerializeField] private Text displayText;
    [SerializeField] private Button okButton;
    private bool userSaidOK = false;

    public IEnumerator DisplayAlert(string alert, Color okColor)
    {
        okButton.gameObject.GetComponent<Image>().color = okColor;
        displayText.text = alert;
        transform.GetChild(0).gameObject.SetActive(true);
        
        while (!userSaidOK)
            yield return null;
        
        transform.GetChild(0).gameObject.SetActive(false);
        userSaidOK = false;
        Application.Quit();
    }

    public void UserOK()
    {
        userSaidOK = true;
    }
}
