using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    public Koi koi;
    public Dragon dragon;
    //private float speed = 1;
    private int direction = 1;
    private float timeAlive = 0;
    // Start is called before the first frame update
    void Start()
    {
        koi = FindObjectOfType<Koi>();
        dragon = FindObjectOfType<Dragon>();
        if (gameObject.transform.position.x > 0)
            direction = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Managers.Game.IsPlaying())
        {

            gameObject.transform.Translate(new Vector3(0.5f * direction, -1, 0) * Time.deltaTime * Managers.Game.Speed);

            if (gameObject.transform.position.y < -7)
            {
                Destroy(this.gameObject);
            }

            timeAlive += Time.deltaTime;
            if(timeAlive > 60)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            if(col.gameObject.GetComponent<Koi>())
            {
                koi.Hit();
            } else if (col.gameObject.GetComponent<Dragon>())
            {
                dragon.Hit();
            }
            
        } else if(col.gameObject.tag == "Angry_Fish")
        {
            HitByAngry();
        }

        
        Debug.Log("OnCollisionEnter2D");
        direction *= -1;
        
    }

    private void HitByAngry()
    {
        Destroy(this.gameObject);
    }

   
}
