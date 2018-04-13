using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour {

    private PlayerController player;
    private NavMeshEnemy enemy;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            enemy = other.gameObject.GetComponent<NavMeshEnemy>();
            if (enemy.beenHit == false)
            {
                enemy.beenHit = true;
                player.hitEnemies.Add(enemy);
                enemy.DealDamage(player.meleeDamage, player.playerId, false);
            }
        }
    }
}
