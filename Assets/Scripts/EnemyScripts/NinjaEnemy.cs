using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaEnemy : MonoBehaviour {

    // Public variables
    // none

    // Private Variables
    [Tooltip("Percent of distance from target to start slowing")]
        [SerializeField] private float slowDistPerc = 0.16f; // Percentage of distance away from target location to toggle slow down.
    [Tooltip("Distance to stop from target")]
        [SerializeField] private float stopDist = 2;
    [Tooltip("Percent of distance from desired rotation to start slowing")]
        [SerializeField] private float slowRotPerc = 0.25f; // Percentage of rotation away from target rotation to toggle slow down.
    [Tooltip("Maximum possible velocity")]
        [SerializeField] private float velocityMax = 5f;
    [Tooltip("Maximum possible rotation")]
        [SerializeField] private float rotationMax = 135f;
    [Tooltip("Increase in linear acceleration per frame")]
        [SerializeField] private float accelLinearInc = 0.1f;
    [Tooltip("Increase in angular acceleration per frame")]
        [SerializeField] private float accelAngularInc = 1f;
    [Tooltip("Maximum possible linear acceleration")]
        [SerializeField] private float accelLinearMax = 2f;
    [Tooltip("Maximum possible angular acceleration")]
        [SerializeField] private float accelAngularMax = 180f;
    [Tooltip("Maximum health for enemy")]
        [SerializeField] private float maxHealth = 100f;
    [Tooltip("State the enemy will start in. Should probably be 'seek'.")]
        [SerializeField] private states initialState;
    private float slowDist;
    private float accelLinear;
    private float accelAngular;
    private float velocity = 0.0F; //Linear velocity.
    private float rotation = 0.0F; //Angular velocity.
    private float slowRot;
    private float rotRemaining; //Rotation remaining to destination rotation.
    private float distTo;
    private float health;
    private float p1Dmg; // Amount of damage done from each player
    private float p2Dmg; // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private bool hasTarget = false;
    private bool inDefense = false;
    private Vector3 playerPos;
    private Vector3 targetPos;
    private Quaternion destRot;
    private GameObject targetObj;
    private enum states { seek, defense, charge, idle };
    private states currState;

    [SerializeField] private GameObject m_gPlayer1;
    [SerializeField] private GameObject m_gPlayer2;
    [SerializeField] private GameObject scrapMetalPrefab;



    private bool charging = false;
    [Tooltip("Range that character should start charging")]
        [SerializeField] private float chargeDist = 15f;
    [Tooltip("Maximum possible linear acceleration while charging")]
        [SerializeField] private float chargeAccelMax = 5f;
    [Tooltip("Increase in linear acceleration per frame while charging")]
        [SerializeField] private float chargeAccelInc = 0.5f;
    [Tooltip("Maximum velocity possible while charging")]
        [SerializeField] private float maxChargeVelocity = 25;

    private float currAccelMax;
    private float currAccelInc;
    private float currVelocityMax;


    // Use this for initialization
    void Start()
    {
        InitVars();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
        CheckChargeDist();
        Move();
    }

    // Intialization of various variables.
    void InitVars()
    {
        ChangeState(initialState);
        health = maxHealth;
        accelLinear = 0.0F;
        accelAngular = 0.0F;
        rotation = 0.0F;
    }

    // Initializes movement
    void StartMoving()
    {
        SetGoalPos();
        hasTarget = true;
        slowRot = rotRemaining * slowRotPerc;
    }

    void ChangeState(states newState)
    {
        switch (newState)
        {
            case states.seek:
                targetObj = GameObject.FindGameObjectWithTag("dataCube");
                StartMoving();
                break;
            case states.defense:
                targetObj = GetHighPlayer();
                StartMoving();
                break;
            case states.idle:
                hasTarget = false;
                velocity = 0.0f;
                break;
        }
        currState = newState;
    }

    //Checks which player did the most damage
    GameObject GetHighPlayer()
    {
        return (p1Dmg >= p2Dmg ? m_gPlayer1 : m_gPlayer2);
    }

    // Function for assigning the desired location for character to move to
    void SetGoalPos()
    {
        targetPos = targetObj.transform.position - transform.position;
        distTo = targetPos.magnitude;
        slowDist = distTo * slowDistPerc;
        destRot = Quaternion.LookRotation(targetPos);
        rotRemaining = Quaternion.Angle(transform.rotation, destRot);
    }

    // Movement towards target player with smoothing
    void Move()
    {
        SetGoalPos();
        if (hasTarget)
        {
            LinearMove();
            AngularMove();
        }
        Stopping();
    }

    // Check if within charge distance and start charging if so
    // Adjusts velocity variables if charging or not
    void CheckChargeDist()
    {
        if (distTo <= chargeDist)
            charging = true;
        else
            charging = false;

        if (charging)
        {
            currVelocityMax = maxChargeVelocity;
            currAccelInc = chargeAccelInc;
            currAccelMax = chargeAccelMax;
        }
        else
        {
            currVelocityMax = velocityMax;
            currAccelInc = accelLinearInc;
            currAccelMax = accelLinearMax;
        }
    }

    // Stop if distance to target is within set stopping range.
    void Stopping()
    {
        if (distTo < stopDist)
        {
            hasTarget = false;
            velocity = 0.0F;
            die();
        }
        else
            hasTarget = true;
    }

    // Physically rotate the character towards desired location
    void AngularMove()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, destRot, GetRotSpeed() * Time.deltaTime);
        rotation = Mathf.Clamp((rotation + accelAngular), 0.0F, rotationMax);
        accelAngular = Mathf.Clamp((accelAngular + accelAngularInc), 0.0F, accelAngularMax);
        transform.eulerAngles = new Vector3(0.0F, transform.eulerAngles.y, 0.0F);
    }

    // Physically move the character towards desired location
    void LinearMove()
    {
        transform.Translate(Vector3.forward * GetMoveSpeed() * Time.deltaTime);
        velocity = Mathf.Clamp(velocity + accelLinear, 0.0F, currVelocityMax);
        accelLinear = Mathf.Clamp(accelLinear + currAccelInc, 0.0F, currAccelMax);
    }

    // Slows velocity gradually if within slow distance
    float GetMoveSpeed()
    {
        return (distTo >= slowDist ? velocity : Mathf.Lerp(0.0F, velocity, distTo / slowDist));
    }

    // Slows rotation slowly if closer to desired rotation
    float GetRotSpeed()
    {
        return (rotRemaining >= slowRot ? rotation : Mathf.Lerp(0.0F, rotation, rotRemaining / slowRot));
    }

    // Switches to defense mode if health is less than OR equal to half of max health
    void CheckHealth()
    {
        if (health <= maxHealth / 2)
        {
            ChangeState(states.defense);
        }
    }

    // Call this function from other scripts to deal damage to this enemy from
    // A non-player source
    public void DealDamage(float damage)
    {
        health -= damage;
        if (health < 0)
            health = 0;
    }

    // Call this function from other scripts to deal damage to this enemy from 
    // A player source
    public void DealDamage(float damage, int playerNum)
    {
        health -= damage;
        if (health < 0)
            health = 0;
        switch (playerNum)
        {
            case 1:
                p1Dmg += damage;
                break;
            case 2:
                p2Dmg += damage;
                break;
        }
    }

    void die()
    {
        Instantiate(scrapMetalPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
