using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public int[,] shopItems = new int[4, 6];
    public float coins;

    [SerializeField] private Text coinsText;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private IntEvent onCoinGain;
    [SerializeField] private Text bananaText;
    [SerializeField] private Text legoText;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AudioManager audioManager;

    private void Start()
    {
        coinsText.text = coins.ToString();

        // Item IDs
        shopItems[1, 1] = 1; // Lego
        shopItems[1, 2] = 2; // Banana
        shopItems[1, 3] = 3; // Movement speed upgrade
        shopItems[1, 4] = 4; // Sword damage upgrade
        shopItems[1, 5] = 5; // Banana damage upgrade

        // Item prices
        shopItems[2, 1] = 100;
        shopItems[2, 2] = 50;
        shopItems[2, 3] = 1000;
        shopItems[2, 4] = 500;
        shopItems[2, 5] = 300;

        // Item quantities
        shopItems[3, 1] = 3;
        shopItems[3, 2] = 0;
        shopItems[3, 3] = 0;
        shopItems[3, 4] = 0;
        shopItems[3, 5] = 0;
    }

    private void Update()
    {
        bananaText.text = shopItems[3, 2].ToString();
        legoText.text = shopItems[3, 1].ToString();
    }

    public void Buy()
    {
        audioManager.PlaySFX(audioManager.butonBasma);

        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
        int itemID = selectedButton.GetComponent<ShopButtonInfo>().itemID;

        int maxQuantity = 9999;

        if (itemID == 3)
        {
            maxQuantity = 2;
        }
        else if (itemID == 4)
        {
            maxQuantity = 3;
        }
        else if (itemID == 5)
        {
            maxQuantity = 5;
        }

        if (coins >= shopItems[2, itemID] && shopItems[3, itemID] < maxQuantity)
        {
            coins -= shopItems[2, itemID];
            shopItems[3, itemID]++;

            coinsText.text = coins.ToString();
            selectedButton.GetComponent<ShopButtonInfo>().quantityText.text = shopItems[3, itemID].ToString();

            if (itemID == 3)
            {
                playerController.walkspeed += 5;
            }
        }
    }

    public void OpenShopPanel()
    {
        shopPanel.SetActive(true);
        audioManager.PlaySFX(audioManager.shopAcma);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseShopPanel()
    {
        shopPanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnemyDeath(int coinAmount)
    {
        coins += coinAmount;
        coinsText.text = coins.ToString();
    }

    private void OnEnable()
    {
        onCoinGain.AddListener(OnEnemyDeath);
    }

    private void OnDisable()
    {
        onCoinGain.RemoveListener(OnEnemyDeath);
    }
}