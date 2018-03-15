using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour {

    [SerializeField] private Camera sourceCam;
    [SerializeField] private float damage = 10;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = sourceCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "enemy")
                {
                    hit.transform.gameObject.GetComponent<SeekEnemy>().DealDamage(damage, 0, true);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = sourceCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "enemy")
                {
                    hit.transform.gameObject.GetComponent<SeekEnemy>().DealDamage(damage, 1, false);
                }
            }
        }
    }
}
