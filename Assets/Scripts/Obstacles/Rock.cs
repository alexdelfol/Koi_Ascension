using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public Koi koi;
    public Dragon dragon;
    //private float speed = 1;
    // Start is called before the first frame update
    void Start()
    {
        koi = FindObjectOfType<Koi>();
        dragon = FindObjectOfType<Dragon>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Managers.Game.IsPlaying())
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
            if (col.gameObject.GetComponent<Koi>())
            {
                koi.Hit();
            }
            else if (col.gameObject.GetComponent<Dragon>())
            {
                dragon.Hit();
            }
        }
        
    }
   
}
