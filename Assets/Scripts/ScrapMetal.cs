using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapMetal : MonoBehaviour {

    [SerializeField] private int scrapValue = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().scrapMetal += scrapValue;
            Debug.Log(collision.gameObject.GetComponent<PlayerController>().scrapMetal);
            Destroy(gameObject);
        }
    }
}
