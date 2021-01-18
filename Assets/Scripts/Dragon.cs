using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{

    public GameObject shield_Object;

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

    public Koi koi;
    
    [Header("Cabeza")]
    public GameObject headCenter;
    public GameObject headRight;
    public GameObject headLeft;

    [Header("Cuerpo")]
    public GameObject bodyCenter;
    public GameObject bodyToLeft;
    public GameObject bodyLeftToCenter;
    public GameObject bodyToRight;
    public GameObject bodyRightToCenter;

    private int hits;


    // Start is called before the first frame update
    void Start()
    {
        ocolor = bodyCenter.gameObject.GetComponent<SpriteRenderer>().color;
        icolor = new Color(ocolor.r, ocolor.g, ocolor.b, 0.25f);
        starcolor = new Color(200f, 200f, 0f);
        shield_Object.SetActive(false);
        initPos = gameObject.transform.position;
        initPos.y = -3.91f;
        headCenter.SetActive(true);
        headRight.SetActive(false);
        headLeft.SetActive(false);

        bodyRightToCenter.SetActive(false);
        bodyLeftToCenter.SetActive(false);
        bodyToRight.SetActive(false);
        bodyToLeft.SetActive(false);
        bodyCenter.SetActive(true);

        hits = Managers.Inventory.GetHit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Managers.Game.IsPlaying())
        {

            //TO DO: Cambiar Right y Left para android
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || (Input.touchCount > 0 && Input.GetTouch(0).position.x > Screen.width / 2))
            {
                if (bodyCenter.activeSelf)
                {
                    bodyCenter.SetActive(false);
                    bodyToRight.SetActive(true);
                }
                else if (bodyToLeft.activeSelf)
                {
                    bodyToLeft.SetActive(false);
                    bodyLeftToCenter.SetActive(true);
                }
                else if ((bodyLeftToCenter.activeSelf /*&& !bodyLeftToCenter.GetComponent<Animator>().GetComponent<Animation>().isPlaying*/) ||
                       (bodyRightToCenter.activeSelf /*&& !bodyRightToCenter.GetComponent<Animator>().GetComponent<Animation>().isPlaying*/))
                {
                    bodyRightToCenter.SetActive(false);
                    bodyLeftToCenter.SetActive(false);
                    bodyToRight.SetActive(true);
                }
                //print("Derecha");
                gameObject.transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * hspeed);
                //if(!Managers.Sound.KoiMovementSound.isPlaying)
                //Managers.Sound.PlayClip(Managers.Sound.KoiMovementSound);
                //spriteRender.sprite = rightKoi;
                headCenter.SetActive(false);
                headRight.SetActive(true);
                headLeft.SetActive(false);

            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || (Input.touchCount > 0 && Input.GetTouch(0).position.x <= Screen.width / 2))
            {
                if (bodyCenter.activeSelf)
                {
                    bodyCenter.SetActive(false);
                    bodyToLeft.SetActive(true);
                }
                else if (bodyToRight.activeSelf)
                {
                    bodyToRight.SetActive(false);
                    bodyRightToCenter.SetActive(true);
                }
                else if ((bodyLeftToCenter.activeSelf /*&& !bodyLeftToCenter.GetComponent<Animator>().GetComponent<Animation>().isPlaying*/) || 
                        (bodyRightToCenter.activeSelf /*&& !bodyRightToCenter.GetComponent<Animator>().GetComponent<Animation>().isPlaying*/))
                {
                    bodyRightToCenter.SetActive(false);
                    bodyLeftToCenter.SetActive(false);
                    bodyToLeft.SetActive(true);
                }
                //print("Izquierda");
                gameObject.transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * hspeed);
                //if (!Managers.Sound.KoiMovementSound.isPlaying)
                //    Managers.Sound.PlayClip(Managers.Sound.KoiMovementSound);
                //spriteRender.sprite = leftKoi;
                headCenter.SetActive(false);
                headRight.SetActive(false);
                headLeft.SetActive(true);
            }

            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || (Application.platform == RuntimePlatform.Android && Input.touchCount <= 0))
            {
                if (bodyToLeft.activeSelf)
                {
                    bodyToLeft.SetActive(false);
                    bodyLeftToCenter.SetActive(true);
                }
                else if (bodyToRight.activeSelf)
                {
                    bodyToRight.SetActive(false);
                    bodyRightToCenter.SetActive(true);
                }
                //spriteRender.sprite = originalKoiSprite;
                headCenter.SetActive(true);
                headRight.SetActive(false);
                headLeft.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.touchCount > 0)
            {
                Managers.Sound.PlayClip(Managers.Sound.DragonMoveSound);
            }

            //Touch





            //Hit animation
            if (itime > 0)
            {
                if (itime % 0.4f > 0.2f)
                {
                    //gameObject.GetComponent<SpriteRenderer>().color = ocolor;
                    ChangeColor(ocolor);
                }
                else
                {
                    //gameObject.GetComponent<SpriteRenderer>().color = icolor;
                    ChangeColor(icolor);
                }
                itime -= Time.deltaTime;
                if (itime <= 0)
                {
                    itime = 0;
                    //gameObject.GetComponent<SpriteRenderer>().color = ocolor;
                    ChangeColor(ocolor);

                }
            }

            if (stared)
            {
                starduration -= Time.deltaTime;
                if (starduration <= 0)
                {
                    StarDown();
                }
            }

        }

    }



    public void Hit()
    {
        if (itime == 0 && !stared)
        {
            //if hay escudo, hay hit extra - etc quitar escudo. Else muerte.
            if (shielded)
            {
                itime = 2;
                ShieldDown();
            }
            else
            {
                Managers.Sound.PlayClip(Managers.Sound.KoiHitSound);
                hits--;
                itime = 2;
                Debug.Log("Dragon hit");
                if (hits == 0)
                {
                    //Muerte                                     
                    Managers.Game.GameOver();
                } else {
                    Managers.UI.Hitted(hits);
                }
            }


        }
    }

    public void ResetPos()
    {
        gameObject.SetActive(true);
        //gameObject.transform.position = initPos;
        itime = 0;
        headCenter.SetActive(true);
        headRight.SetActive(false);
        headLeft.SetActive(false);
        hits = koi.GetHits();
        stared = false;
        ShieldDown();
        //ChangeColor(ocolor);
        //spriteRender.sprite = originalKoiSprite;

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
            //gameObject.GetComponent<SpriteRenderer>().color = starcolor;
            ChangeColor(starcolor);
            starduration = 4.5f;
        }
        
    }

    public void StarDown()
    {
        stared = false;
        Managers.Game.StarDown();
        //gameObject.GetComponent<SpriteRenderer>().color = ocolor;
        ChangeColor(ocolor);
    }


    public void ChangeColor(Color c)
    {

        bodyToLeft.GetComponent<SpriteRenderer>().color = c;
        bodyLeftToCenter.GetComponent<SpriteRenderer>().color = c;
        bodyToRight.GetComponent<SpriteRenderer>().color = c;
        bodyRightToCenter.GetComponent<SpriteRenderer>().color = c;
        bodyCenter.GetComponent<SpriteRenderer>().color = c;
        headCenter.GetComponent<SpriteRenderer>().color = c;
        headRight.GetComponent<SpriteRenderer>().color = c;
        headLeft.GetComponent<SpriteRenderer>().color = c;
    }


}
