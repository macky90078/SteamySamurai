using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordScript : MonoBehaviour {

    [SerializeField] private GameObject enemy;
    private NavMeshEnemy enemyScript;

	// Use this for initialization
	void Start () {
        enemyScript = enemy.GetComponent<NavMeshEnemy>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Player2")
        {
            Debug.Log("Hit Player");
            other.gameObject.GetComponent<PlayerController>().DealDamage(enemyScript.damage);
        }
    }
}
