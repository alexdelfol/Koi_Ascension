using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    private int coins;
    private readonly string COINS = "Coins";

    private readonly string HIT = "Hits";
    private readonly string COINMULT = "CoinMult";
    private readonly string SKIN = "NumSkin";


    private int numHits = 1;
    private int coinMult = 1;
    private int numSkins = 1;

    [Header("Store")]
    public Text textNumHits;
    public Text textValueNumHits;

    public Text textCoinMult;
    public Text textValueCoinMult;

    public Text textValueSkin;

    public Text totalCoins;

    [Header("Skins")]
    public GameObject greenSkin;
    public GameObject ninjaSkin;

    public GameObject showSkin;
    public GameObject buyButtonSkin;



    // Start is called before the first frame update
    void IGameManager.Startup()
    {
        status = ManagerStatus.Initializing;
        Debug.Log("Inventory manager starting...");

        //coins totales
        coins = PlayerPrefs.GetInt(COINS);

        numHits = PlayerPrefs.GetInt(HIT);
        if (numHits == 0)
        {
            numHits = 1;
        }
           
        coinMult = PlayerPrefs.GetInt(COINMULT);
        if(coinMult == 0)
        {
            coinMult = 1;
        }

        numSkins = PlayerPrefs.GetInt(SKIN);
        if (numSkins == 0)
        {
            numSkins = 1;
        }

        //Debug.Log("Inventory: H: " + PlayerPrefs.GetInt(HIT) + " M: " + PlayerPrefs.GetInt(COINMULT) + " S: " + PlayerPrefs.GetInt(SKIN));

        status = ManagerStatus.Started;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoin()
    {
        //Multiplicadores de monedas
        int m = GetCoinMultiplayer();
        coins = coins + 1 * m;
        PlayerPrefs.SetInt(COINS, coins);
        Debug.Log("Coins: " + coins);
        
    }

    public void AddMoreCoins(int c)
    {
        int m = GetCoinMultiplayer();
        coins = coins + c * m;
        PlayerPrefs.SetInt(COINS, coins);
    }

    public int GetCoinMultiplayer()
    {
        return coinMult;
    }

    public bool RemoveCoins(int c)
    {
        if(c > coins)
        {
            return false;
        } else
        {
            coins = coins - c;
            return true;
        }

    }

    public int GetCoins()
    {
        return coins;
    }

    public void SetShopValues()
    {
        totalCoins.text = "" + coins;

        textNumHits.text = "" + numHits;
        textCoinMult.text = "" + coinMult;

        textValueNumHits.text ="" + ValueHit();
        textValueCoinMult.text = "" + ValueMult();
        textValueSkin.text = "" + ValueSkin();

        SetSkins();
        //Debug.Log("Inventory: H: " + PlayerPrefs.GetInt(HIT) + " M: " + PlayerPrefs.GetInt(COINMULT) + " S: " + PlayerPrefs.GetInt(SKIN));


    }

    public int ValueHit()
    {
        switch (numHits)
        {
            case 1:
                return 200;
            case 2:
                return 750;
            case 3:
                return 1500;
        }

        return 5000;
    }

    public int ValueMult()
    {
        switch (coinMult)
        {
            case 1:
                return 150;
            case 2:
                return 400;
            case 3:
                return 800;
            case 4:
                return 2000;
        }

        return 5000;
    }

    public int ValueSkin()
    {
        switch (numSkins)
        {
            case 1:
                return 80;
            case 2:
                return 200;
        }

        return 5000;
    }

    public void BuyHit()
    {
        if(coins >= ValueHit())
        {
            coins -= ValueHit();
            numHits++;
            //Meter en persistencia
            PlayerPrefs.SetInt(HIT,numHits);
            PlayerPrefs.SetInt(COINS, coins);
            SetShopValues();
        }
    }

    public void BuyCMult()
    {
        if(coins >= ValueMult())
        {
            coins -= ValueMult();
            coinMult++;
            PlayerPrefs.SetInt(COINMULT, coinMult);
            PlayerPrefs.SetInt(COINS, coins);
            SetShopValues();
        }
    }

    public void BuySkin()
    {
        if (coins >= ValueSkin())
        {
            coins -= ValueSkin();
            numSkins++;
            PlayerPrefs.SetInt(SKIN, numSkins);
            PlayerPrefs.SetInt(COINS, coins);
            SetShopValues();
        }
    }

    public int GetHit()
    {
        return numHits;
    }

    public int GetSkins()
    {
        return numSkins;
    }

    public void SetSkins()
    {
        switch(numSkins)
        {
            case 1:
                greenSkin.SetActive(false);
                ninjaSkin.SetActive(false);
                showSkin.GetComponent<Image>().sprite = Managers.Game.verdeCenter;
                break;
            case 2:
                greenSkin.SetActive(true);
                ninjaSkin.SetActive(false);
                showSkin.GetComponent<Image>().sprite = Managers.Game.ninjaCenter;
                break;
            case 3:
                greenSkin.SetActive(true);
                ninjaSkin.SetActive(true);
                showSkin.SetActive(false);
                buyButtonSkin.SetActive(false);
                break;
            default:
                greenSkin.SetActive(true);
                ninjaSkin.SetActive(true);
                showSkin.SetActive(false);
                buyButtonSkin.SetActive(false);
                break;
        }
    }


    public void DebugAddCoins()
    {
        coins += 1000;
        SetShopValues();
    }

    public void DebugResetInventario()
    {
        coins = 0;     
        numHits = 1;
        coinMult = 1;
        numSkins = 1;

        PlayerPrefs.SetInt(SKIN, numSkins);
        PlayerPrefs.SetInt(COINMULT, coinMult);
        PlayerPrefs.SetInt(HIT, numHits);
        PlayerPrefs.SetInt(COINS, coins);
    }


}
