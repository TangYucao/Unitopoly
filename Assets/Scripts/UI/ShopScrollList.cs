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
    public static ShopScrollList instance;
    
    public Transform contentPanel;
    // public ShopScrollList otherShop;
    public Text myGoldDisplay;
    // public GameObject shop;
    public Button bomb;
    public Button thief;
    private static int bombPrice = 50000;
    private static int thiefPrice = 50000;
    // public Text myItemDisplay;
    // public SimpleObjectPool buttonObjectPool;
    // public PurchaseConfirmationUI purchaseConfirmationUI;

    public float gold;

    public bool endTurn = false;

    public int itemSelection = 0;

    void Awake()
    {
        instance = this;
        // shop = transform.parent.parent.parent.gameObject;
    }
    // Use this for initialization
    void Start () 
    {
        RefreshDisplay ();
    }

    void RefreshDisplay()
    {
        // gold = Player.
        myGoldDisplay.text = "Current Balance: " + gold.ToString ();
        DisplayButtons();
    }

    private void DisplayButtons() 
    {
        for(int i = 0;i<contentPanel.childCount;i++){
            GameObject toDisplay = contentPanel.GetChild(i).gameObject;
            toDisplay.SetActive(true);
        }
    }

    public void SelectBoom()
    {
        if (gold >= bombPrice) 
        {
            itemSelection = 1;
            gold -= bombPrice;
            bomb.gameObject.SetActive(false);
            myGoldDisplay.text = "Current Balance: " + gold.ToString ();

            Debug.Log ("enough gold");

        }
        Debug.Log ("attempted");
    }
    
    public void SelectThief()
    {
        if (gold >= thiefPrice) 
        {
            itemSelection = 1;
            gold -= thiefPrice;
            thief.gameObject.SetActive(false);
            myGoldDisplay.text = "Current Balance: " + gold.ToString ();

            Debug.Log ("enough gold");

        }
        Debug.Log ("attempted");
    }

    public void EndTurn()
    {
        itemSelection = 0;
        endTurn = true;
    }

    // private void RemoveButtons()
    // {
    //     while (contentPanel.childCount > 0) 
    //     {
    //         GameObject toRemove = transform.GetChild(0).gameObject;
    //         buttonObjectPool.ReturnObject(toRemove);
    //     }
    // }

    // private void AddButtons()
    // {
    //     for (int i = 0; i < itemList.Count; i++) 
    //     {
    //         Item item = itemList[i];
    //         GameObject newButton = buttonObjectPool.GetObject();
    //         newButton.transform.SetParent(contentPanel);

    //         SampleShopButton sampleShopButton = newButton.GetComponent<SampleShopButton>();
    //         // print("index" + i);
    //         if(sampleShopButton){
    //             sampleShopButton.Setup(item, this);
    //         }
    //     }
    // }

    // public void TryTransferItemToOtherShop(Item item)
    // {
    //     if (otherShop.gold >= item.price) 
    //     {
    //         // PurchaseConfirmation();
    //         gold += item.price;
    //         otherShop.gold -= item.price;

    //         AddItem(item, otherShop);
    //         RemoveItem(item, this);

    //         RefreshDisplay();
    //         otherShop.RefreshDisplay();
    //         Debug.Log ("enough gold");

    //     }
    //     Debug.Log ("attempted");
    // }

    // public IEnumerator PurchaseGameItem(Button curItem, Text price)
    // {
    //     Debug.Log("entr");
    //     int itemPrice = int.Parse(price.text);
    //     if (gold >= itemPrice) 
    //     {
    //         gold -= itemPrice;
    //         curItem.gameObject.SetActive(false);
    //         // shop.gameObject.SetActive(false);
    //         myGoldDisplay.text = "Current Balance: " + gold.ToString ();
    //         yield return BombUI.instance.Bomb(3);

    //         Debug.Log ("enough gold");

    //     }
    //     Debug.Log ("attempted");
    // }

    public IEnumerator PurchaseInShop(Player player)
    {
        endTurn = false;
        Debug.Log("test");
        gold = player.GetBalance();
        RefreshDisplay();
        transform.GetChild(0).gameObject.SetActive(true);
        while (!endTurn){
            yield return null;
            while(itemSelection != 0){
                switch(itemSelection){
                    case 1:
                        transform.GetChild(0).gameObject.SetActive(false);
                        player.AdjustBalanceBy(-bombPrice);
                        yield return BombUI.instance.Bomb(3);
                        itemSelection = 0;
                        Debug.Log("back");
                        transform.GetChild(0).gameObject.SetActive(true);
                        break;
                    case 2:
                        // yield return TheifUI.instance.Thief();
                        break;
                }
            }
        }
        transform.GetChild(0).gameObject.SetActive(false);
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
}