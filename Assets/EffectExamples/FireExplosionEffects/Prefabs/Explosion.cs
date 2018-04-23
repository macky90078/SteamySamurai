using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(die());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator die()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
