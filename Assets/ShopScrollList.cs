using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite icon;
    public float price = 1;
}

public class ShopScrollList : MonoBehaviour {

    public List<Item> itemList;
    public Transform contentPanel;
    public ShopScrollList otherShop;
    public Text myGoldDisplay;
    public Text myItemDisplay;
    public SimpleObjectPool buttonObjectPool;

    public float gold = 20f;


    // Use this for initialization
    void Start () 
    {
        RefreshDisplay ();
    }

    void RefreshDisplay()
    {
        if(myGoldDisplay)
            myGoldDisplay.text = "Current Balance: " + gold.ToString ();
        if(myItemDisplay)
            myItemDisplay.text = "My Game Items";
        RemoveButtons ();
        AddButtons ();
    }

    private void RemoveButtons()
    {
        while (contentPanel.childCount > 0) 
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            buttonObjectPool.ReturnObject(toRemove);
        }
    }

    private void AddButtons()
    {
        for (int i = 0; i < itemList.Count; i++) 
        {
            Item item = itemList[i];
            GameObject newButton = buttonObjectPool.GetObject();
            newButton.transform.SetParent(contentPanel);

            SampleShopButton sampleShopButton = newButton.GetComponent<SampleShopButton>();
            // print("index" + i);
            if(sampleShopButton){
                sampleShopButton.Setup(item, this);
            }
        }
    }

    public void TryTransferItemToOtherShop(Item item)
    {
        if (otherShop.gold >= item.price) 
        {
            gold += item.price;
            otherShop.gold -= item.price;

            AddItem(item, otherShop);
            RemoveItem(item, this);

            RefreshDisplay();
            otherShop.RefreshDisplay();
            Debug.Log ("enough gold");

        }
        Debug.Log ("attempted");
    }

    void AddItem(Item itemToAdd, ShopScrollList shopList)
    {
        shopList.itemList.Add (itemToAdd);
    }

    private void RemoveItem(Item itemToRemove, ShopScrollList shopList)
    {
        for (int i = shopList.itemList.Count - 1; i >= 0; i--) 
        {
            if (shopList.itemList[i] == itemToRemove)
            {
                shopList.itemList.RemoveAt(i);
            }
        }
    }

    // public IEnumerator getPlayerItems()
    // {

    // }
}