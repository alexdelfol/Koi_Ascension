using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trasformacion : MonoBehaviour
{
    [Header("Objetos")]
    public GameObject Fake_Koi;
    public GameObject Fondo;
    public GameObject Real_Koi;
    public GameObject pantallaNegra;
    public GameObject pantallaBlanca;
    public GameObject koi_ascension;
    public GameObject moon_blessing;

    public GameObject fake_dragon;


    private float moonPos = 2.33f;
    private float ascensionPos = -2.94f;
    private int fase = 0;

    private float time;

    private Color ascensionColor;


    //Posiciones base

    private float fakeKoiOrgYPos = -6f;
    private float fakeDragonOrgYPos = 3.12f;


    // Start is called before the first frame update
    void Start()
    {
        ascensionColor = koi_ascension.GetComponent<SpriteRenderer>().color;
        ascensionColor.a = 0;
        koi_ascension.GetComponent<SpriteRenderer>().color = ascensionColor;

        //fakeKoiOrgYPos = Fake_Koi.transform.position.y;
        //fakeDragonOrgYPos = fake_dragon.transform.position.y;

        fakeKoiOrgYPos = -6f;
        fakeDragonOrgYPos = 3.12f;

    }

    // Update is called once per frame
    void Update()
    {
        if(Managers.Game.TransformationIsActive())
        {
            time = time - Time.deltaTime;
            switch(fase)
            {
                //Flash
                case 0:
                    if(time < 0f)
                    {
                        pantallaNegra.SetActive(false);
                        fase++;
                    }
                    break;
                //Koi ASCENSION
                case 1:
                    if (Fake_Koi.transform.position.y >= moonPos)
                    {                       
                        fase++;
                        time = 0.2f;
                        pantallaBlanca.SetActive(true);
                        //Sonido
                        Managers.Sound.PlayClip(Managers.Sound.FlashSound);
                    }
                    else
                    {
                        
                        Fake_Koi.transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * 0.5f);
                        if (Fake_Koi.transform.position.y >= ascensionPos)
                        {
                            ascensionColor.a = (Fake_Koi.transform.position.y - ascensionPos) /(moonPos - ascensionPos);
                            koi_ascension.GetComponent<SpriteRenderer>().color = ascensionColor;
                        }
                        }
                    break;
                //Pequena pausa
                case 2:
                    if (time < 0f)
                    {
                        pantallaBlanca.SetActive(false);
                        fase++;
                        time = 2f;
                        moon_blessing.SetActive(true);
                    }
                    break;
                //Bendicion de la Luna
                case 3:
                    if (time < 0f)
                    {
                        pantallaBlanca.SetActive(true);
                        Managers.Sound.PlayClip(Managers.Sound.FlashSound);
                        moon_blessing.SetActive(false);
                        Fake_Koi.SetActive(false);
                        fase++;
                        time = 0.2f;
                    } 
                    break;
                //Flash transformacion
                case 4:
                    if (time < 0f)
                    {
                        pantallaBlanca.SetActive(false); 
                        fase++;
                        time = 1f;
                        fake_dragon.SetActive(true);
                    }
                    break;
                //Aparicion Dragon
                case 5:
                    if (time < 0f)
                    {                     
                        fase++;

                    }
                    break;
                //Ascension Dragon
                case 6:
                    if (fake_dragon.transform.position.y >= 8.12)
                    {
                        fase++;
                        time = 0.2f;
                        pantallaNegra.SetActive(true);
                        //Sonido
                        Managers.Sound.PlayClip(Managers.Sound.FlashSound);
                    }
                    else
                    {

                        fake_dragon.transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * 1.5f);
                        
                    }
                    break;
               //Ultimo flash y fin
                case 7:
                    if (time < 0f)
                    {
                        fase++;
                        pantallaNegra.SetActive(false);
                        //Cambiar Koi Real por dragon

                        Managers.Temp.SpaceTime();

                        //Resetear posiciones
                        fake_dragon.transform.Translate(0, (fakeDragonOrgYPos - 8.12f), 0);
                        Fake_Koi.transform.Translate(0, (fakeKoiOrgYPos - moonPos), 0);
                        Fake_Koi.SetActive(true);
                        Managers.UI.scorePanel.SetActive(true);

                        DeactivateMe();
                    }
                    break;
            }

        } else
        {
            gameObject.SetActive(false);
        }
    }

    public void InitTansform()
    {
        Managers.UI.scorePanel.SetActive(false);
        Real_Koi.SetActive(false);
        fase = 0;
        time = 0.3f;
        pantallaNegra.SetActive(true);
        pantallaBlanca.SetActive(false);

        //Sonido
        Managers.Sound.PlayClip(Managers.Sound.FlashSound);

        ascensionColor = pantallaBlanca.GetComponent<SpriteRenderer>().color;
        ascensionColor.a = 0;
        koi_ascension.GetComponent<SpriteRenderer>().color = ascensionColor;
        moon_blessing.SetActive(false);

        //Cambio música
        Managers.Sound.PlayClip(Managers.Sound.TransformationTheme);

        fake_dragon.SetActive(false);

        Fake_Koi.GetComponent<SpriteRenderer>().sprite = Real_Koi.GetComponent<SpriteRenderer>().sprite;


        //Resetear posiciones
        //fake_dragon.transform.position.Set(fake_dragon.transform.position.x, fakeDragonOrgYPos, fake_dragon.transform.position.z);
        //Fake_Koi.transform.position.Set(Fake_Koi.transform.position.x, fakeKoiOrgYPos, Fake_Koi.transform.position.z);



    }

    public void SkipC()
    {
        fase = 7;
        time = 0;
    }

    private void DeactivateMe()
    {
        gameObject.SetActive(false);
    }
}
