using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    public GameObject mainPanel, optionPanel, storePanel, customizePanel, upPanel, scorePanel, aboutPanel, gameOverPanel, tutorialPanel, lorePanel;
    public GameObject scoreText;
    public GameObject coinText;
    public GameObject coinTextStore;
    public Text gmScoreText;

    [Header("Selector Dificultad")]
    public GameObject easy_button;
    public GameObject hard_button;

    [Header("Control de sonido")]
    public GameObject music_slider;
    public GameObject effects_slider;

    [Header("Botones de Inicio")]
    public GameObject botonInicioReal;
    public GameObject botonInicioTutorial;

    [Header("Imagenes de seleccion de skin")]
    public GameObject originalSkin;
    public GameObject verdeSkin;
    public GameObject ninjaSkin;

    [Header("Hit coint")]
    public GameObject hitInfo;
    public Text hitInfoText;

    private Color selectionColor;
    private Color noColor;

    private bool tutorial;
    private bool lore;
    private const string TUTORIAL_KEY = "Tutorial";
    private const string LORE_KEY = "Lore";

    private const string keyMusic = "music";

    private const string keyEffects = "sfx";
    // Start is called before the first frame update
    void IGameManager.Startup()
    {
        status = ManagerStatus.Initializing;
        Debug.Log("UI manager starting...");
        optionPanel.SetActive(false);
        storePanel.SetActive(false);
        customizePanel.SetActive(false);
        mainPanel.SetActive(false);
        upPanel.SetActive(false);
        lorePanel.SetActive(true);
        scorePanel.SetActive(false);
        aboutPanel.SetActive(false);
        gameOverPanel.SetActive(false);

       

        //Musica de intro
        //Managers.Sound.ChangeMusicVolume(PlayerPrefs.GetFloat(keyMusic));
        Managers.Sound.PlayClip(Managers.Sound.IntroTheme);

        music_slider.GetComponent<Slider>().value = PlayerPrefs.GetFloat(keyMusic,0.75f);
        effects_slider.GetComponent<Slider>().value = PlayerPrefs.GetFloat(keyEffects,1f);

        //Managers.Sound.ChangeMusicVolume(0.05f);

        selectionColor = easy_button.GetComponent<Image>().color;
        noColor = hard_button.GetComponent<Image>().color;

        //Usar persistencia
        tutorial = ("True" == PlayerPrefs.GetString(TUTORIAL_KEY));
        lore = ("True" == PlayerPrefs.GetString(LORE_KEY));
        //Debug.Log(PlayerPrefs.GetString(TUTORIAL_KEY));
        if (tutorial || lore)
        {
            //Buscar el tutorial y destruirlo
            DestroyTutorialObjects();
            DestroyLoreObjects();
        } else
        {
            botonInicioReal.SetActive(false);
        }

        if (Managers.Inventory.GetHit() > 1)
        {
            hitInfo.SetActive(true);
            hitInfoText.text = "" + Managers.Inventory.GetHit();
        } else
        {
            hitInfo.SetActive(false);
        }


            status = ManagerStatus.Started;
    }

    // Update is called once per frame
    void Update()
    {
        if(status == ManagerStatus.Started)
        {
            if(Managers.Game.IsPlaying())
            {
                scoreText.GetComponent<Text>().text = Managers.Game.GetScore() + " m";
                int monedasTotales = Managers.Inventory.GetCoins();
                coinText.GetComponent<Text>().text = "" + monedasTotales;
                coinTextStore.GetComponent<Text>().text = "" + monedasTotales;
            } 
        }
    }

    public void DestroyTutorialObjects()
    {
        Destroy(tutorialPanel);
        Destroy(botonInicioTutorial);
        botonInicioReal.SetActive(true);
    }

    public void DestroyLoreObjects()
    {
        Destroy(lorePanel);
        mainPanel.SetActive(true);
        upPanel.SetActive(true);
    }

    public void ShowOptions()
    {
        // TO DO: diferenciar de donde vienes, pantalla de inicio o pantalla de juego
        optionPanel.SetActive(true);
        storePanel.SetActive(false);
        customizePanel.SetActive(false);
        mainPanel.SetActive(false);
    }

    public void ShowStore()
    {
        optionPanel.SetActive(false);
        storePanel.SetActive(true);
        customizePanel.SetActive(false);
        mainPanel.SetActive(false);
    }

    public void ShowCustomize()
    {
        optionPanel.SetActive(false);
        storePanel.SetActive(false);
        customizePanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void QuitPanel()
    {
        optionPanel.SetActive(false);
        storePanel.SetActive(false);
        customizePanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void QuitPanelOptions()
    {
        optionPanel.SetActive(false);
        storePanel.SetActive(false);
        customizePanel.SetActive(false);
        if(Managers.Game.gameStatus == GameStatus.Init)
        {
            mainPanel.SetActive(true);
        }
       
    }

    public void Play()
    {
        optionPanel.SetActive(false);
        storePanel.SetActive(false);
        customizePanel.SetActive(false);
        mainPanel.SetActive(false);
        upPanel.SetActive(false);
        Managers.Sound.PlayClip(Managers.Sound.InGameTheme);

    }

    public void SetEasy()
    {
        easy_button.GetComponent<Image>().color = selectionColor;
        easy_button.GetComponentInChildren<Text>().color = selectionColor;

        hard_button.GetComponent<Image>().color = noColor;
        hard_button.GetComponentInChildren<Text>().color = noColor;

        Managers.Game.speedMod = 0.01f;
    }

    public void SetHard()
    {
        easy_button.GetComponent<Image>().color = noColor;
        easy_button.GetComponentInChildren<Text>().color = noColor;

        hard_button.GetComponent<Image>().color = selectionColor;
        hard_button.GetComponentInChildren<Text>().color = selectionColor;

        Managers.Game.speedMod = 0.05f;
    }

    public void ChangeScoreGMText(string tt)
    {
        gmScoreText.text = tt;
    }

    public void ChangeVolume()
    {
        /*
        Debug.Log("Change Volumen -------------------------");
        //Coger valor de persistencia
        PlayerPrefs.SetFloat(keyMusic, music_slider.GetComponent<Slider>().value);
        Managers.Sound.ChangeMusicVolume(music_slider.GetComponent<Slider>().value);
        
        PlayerPrefs.SetFloat(keyEffects, effects_slider.GetComponent<Slider>().value);
        Managers.Sound.ChangeEffectsVolume(effects_slider.GetComponent<Slider>().value);

        Debug.Log("Slider Value: " + effects_slider.GetComponent<Slider>().value);
        Debug.Log("Slider Persistencia Value: " + PlayerPrefs.GetFloat(keyEffects));


        PlayerPrefs.Save();*/
    }

    public void ChangeSliderMusic()
    {
        PlayerPrefs.SetFloat(keyMusic, music_slider.GetComponent<Slider>().value);
        Managers.Sound.ChangeMusicVolume(music_slider.GetComponent<Slider>().value);

        PlayerPrefs.Save();
    }

    public void ChangeSliderSound()
    {
        PlayerPrefs.SetFloat(keyEffects, effects_slider.GetComponent<Slider>().value);
        Managers.Sound.ChangeEffectsVolume(effects_slider.GetComponent<Slider>().value);

        PlayerPrefs.Save();
    }



    public void TutorialExecuted()
    {
        PlayerPrefs.SetString("Tutorial", bool.TrueString);
        PlayerPrefs.Save();
    }

    public void LoreExecuted()
    {
        PlayerPrefs.SetString("Lore", bool.TrueString);
        PlayerPrefs.Save();
    }

    public void Hitted(int h)
    {
        hitInfoText.text = "" + h;
    }

    public void ShowHit()
    {

        if (Managers.Inventory.GetHit() > 1)
        {
           
            hitInfo.SetActive(true);
            hitInfoText.text = "" + Managers.Inventory.GetHit();
        }
        else
        {
            hitInfo.SetActive(false);
        }
    }

}
