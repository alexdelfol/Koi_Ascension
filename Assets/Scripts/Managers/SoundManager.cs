using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    public AudioSource KoiMovementSound, KoiHitSound, KoiDieSound, IntroTheme, InGameTheme;

    public AudioSource FlashSound, TransformationTheme, SpaceTheme, CoinSound, DragonMoveSound, BubblePopSound;

    private const string keyMusic = "music";
    private const string keyEffects = "sfx";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void IGameManager.Startup()
    {
        status = ManagerStatus.Initializing;
        Debug.Log("UI manager starting...");
        KoiMovementSound.Stop();
        KoiHitSound.Stop();
        KoiDieSound.Stop();
        IntroTheme.Stop();
        InGameTheme.Stop();
        FlashSound.Stop();
        TransformationTheme.Stop();
        CoinSound.Stop();
        BubblePopSound.Stop();


        //Debug.Log("Slider Persistencia Value: " + PlayerPrefs.GetFloat(keyEffects));

        ChangeMusicVolume(PlayerPrefs.GetFloat(keyMusic,0.75f));
        ChangeEffectsVolume(PlayerPrefs.GetFloat(keyEffects,1f));

        status = ManagerStatus.Started;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeEffectsVolume(float value)
    {
        KoiMovementSound.volume = value;
        KoiHitSound.volume = value;
        KoiDieSound.volume = value;
        FlashSound.volume = value;
        CoinSound.volume = value;
        BubblePopSound.volume = value;
        DragonMoveSound.volume = value;

    }

    public void ChangeMusicVolume(float value)
    {
        IntroTheme.volume = value;
        InGameTheme.volume = value;
        TransformationTheme.volume = value;
        SpaceTheme.volume = value;
    }

    public void PlayClip(AudioSource audioSource)
    {
        //MaxVolume();
        switch (audioSource.name)
        {
            case "InGameTheme":
                KoiMovementSound.Stop();
                KoiHitSound.Stop();
                KoiDieSound.Stop();
                IntroTheme.Stop();
                InGameTheme.Play();
                TransformationTheme.Stop();
                SpaceTheme.Stop();
                break;
            case "IntroTheme":
                KoiMovementSound.Stop();
                KoiHitSound.Stop();
                KoiDieSound.Stop();
                IntroTheme.Play();
                InGameTheme.Stop();
                TransformationTheme.Stop();
                SpaceTheme.Stop();
                break;
            case "KoiMovementSound":
                KoiMovementSound.Play();
                //KoiHitSound.Stop();
                //KoiDieSound.Stop();
                //IntroTheme.Stop();
                //InGameTheme.Stop();
                break;
            case "KoiHitSound":
                //KoiMovementSound.Stop();
                KoiHitSound.Play();
                //KoiDieSound.Stop();
                //IntroTheme.Stop();
                //InGameTheme.Stop();
                break;
            case "KoiDieSound":
                //KoiMovementSound.Stop();
                //KoiHitSound.Stop();
                KoiDieSound.Play();
                //IntroTheme.Stop();
                //InGameTheme.Stop();
                break;
            case "FlashSound":
                FlashSound.Play();
                break;
            case "TransformationTheme":
                TransformationTheme.Play();
                IntroTheme.Stop();
                InGameTheme.Stop();
                SpaceTheme.Stop();
                break;
            case "SpaceTheme":
                TransformationTheme.Stop();
                IntroTheme.Stop();
                InGameTheme.Stop();
                SpaceTheme.Play();
                break;
            case "CoinSound":
                CoinSound.Play();
                break;
            case "DragonMoveSound":
                DragonMoveSound.Play();
                break;
            case "BubblePopSound":
                BubblePopSound.Play();
                break;

        }
        
    }

    public void StopMusic()
    {
        KoiMovementSound.Stop();
        KoiHitSound.Stop();
        KoiDieSound.Stop();
        IntroTheme.Stop();
        InGameTheme.Stop();
        TransformationTheme.Stop();
    }

    public void PlayDeathMusic()
    {
        //StopMusic();
        //InGameTheme.volume = PlayerPrefs.GetFloat("music", 0.2f);
        //InGameTheme.Play();
    }

    public void MaxVolume()
    {
        InGameTheme.volume = 1.0f;
        IntroTheme.volume = 1.0f;
        KoiDieSound.volume = 1.0f;
        KoiHitSound.volume = 1.0f;
        KoiMovementSound.volume = 1.0f;
    }
}
