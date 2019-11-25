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
    public Button BenzG;
    public Button Lamborghini;
    public Button Bentley;
    public Button Ferrari;
    public Button RollsRoyce;
    private static int bombPrice = 50000;
    private static int thiefPrice = 50000;
    private static int BenzGPrice = 150000;
    private static int LamborghiniPrice = 250000;
    private static int BentleyPrice = 300000;
    private static int FerrariPrice = 350000;
    private static int RollsRoycePrice = 500000;

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
            itemSelection = 2;
            gold -= thiefPrice;
            thief.gameObject.SetActive(false);
            myGoldDisplay.text = "Current Balance: " + gold.ToString ();

            Debug.Log ("enough gold");

        }
        Debug.Log ("attempted");
    }
    public void SelectBenzG()
    {
        if (gold >= BenzGPrice) 
        {
            itemSelection = 3;
            gold -= BenzGPrice;
            BenzG.gameObject.SetActive(false);
            myGoldDisplay.text = "Current Balance: " + gold.ToString ();

            Debug.Log ("enough gold");

        }
        Debug.Log ("attempted");
    }
    public void SelectLamborghini()
    {
        if (gold >= LamborghiniPrice) 
        {
            itemSelection = 4;
            gold -= LamborghiniPrice;
            Lamborghini.gameObject.SetActive(false);
            myGoldDisplay.text = "Current Balance: " + gold.ToString ();

            Debug.Log ("enough gold");

        }
        Debug.Log ("attempted");
    }
    public void SelectBentley()
    {
        if (gold >= BentleyPrice) 
        {
            itemSelection = 5;
            gold -= BentleyPrice;
            Bentley.gameObject.SetActive(false);
            myGoldDisplay.text = "Current Balance: " + gold.ToString ();

            Debug.Log ("enough gold");

        }
        Debug.Log ("attempted");
    }
    public void SelectFerrari()
    {
        if (gold >= FerrariPrice) 
        {
            itemSelection = 6;
            gold -= FerrariPrice;
            Ferrari.gameObject.SetActive(false);
            myGoldDisplay.text = "Current Balance: " + gold.ToString ();

            Debug.Log ("enough gold");

        }
        Debug.Log ("attempted");
    }
    public void SelectRollsRoyce()
    {
        if (gold >= RollsRoycePrice) 
        {
            itemSelection = 7;
            gold -= RollsRoycePrice;
            RollsRoyce.gameObject.SetActive(false);
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
                        // transform.GetChild(0).gameObject.SetActive(false);
                        player.AdjustBalanceBy(-bombPrice);
                        player.BuyBomb();
                        // yield return BombUI.instance.Bomb(3);
                        itemSelection = 0;
                        Debug.Log("back");
                        // transform.GetChild(0).gameObject.SetActive(true);
                        break;
                    case 2:
                        // yield return TheifUI.instance.Thief();
                        player.AdjustBalanceBy(-thiefPrice);
                        player.BuyThieves();
                        itemSelection = 0;
                        Debug.Log("thief");
                        break;
                    case 3:
                        // yield return TheifUI.instance.Thief();
                        player.AdjustBalanceBy(-BenzGPrice);
                        
                        player.BuyNewCar(Player.CarCollectionsEnum.BenzG);
                        itemSelection = 0;
                        break;
                    case 4:
                        // yield return TheifUI.instance.Thief();
                        player.AdjustBalanceBy(-LamborghiniPrice);
                        player.BuyNewCar(Player.CarCollectionsEnum.Lamborghini);
                        itemSelection = 0;
                        break;
                    case 5:
                        // yield return TheifUI.instance.Thief();
                        player.AdjustBalanceBy(-BentleyPrice);
                        player.BuyNewCar(Player.CarCollectionsEnum.Bentley);
                        itemSelection = 0;
                        break;
                    case 6:
                        // yield return TheifUI.instance.Thief();
                        player.AdjustBalanceBy(-FerrariPrice);
                        player.BuyNewCar(Player.CarCollectionsEnum.Ferrari);
                        itemSelection = 0;
                        break;
                    case 7:
                        // yield return TheifUI.instance.Thief();
                        player.AdjustBalanceBy(-RollsRoycePrice);
                        player.BuyNewCar(Player.CarCollectionsEnum.RollsRoyce);
                        itemSelection = 0;
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