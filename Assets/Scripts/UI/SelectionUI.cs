// This is for selection UI. Data should be retrieved from database or static data like txt or csv.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionUI : MonoBehaviour
{
    public static SelectionUI instance;
    void Awake()
    {
        instance = this;
    }

    [SerializeField] private Image image;
    [SerializeField] private Text content;

    [SerializeField] private Text choice1;
    [SerializeField] private Text choice2;
    [SerializeField] private Text choice3;
    [SerializeField] private Text choice4;

    [HideInInspector] public bool selection_made;
    [HideInInspector] public int  result_selection;

    public IEnumerator GetSelection(Selection selection)
    {
        selection_made = false;
        content.text = selection.content;
        choice1.text = selection.choice1;
        choice2.text = selection.choice2;
        choice3.text = selection.choice3;
        choice4.text = selection.choice4;
        image.sprite = selection.deed;
        transform.GetChild(0).gameObject.SetActive(true);
        while (!selection_made)
            yield return null;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Select1()
    {
        result_selection = 1;
        selection_made = true;
    }

    public void Select2()
    {
        result_selection = 2;
        selection_made = true;
    }

    public void Select3()
    {
        result_selection = 3;
        selection_made = true;
    }

    public void Select4()
    {
        result_selection = 4;
        selection_made = true;
    }
}
