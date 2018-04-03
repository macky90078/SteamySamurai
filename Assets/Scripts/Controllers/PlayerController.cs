﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
public class PlayerController : MonoBehaviour
{

    /*
     * This class can later be used for controlling the player and otherwise. 
    For now, I've decided not to edit the existing PlayerController.cs file.
    There is also setup blocked out for player looking, not sure how it will be implemented.

    In order to ajust any of the control maps, you will need to edit the 'Rewired Input Manager' prefab or else nothing will sync. Also, the input manager doesn't
    transfer from scene to scene, so you'll need to add new prefabs to each scene                                                                
    */

    public GameManager gameManager;

    //Rewired critical variables
    public int playerId = 0; //the player controller ID. I've set up two controllers, one for each player. 0 is player one, 1 is player two
    private Player player; //getting the instance of the player as held by the input manager

    //movement related variables
    private Vector3 moveVector;
    private Vector2 lookVector;

    public Quaternion eulerAngle;

    //Look related variables
    public bool isLookInverted = false;
    //private bool select;
    private bool attacking;
    [SerializeField] private GameObject cam;
    //private float maxTilt = 45f;
    //private float minTilt = 45f; //to stop player from looking up in circles

    [SerializeField] private float moveSpeed = 10.0f; //how fast player traverses the terrain
    //private float lookSpeed = 20.0f; //how fast the player will look

    private float m_LookAngleInDegrees;

    public float m_damping = 10.0f;

    //game related variables
    public bool containsCube { get; set; }

    // Combat variables
    [SerializeField] private float maxHealth = 20;
    private float health;
    //Buffs
    [HideInInspector] public bool hasMoveBuff = false;
    [HideInInspector] public bool hasAttackBuff = false;

    private Vector3 forward;
    private GameObject target;
    private int enemyMask;


    [SerializeField] private bool meleePlayer;
    [SerializeField] private bool rangedPlayer;

    // Ranged
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject projectileSpawn;
    [SerializeField] private float fireRate = 0.3f;
    [SerializeField] private float rangedDamage = 1.0f;
    private float shootTimer = 0.0f;
    // Melee
    [SerializeField] private float meleeAttackRate = 0.5f;
    [SerializeField] private float meleeRange = 3.0f;
    [SerializeField] private float meleeDamage = 1.0f;
    private float meleeTimer = 0.0f;
    private Vector3 center;
    private float centerHeight = 0.7f;

    //Animations
    [HideInInspector] public Vector3 m_localVel;
    private Animator m_animator;

    [HideInInspector] public bool m_attackAnimationStart = false;
    [HideInInspector] public bool m_geishaAttackAnimationEnd = false;
    public bool m_canAttack = true;

    public Vector3 test1;
    public Vector3 test2;
    public bool isbackped = false;

    void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
        m_animator = gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        // Ensures player cant be both melee and ranged
        if (meleePlayer && rangedPlayer)
            meleePlayer = false;

        shootTimer = fireRate;
        enemyMask = GameManager.enemyMask;
        centerHeight = GetComponent<BoxCollider>().center.y;// Finds the center of the attached box collider, used for raycasting from center of player
        gameManager = GameManager.reference;
    }

    void GetInput()
    {
        moveVector.x = player.GetAxis("MoveHorizontal"); //the left stick on a controller, or WASD
        moveVector.z = player.GetAxis("MoveVertical");

        lookVector.x = player.GetAxis("LookHorizontal");
        lookVector.y = player.GetAxis("LookVertical");

        //select = player.GetButtonDown("Select"); //'a' button on a controller, or the return key
        attacking = player.GetButton("Attack"); // Right trigger
    }

    void ProcessInput()
    {
        Vector3 direction = new Vector3(moveVector.x, 0.0f, moveVector.z); //cam.transform.forward;
        //direction.y = 0;

        direction = cam.transform.TransformDirection(direction);
        direction.y = 0;

        lookVector = cam.transform.TransformDirection(lookVector);

        if (m_canAttack)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        m_localVel = transform.InverseTransformDirection(direction);

        //transform.Translate(direction * Time.deltaTime * moveSpeed); //moving the player

        if (lookVector.x != 0.0f || lookVector.y != 0.0f)
        {
            m_LookAngleInDegrees = Mathf.Atan2(lookVector.x, lookVector.y) * Mathf.Rad2Deg;
            eulerAngle = Quaternion.Euler(0.0f, m_LookAngleInDegrees, 0.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, eulerAngle, Time.deltaTime * m_damping);
        }
        else if (lookVector.x <= 0.0f || lookVector.y <= 0.0f)
        {
            m_LookAngleInDegrees = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            eulerAngle = Quaternion.Euler(0.0f, m_LookAngleInDegrees, 0.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, eulerAngle, Time.deltaTime * m_damping);
        }

        //Movement Animations
        if (containsCube)
        {
            m_animator.SetBool("HoldingCube", true);
        }

        // Combat
        forward = transform.TransformDirection(Vector3.forward);
        center = new Vector3(transform.position.x, transform.position.y + centerHeight, transform.position.z);
        // Ranged or melee depending on ID
        if (containsCube == false)
        {
            m_animator.SetBool("HoldingCube", false);

            if (attacking && rangedPlayer)
                shoot();
            else if (attacking && meleePlayer)
                melee();
        }

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        ProcessInput();
    }

    // -- Animation Event Functions
    public void Arrow()
    {
        Instantiate(projectile, projectileSpawn.transform.position, projectileSpawn.transform.rotation).GetComponent<TranslateProjectile>().SetDamage(rangedDamage, playerId);
    }

    public void Slash()
    {
        RaycastHit hit;
        if (Physics.Raycast(center, forward, out hit, meleeRange, enemyMask))
        {
            target = hit.transform.gameObject;
            target.GetComponent<SeekEnemy>().DealDamage(meleeDamage, playerId, false);
        }
        Debug.DrawRay(center, forward * meleeRange, Color.red);
        Debug.Log("Slash!");
    }

    // Shoots forward out of spawn every 'fireRate' amount of seconds
    void shoot()
    {
        if (m_canAttack)
        {
            m_canAttack = false;
            m_animator.SetTrigger("Attack");
        }
    }

    // Attack directly in front
    void melee()
    {
        if (m_canAttack)
        {
            m_canAttack = false;
            m_animator.SetTrigger("Attack");
        }
    }

    // Buff functions

    public void buffAttack(float inc)
    {
        if (hasAttackBuff == false)
        {
            if (meleePlayer)
                m_animator.SetFloat("AttackSpeed", inc);
            else if (rangedPlayer)
                m_animator.SetFloat("AttackSpeed", inc);
            GameManager.reference.StartWave();
        }
    }

    public void buffSpeed(float inc)
    {
        if (hasMoveBuff == false)
        {
            moveSpeed += inc;
            GameManager.reference.StartWave();
        }
    }

    public void buffHealth()
    {
        health = maxHealth;
        GameManager.reference.StartWave();
    }

    public void Die()
    {
        GameManager.reference.ChangeState(GameManager.gameState.gameOver);
    }
}
