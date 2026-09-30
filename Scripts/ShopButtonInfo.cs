using UnityEngine;
using UnityEngine.UI;

public class ShopButtonInfo : MonoBehaviour
{
    public int itemID;
    public Text priceText;
    public Text quantityText;

    [SerializeField] private ShopManager shopManager;

    private void Update()
    {
        priceText.text = shopManager.shopItems[2, itemID].ToString();
        quantityText.text = shopManager.shopItems[3, itemID].ToString();
    }
}