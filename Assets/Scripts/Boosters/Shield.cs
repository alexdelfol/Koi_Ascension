using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    //private float speed = 1;
    public Koi koi;
    public Dragon dragon;
    // Start is called before the first frame update
    void Start()
    {
        koi = FindObjectOfType<Koi>();
        dragon = FindObjectOfType<Dragon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Managers.Game.IsPlaying())
        {
            gameObject.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * Managers.Game.Speed);

            if (gameObject.transform.position.y < -7)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("OnCollisionEnter2D");
            Managers.Sound.PlayClip(Managers.Sound.BubblePopSound);
            if (col.gameObject.GetComponent<Koi>())
            {
                koi.Shield();
            }
            else if (col.gameObject.GetComponent<Dragon>())
            {
                dragon.Shield();
            }
          
            DeleteMe();
        }

    }

    private void DeleteMe()
    {
        Destroy(this.gameObject);
    }

}
