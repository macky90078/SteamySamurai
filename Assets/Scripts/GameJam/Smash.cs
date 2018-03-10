using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smash : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            UseSmash(-transform.forward);
            Debug.Log("SMASH!!!!!");
        }
    }

    public void UseSmash(Vector3 rayDirection)
    {
        Ray ray = new Ray(transform.position, rayDirection);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f))
        {
            if(hit.transform.tag == "enemy")
            {
                Debug.Log("hit");
            }
            if (hit.transform.tag == "enemy" && hit.transform.gameObject.GetComponent<Unit>().m_isFrozen)
            {
                Debug.Log("Smashed!");
                hit.transform.gameObject.GetComponent<Unit>().m_isDead = true;
            }
        }

    }
}
