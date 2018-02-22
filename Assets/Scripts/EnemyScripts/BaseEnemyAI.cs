using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyAI : MonoBehaviour {

    //Public variables
    //none

    //Private Variables
    [Tooltip("Percent of distance from target to start slowing")]
        [SerializeField] private float slowDistPerc = 0.16f; //Percentage of distance away from target location to toggle slow down.
    [Tooltip("Distance to stop from target")]
        [SerializeField] private float stopDist = 2;
    [Tooltip("Percent of distance from desired rotation to start slowing")]
        [SerializeField] private float slowRotPerc = 0.25f; //Percentage of rotation away from target rotation to toggle slow down.
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
    private float slowDist;
    private float accelLinear;
    private float accelAngular;
    private float velocity = 0.0F; //Linear velocity.
    private float rotation = 0.0F; //Angular velocity.
    private float slowRot;
    private float rotRemaining; //Rotation remaining to destination rotation.
    private float distTo;
    private float health;
    private float p1Dmg;
    private float p2Dmg;
    private bool hasTarget = false;
    private bool inDefense = false;
    private bool attacking = false;
    private Vector3 playerPos;
    private Vector3 targetPos;
    private Quaternion destRot;
    private GameObject targetObj;

    // Use this for initialization
    void Start()
    {
        InitVars();
        StartMoving();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
        Move();
    }

    void InitVars()
    {
        targetObj = GameObject.FindGameObjectWithTag("dataCube");
        health = maxHealth;
        accelLinear = 0.0F;
        accelAngular = 0.0F;
        rotation = 0.0F;
    }

    //Movement towards target player with smoothing
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

    // Stop if distance to target is within set stopping range.
    void Stopping()
    {
        if (distTo < stopDist)
        {
            hasTarget = false;
            velocity = 0.0F;
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
        velocity = Mathf.Clamp(velocity + accelLinear, 0.0F, velocityMax);
        accelLinear = Mathf.Clamp(accelLinear + accelLinearInc, 0.0F, accelLinearMax);
    }

    //Switches to defense mode if health is less than OR equal to half of max health
    void CheckHealth()
    {
        if (health <= maxHealth / 2)
        {
            inDefense = true;
        }
        else
            inDefense = false;
    }

    //Initializes movement
    void StartMoving()
    {
        SetGoalPos();
        hasTarget = true;
        slowRot = rotRemaining * slowRotPerc;
    }

    /* Function for assigning the desired location for character to move to
    Pre-condition: Must have playerPos assigned. Call FindTargetPlayer. */
    void SetGoalPos()
    {
        targetPos = targetObj.transform.position - transform.position;
        distTo = targetPos.magnitude;
        slowDist = distTo * slowDistPerc;
        destRot = Quaternion.LookRotation(targetPos);
        rotRemaining = Quaternion.Angle(transform.rotation, destRot);
    }

    //Call this function from other scripts to deal damage to this enemy from
    //A non-player source
    void dealDamage(float damage)
    {
        health -= damage;
        if (health < 0)
            health = 0;
    }

    //Call this function from other scripts to deal damage to this enemy from a player
    //First parameter is the number of the player. Second is the damage to deal.
    void dealDamage(float damage, int playerNum)
    {
        health -= damage;
        if (health < 0)
            health = 0;
        switch(playerNum)
        {
            case 1:
                p1Dmg += damage;
                break;
            case 2:
                p2Dmg += damage;
                break;
        }
    }

    //Slows velocity gradually if within slow distance
    float GetMoveSpeed()
    {
        return (distTo >= slowDist ? velocity : Mathf.Lerp(0.0F, velocity, distTo / slowDist));
    }

    //Slows rotation slowly if closer to desired rotation
    float GetRotSpeed()
    {
        return (rotRemaining >= slowRot ? rotation : Mathf.Lerp(0.0F, rotation, rotRemaining / slowRot));
    }
}
