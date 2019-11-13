using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SampleShopButton : MonoBehaviour {

    public Button buttonComponent;
    public Text nameLabel;
    public Image iconImage;
    public Text priceText;

    // private Item item;
    private ShopScrollList shopUI;

    // Use this for initialization
    void Start () 
    {
        buttonComponent.onClick.AddListener (HandleClick);
    }

    // public void Setup(Item currentItem, ShopScrollList currentScrollList)
    // {
    //     item = currentItem;
    //     nameLabel.text = item.itemName;
    //     iconImage.sprite = item.icon;
    //     priceText.text = item.price.ToString ();
    //     scrollList = currentScrollList;

    // }
        public void Setup(ShopScrollList currentScrollList)
    {
        shopUI = currentScrollList;
    }

    public void HandleClick()
    { 
        // buttonComponent.gameObject.SetActive(false);
        Debug.Log("click");
        // ShopScrollList.instance.PurchaseGameItem(buttonComponent, priceText);
        // Debug.Log("click2");
    }
}