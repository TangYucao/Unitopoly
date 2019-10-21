// This is for selection UI. Data should be retrieved from database or static data like txt or csv.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrueFalseUI : MonoBehaviour
{
    public static TrueFalseUI instance;
    void Awake()
    {
        instance = this;
    }

    [SerializeField] private Image image;
    [SerializeField] private string content;

    [HideInInspector] public bool selection_made, result_selection;

    public IEnumerator GetSelection(TrueFalse true_false)
    {
        selection_made = false;
        content=true_false.content;
        image.sprite = true_false.deed;
        transform.GetChild(0).gameObject.SetActive(true);
        while (!selection_made)
            yield return null;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SelectTrue()
    {
        result_selection = true;
        selection_made = true;
    }

    public void SelectFalse()
    {
        result_selection = false;
        selection_made = true;
    }
}
