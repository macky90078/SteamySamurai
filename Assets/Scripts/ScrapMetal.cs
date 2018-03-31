using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapMetal : MonoBehaviour {

    [SerializeField] private int scrapValue = 1;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Player2")
        {
            GameManager.reference.scrapMetal += scrapValue;
            GameManager.reference.waveScrap += scrapValue;
            Destroy(gameObject);
        }
    }
}
