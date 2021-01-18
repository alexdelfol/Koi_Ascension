using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angry_Fish : MonoBehaviour
{
    public Koi koi;
    //public GameObject estela;
    private float speed = 1;

    private int state = 0;
    private float adv_time = 2f; //Tiempo de advertencia
    private float atack_time = 2f;
    private float time_count = 0;

    private float speed_mod = 2f;

    // Start is called before the first frame update
    void Start()
    {
        koi = FindObjectOfType<Koi>();
        state = 0;
        time_count = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (Managers.Game.IsPlaying())
        {

            //gameObject.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * speed);
            time_count += Time.deltaTime;
            switch (state)
            {
                case 0:
                    if (time_count > adv_time)
                    {
                        state++;
                        //estela.SetActive(false);
                        time_count = 0;
                        speed_mod = atack_time;
                    }
                    break;
                case 1:
                    speed_mod -= Time.deltaTime;
                    gameObject.transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * speed * 2 * speed_mod);

                    if (time_count > atack_time)
                    {
                        state++;
                        time_count = 0;
                        speed_mod = 0;
                    }
                    break;
                case 2:
                    speed_mod += Time.deltaTime;
                    gameObject.transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * speed * 2 * speed_mod);

                    if (time_count > (atack_time + 0.5f))
                    {
                        Destroy(this.gameObject);

                    }
                    break;

            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");
        if(col.gameObject.tag == "Player")
        {
            koi.Hit();
        } else if(col.gameObject.tag == "Rock")
        {
            DieByRock();
        }
        

    }

    private void DieByRock()
    {
        Destroy(this.gameObject);
    }

}
