using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Rewired;

public class NavMeshEnemy : MonoBehaviour {

    // Public variables
    public bool beenHit = false;

    // Private Variables
    [SerializeField] private bool isNinja;
    [SerializeField] private BoxCollider meleeOneBox;
    [SerializeField] private BoxCollider meleeTwoBox;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Slider healthBar;
    [SerializeField] private states initialState;
    public float damage;
    private bool dead = false;
    private float health;
    private float p1Dmg; // Amount of damage done from each player
    private float p2Dmg; // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    private bool hasTarget = false;
    private Vector3 playerPos;
    private Vector3 targetPos;
    private Quaternion destRot;
    private GameObject targetObj;
    private enum states { seek, defense, charge, idle };

    private GameObject m_gPlayer1;
    private GameObject m_gPlayer2;
    [SerializeField] private GameObject scrapMetalPrefab;
    private bool charging = false;
    private bool inCombo;
    private float comboTimer;

    private float centerHeight = 0.7f;

    // -- Attack Stuff
    private Vector3 forward;
    private Vector3 center;
    private GameObject target;
    [SerializeField] private float meleeRange = 3.0f;
    [HideInInspector] public bool m_canAttack = true;
    [SerializeField] private float comboTimeLimit = 3.0f;
    private GameObject[] players;
    // -- Animations
    private Animator m_animator;

    //NavMesh stuff
    private NavMeshAgent agent;
    private Vector3 targetVec;
    private float distTo;
    [SerializeField] private float stopDist = 4.0f;
    [SerializeField] private float chargeAngle = 45.0f;
    [SerializeField] private float acceleration = 8.0f;
    [SerializeField] private float maxSpeed = 3.5f;
    [SerializeField] private float angularSpeed = 120.0f;
    [SerializeField] private float chargeDistance = 5.0f;
    [SerializeField] private float chargeAccel = 16.0f;
    [SerializeField] private float chargeMaxSpeed = 7.0f;
    [SerializeField] private float chargeAngularSpeed = 240.0f;

    private void Awake()
    {
        m_animator = gameObject.GetComponent<Animator>();
    }

    // Use this for initialization
    void Start()
    {
        InitVars();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
        CheckCombo();
        CheckDistance();
        if (distTo < agent.stoppingDistance && m_canAttack)
        {
            m_canAttack = false;
            m_animator.SetTrigger("Attack");
        }
        if (isNinja)
            CheckChargeDist();
        Move();

        forward = transform.TransformDirection(Vector3.forward);
        center = new Vector3(transform.position.x, transform.position.y + centerHeight, transform.position.z);
    }

    // Intialization of various variables.
    void InitVars()
    {
        ChangeState(initialState);
        health = maxHealth;
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stopDist;
        m_gPlayer1 = GameObject.FindGameObjectWithTag("Player");
        m_gPlayer2 = GameObject.FindGameObjectWithTag("Player2");
    }

    void ChangeState(states newState)
    {
        switch (newState)
        {
            case states.seek:
                targetObj = GameObject.FindGameObjectWithTag("dataCube");
                break;
            case states.defense:
                targetObj = GetHighPlayer();
                break;
            case states.idle:
                break;
        }
    }

    void Move()
    {
        if(!dead)
            agent.SetDestination(targetObj.transform.position);
        if(charging && CheckChargeCone())
        {
            agent.acceleration = chargeAccel;
            agent.speed = chargeMaxSpeed;
            agent.angularSpeed = chargeAngularSpeed;
        }
        else
        {
            agent.acceleration = acceleration;
            agent.speed = maxSpeed;
            agent.angularSpeed = angularSpeed;
        }
    }

    void CheckDistance()
    {
        targetVec = targetObj.transform.position - transform.position;
        distTo = targetVec.magnitude;
    }

    //********************************************************************************//
    //Checks which player did the most damage
    GameObject GetHighPlayer()
    {
        return (p1Dmg >= p2Dmg ? m_gPlayer1 : m_gPlayer2);
    }

    public void AttackPlayer()
    {
        //RaycastHit hit;
        //if (Physics.Raycast(center, forward, out hit, meleeRange, 12))
        //{
        //    target = hit.transform.gameObject;
        //    //target.GetComponent<SeekEnemy>().DealDamage(meleeDamage, playerId, false);
        //    Debug.Log(target);
        //}
        //Debug.DrawRay(center, forward * meleeRange, Color.red);
        Debug.Log("Slash!");
    }

    //Checks if target is within a cone in front of self
    //Cone measures charge angle from forward vector to both left and right
    bool CheckChargeCone()
    {
        //Draw cone based on charge angle
        Debug.DrawRay(transform.position, transform.forward * 100, Color.red);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, chargeAngle, 0) * transform.forward * 100, Color.red);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, -chargeAngle, 0) * transform.forward * 100, Color.red);


        return (Vector3.Angle(transform.forward, targetVec) < Mathf.Abs(chargeAngle));
    }

    // Check if within charge distance and start charging if so
    void CheckChargeDist()
    {
        if (distTo <= chargeDistance)
            charging = true;
        else if (agent.isStopped)
            charging = false;
        else
            charging = false;

        if (charging)
        {
            m_animator.SetBool("Sprint", true);
        }
        else
        {
            m_animator.SetBool("Sprint", false);
        }
    }

    // Switches to defense mode if health is less than OR equal to half of max health
    void CheckHealth()
    {
        healthBar.value = health / maxHealth;
        if (health <= 0)
            Die();
        else if (health <= maxHealth / 2)
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
    public void DealDamage(float damage, int playerID)
    {
        ReInput.players.GetPlayer(playerID).SetVibration(0, 0.75f, 0.1f);
        health -= damage;
        if (health < 0)
            health = 0;
        switch (playerID)
        {
            case 0:
                p1Dmg += damage;
                break;
            case 1:
                p2Dmg += damage;
                break;
        }
    }

    public void DealDamage(float damage, int playerID, bool ranged)
    {
        ReInput.players.GetPlayer(playerID).SetVibration(0, 0.75f, 0.1f);
        health -= damage;
        if (health < 0)
            health = 0;
        switch (playerID)
        {
            case 0:
                p1Dmg += damage;
                break;
            case 1:
                p2Dmg += damage;
                break;
        }
        if (!ranged)
        {
            StartCombo();
        }
        if (ranged && inCombo)
        {
            Die();
        }
    }

    void StartCombo()
    {
        comboTimer = comboTimeLimit;
        inCombo = true;
    }

    void CheckCombo()
    {
        if (inCombo)
        {
            m_animator.SetBool("CoreOut", true);
            comboTimer -= Time.deltaTime;
        }
        if (inCombo && comboTimer <= 0)
        {
            m_animator.SetBool("CoreOut", false);
            inCombo = false;
        }
    }

    void ActivateCollider()
    {
        meleeOneBox.enabled = true;
        if (isNinja)
            meleeTwoBox.enabled = false;
    }

    void DeactivateCollider()
    {
        meleeOneBox.enabled = false;
        if (isNinja)
            meleeTwoBox.enabled = false;
    }

    void Die()
    {
        dead = true;
        Instantiate(scrapMetalPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
