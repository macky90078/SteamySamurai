using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyAI : MonoBehaviour {

    //Public variables
        //None

    //Private Variables
    [SerializeField] private float slowDistPerc; //Percentage of distance away from target location to toggle slow down.
    [SerializeField] private float stopDist;
    [SerializeField] private float slowRotPerc; //Percentage of rotation away from target rotation to toggle slow down.
    [SerializeField] private float velocityMax;
    [SerializeField] private float rotationMax;
    [SerializeField] private float accelLinearInc;
    [SerializeField] private float accelAngularInc;
    [SerializeField] private float accelLinearMax;
    [SerializeField] private float accelAngularMax;
    private float slowDist;
    private float accelLinear;
    private float accelAngular;
    private float velocity = 0.0F; //Linear velocity.
    private float rotation = 0.0F; //Angular velocity.
    private float slowRot;
    private float rotLeft; //Rotation remaining to destination rotation.
    private float distTo;
    private GameObject targetPlayer;
    private Vector3 playerPos;
    private Vector3 targetPos;
    private Quaternion destRot;

    private bool hasTarget = false;
    private bool inDefense = false;
    [SerializeField] private float maxHealth;
    private float health;
    private bool attacking = false;
    private GameObject[] players;

    // Use this for initialization
    void Start()
    {
        StartMoving();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
        FindTargetPlayer();
        Move();
    }

    //Movement towards target player with smoothing
    void Move()
    {
        if (hasTarget)
        {
            targetPos = playerPos - transform.position;
            distTo = targetPos.magnitude;
            destRot = Quaternion.LookRotation(targetPos);
            rotLeft = Quaternion.Angle(transform.rotation, destRot);
            transform.Translate(Vector3.forward * GetMoveSpeed() * Time.deltaTime);
            velocity = Mathf.Clamp(velocity + accelLinear, 0.0F, velocityMax);
            accelLinear = Mathf.Clamp(accelLinear + accelLinearInc, 0.0F, accelLinearMax);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, destRot, GetRotSpeed() * Time.deltaTime);
            rotation = Mathf.Clamp((rotation + accelAngular), 0.0F, rotationMax);
            accelAngular = Mathf.Clamp((accelAngular + accelAngularInc), 0.0F, accelAngularMax);
            transform.eulerAngles = new Vector3(0.0F, transform.eulerAngles.y, 0.0F);
            if (distTo < stopDist)
            {
                hasTarget = false;
                velocity = 0.0F;
            }
        }
    }

    //Switches to defense mode if health is less than OR equal to half of max health
    void CheckHealth()
    {
        if (health <= maxHealth / 2)
        {
            inDefense = true;
            FindTargetPlayer();
        }
        else
            inDefense = false;
    }


    //Finds player with current cube if not in defense mode
    //Used testing script for data cube, in order to not mess with other scripts
    void FindTargetPlayer()
    {
        if(!inDefense)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetComponent<playerCube>().hasCube == true)
                {
                    targetPlayer = players[i];
                    playerPos = targetPlayer.transform.position;
                }
            }
        }

        if (targetPlayer != null)
            hasTarget = true;
        else
            hasTarget = false;
            
    }

    //Initializes movement
    void StartMoving()
    {
        FindTargetPlayer();
        accelLinear = 0.0F;
        accelAngular = 0.0F;
        rotation = 0.0F;
        targetPos = playerPos - transform.position;
        distTo = targetPos.magnitude;
        slowDist = distTo * slowDistPerc;
        destRot = Quaternion.LookRotation(targetPos);
        rotLeft = Quaternion.Angle(transform.rotation, destRot);
        slowRot = rotLeft * slowRotPerc;
    }

    
    float GetMoveSpeed()
    {
        return (distTo >= slowDist ? velocity : Mathf.Lerp(0.0F, velocity, distTo / slowDist));
    }

    float GetRotSpeed()
    {
        return (rotLeft >= slowRot ? rotation : Mathf.Lerp(0.0F, rotation, rotLeft / slowRot));
    }
}
