using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateProjectile : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private float lifeSpan; // How long the projectile will last for
    private float damage;
    private int levelMask;
    private int enemyMask;
    private int sourcePlayerId;
    private Vector3 forward; // The forward vector of this projectile
    private Vector3 startOfRay; // Where to begin raycasting for collision
    private GameObject target; // If something is hit, this is used to store it


    // Use this for initialization
    void Start () {
        SetMasks();
        Destroy(transform.gameObject, lifeSpan);
	}
	
	// Update is called once per frame
	void Update () {
        forward = transform.TransformDirection(Vector3.forward);
        MoveProjectile();
        CheckCollision();
	}

    void CheckCollision()
    {
        // Destroys self if hits level objects
        if (Physics.Raycast(transform.position, forward, 2, levelMask))
        {
            Destroy(gameObject);
        }

        // Check if hits enemy and deals damage
        startOfRay = transform.position - (forward * 2);
        RaycastHit hit;
        if(Physics.Raycast(startOfRay, forward, out hit, 2, enemyMask))
        {
            target = hit.transform.gameObject;
            // Finds the script attached to the enemy, this is a design issue, made a note of it
            target.GetComponent<SeekEnemy>().DealDamage(damage, sourcePlayerId, true);
            Destroy(gameObject);
        }
    }

    void MoveProjectile()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // Assign the damage of this projectile
    public void SetDamage(float dmg, int playerId)
    {
        damage = dmg;
        sourcePlayerId = playerId;
    }
    
    //Sets masks from game manager
    private void SetMasks()
    {
        levelMask = GameManager.levelMask;
        enemyMask = GameManager.enemyMask;
    }
}
