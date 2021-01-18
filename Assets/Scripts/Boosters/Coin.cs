using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
            Managers.Inventory.AddCoin();
            Managers.Sound.PlayClip(Managers.Sound.CoinSound);
            DeleteMe();
        } else if (col.gameObject.tag == "Rock")
        {
            DeleteMe();
        }

    }

   
    private void DeleteMe()
    {
        Destroy(this.gameObject);
    }
}
