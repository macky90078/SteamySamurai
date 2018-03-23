using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
public class PlayerController : MonoBehaviour {

/*
 * This class can later be used for controlling the player and otherwise. 
For now, I've decided not to edit the existing PlayerController.cs file.
There is also setup blocked out for player looking, not sure how it will be implemented.

In order to ajust any of the control maps, you will need to edit the 'Rewired Input Manager' prefab or else nothing will sync. Also, the input manager doesn't
transfer from scene to scene, so you'll need to add new prefabs to each scene
		                                                            
                                                                   
*/

	//Rewired critical variables
	public int playerId = 0; //the player controller ID. I've set up two controllers, one for each player. 0 is player one, 1 is player two
	private Player player; //getting the instance of the player as held by the input manager

    //movement related variables
	private Vector3 moveVector;
    private Vector2 lookVector;

    //Look related variables
    public bool isLookInverted = false;
	//private bool select;
    private bool attacking;
    private Camera cam;
    //private float maxTilt = 45f;
    //private float minTilt = 45f; //to stop player from looking up in circles

    [SerializeField] private float moveSpeed = 10.0f; //how fast player traverses the terrain
    //private float lookSpeed = 20.0f; //how fast the player will look

    private float m_LookAngleInDegrees;

    public float m_damping = 10.0f;

    //game related variables
    public bool containsCube { get; set; }

    // Combat variables
    private Vector3 forward;
    private GameObject target;
    private int enemyMask;

    //This is temporary, only one should be selected true for now
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

    void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
    }

    private void Start()
    {
        // Ensures player cant be both melee and ranged
        if (meleePlayer && rangedPlayer)
            meleePlayer = false;

        shootTimer = fireRate;
        enemyMask = GameManager.enemyMask;
        centerHeight = GetComponent<BoxCollider>().center.y;// Finds the center of the attached box collider, used for raycasting from center of player
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
		transform.Translate(moveVector * Time.deltaTime * moveSpeed); //moving the player

        if(lookVector.x != 0.0f || lookVector.y != 0.0f)
        {
            m_LookAngleInDegrees = Mathf.Atan2(lookVector.x, lookVector.y) * Mathf.Rad2Deg;
            Quaternion eulerAngle = Quaternion.Euler(0.0f, m_LookAngleInDegrees, 0.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, eulerAngle, Time.deltaTime * m_damping);
        }


        // Combat
        forward = transform.TransformDirection(Vector3.forward);
        center = new Vector3(transform.position.x, transform.position.y + centerHeight, transform.position.z);
        // Ranged or melee depending on ID
        if(containsCube == false)
        {
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

    // Shoots forward out of spawn every 'fireRate' amount of seconds
    void shoot()
    {
        shootTimer -= Time.deltaTime;
        if(shootTimer <= 0)
        {
            shootTimer = fireRate;
            // Creates the projectile and assigns the damage it will do based on the players ranged damage
            Instantiate(projectile, projectileSpawn.transform.position, projectileSpawn.transform.rotation).GetComponent<TranslateProjectile>().SetDamage(rangedDamage, playerId);
        }
    }

    // Attack directly in front
    void melee()
    {
        meleeTimer -= Time.deltaTime;
        if(meleeTimer <= 0)
        {
            meleeTimer = meleeAttackRate;

            //Raycast to see if hit anything
            RaycastHit hit;
            if (Physics.Raycast(center, forward, out hit, meleeRange, enemyMask))
            {
                target = hit.transform.gameObject;
                target.GetComponent<SeekEnemy>().DealDamage(meleeDamage, playerId, false);
            }
            Debug.DrawRay(center, forward*meleeRange, Color.red);
        }
    }
}
