using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koi : MonoBehaviour
{
    public GameObject shield_Object;
    public Sprite leftKoi;
    public Sprite rightKoi;

    private float hspeed = 3.8f;



    //Variables auxiliares

    private float itime = 0;
    //private int ianimation = 0;
    private Color ocolor;
    private Color icolor;

    private bool shielded = false;
    private bool stared = false;
    private Color starcolor;
    private float starduration;

    private Vector3 initPos;

    public Sprite originalKoiSprite;
    private SpriteRenderer spriteRender;

    private int hits;
    //public Dragon dragon;

    //Touch
    //private Rect screenLeft = new Rect(0, 0, Screen.width / 2, Screen.height);


    // Start is called before the first frame update
    void Start()
    {
        ocolor = this.gameObject.GetComponent<SpriteRenderer>().color;
        icolor = new Color(ocolor.r, ocolor.g, ocolor.b, 0.25f);
        starcolor = new Color(200f, 200f, 0f);
        shield_Object.SetActive(false);
        initPos = gameObject.transform.position;

        hits = Managers.Inventory.GetHit();

        spriteRender = GetComponent<SpriteRenderer>();
        //originalKoiSprite = spriteRender.sprite;


    }

    // Update is called once per frame
    void Update()
    {
        if (Managers.Game.IsPlaying())
        {

            //TO DO: Cambiar Right y Left para android
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || (Input.touchCount > 0 && Input.GetTouch(0).position.x > Screen.width/2))
            {
                //print("Derecha");
                gameObject.transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * hspeed);
                //if(!Managers.Sound.KoiMovementSound.isPlaying)
                //Managers.Sound.PlayClip(Managers.Sound.KoiMovementSound);
                spriteRender.sprite = rightKoi;
            }
            
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || (Input.touchCount > 0 && Input.GetTouch(0).position.x <= Screen.width / 2))
            {
                //print("Izquierda");
                gameObject.transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * hspeed);
                //if (!Managers.Sound.KoiMovementSound.isPlaying)
                //    Managers.Sound.PlayClip(Managers.Sound.KoiMovementSound);
                spriteRender.sprite = leftKoi;
            }

            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || (Application.platform == RuntimePlatform.Android && Input.touchCount <= 0))
            {
                spriteRender.sprite = originalKoiSprite;
            }

            if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.touchCount > 0)
            {
                Managers.Sound.PlayClip(Managers.Sound.KoiMovementSound);
            }

            //Touch





                //Hit animation
                if (itime > 0)
            {
                if (itime % 0.4f > 0.2f)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = ocolor;
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().color = icolor;
                }
                itime -= Time.deltaTime;
                if (itime <= 0)
                {
                    itime = 0;
                    gameObject.GetComponent<SpriteRenderer>().color = ocolor;

                }
            }

            if(stared)
            {
                starduration -= Time.deltaTime;
                if(starduration <= 0)
                {
                    StarDown();
                }
            }

        }
        
    }

    public void Hit()
    {
        if(itime == 0 && !stared)
        {
            //if hay escudo, hay hit extra - etc quitar escudo. Else muerte.
            if(shielded)
            {
                itime = 2;
                Managers.Sound.PlayClip(Managers.Sound.BubblePopSound);
                ShieldDown();
            } else
            {
                hits--;
                itime = 2;
                Debug.Log("Koi hit");
                Managers.Sound.PlayClip(Managers.Sound.KoiHitSound);
                if (hits == 0)
                {
                    //Muerte                                  
                    Managers.Game.GameOver();
                } else
                {
                    Managers.UI.Hitted(hits);
                }
               
            }
            
            
        }
    }

    public void ResetPos()
    {
        gameObject.SetActive(true);
        gameObject.transform.position = initPos;
        itime = 0;
        spriteRender.sprite = originalKoiSprite;
        hits = Managers.Inventory.GetHit();
        stared = false;
        gameObject.GetComponent<SpriteRenderer>().color = ocolor;
        ShieldDown();

    }

    public void Shield()
    {
        shielded = true;
        shield_Object.SetActive(true);
    }

    private void ShieldDown()
    {
        shielded = false;
        shield_Object.SetActive(false);
    }

    public void Stared()
    {
        if(stared)
        {
            starduration = starduration + 4.5f;
        } else
        {
            stared = true;
            Managers.Game.StarUp();
            gameObject.GetComponent<SpriteRenderer>().color = starcolor;
            starduration = 4.5f;
        }
       
    }

    public void StarDown()
    {
        stared = false;
        gameObject.GetComponent<SpriteRenderer>().color = ocolor;
        Managers.Game.StarDown();
        
    }

    public int GetHits()
    {
        return hits;
    }
  

}

