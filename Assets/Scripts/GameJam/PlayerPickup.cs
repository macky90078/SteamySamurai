using UnityEngine;
using System.Collections;

public class PlayerPickup : MonoBehaviour
{
    public bool hasWeapon;
    public Transform weaponHoldPoint;

    private WeaponManager weaponManagerScript;
    public GameObject weaponManager;

    private Transform weaponToThrowTransform;

    [SerializeField]
    private float launchAngle = 45.0f;

    private bool isLaunching = false;

    private void Awake()
    {
        hasWeapon = false;
    }

    private void Start()
    {
        weaponManagerScript = weaponManager.GetComponent<WeaponManager>();
    }

    private void Update()
    {
		if(Input.GetButtonDown("SwapWeapon1"))
        {
			if(hasWeapon)
			{
				if (this.gameObject.tag == "Moon")
	            {
					Debug.Log("swap weapon 1");
					weaponManagerScript.moonWantsToSwitch = !weaponManagerScript.moonWantsToSwitch;
	            }
	           	
				if (weaponManagerScript.sunWantsToSwitch && weaponManagerScript.moonWantsToSwitch)
		        {
		            // transmit weapon call here
		            Debug.Log("script 1");
		            TrasmitWeapon();

		        }
			}
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Weapon")
        {
            if(!hasWeapon)
            {
                GrabWeapon(other.gameObject);
            }          
        }
    }

    private void GrabWeapon(GameObject weapon)
    {
        hasWeapon = true;
        weapon.transform.position = weaponHoldPoint.transform.position;
        weapon.transform.parent = transform;

		this.GetComponent<Rigidbody>().velocity = Vector3.zero;

		this.weaponToThrowTransform = weapon.transform;
        weapon.GetComponent<CapsuleCollider>().enabled = false;
		this.weaponToThrowTransform.GetComponent<Rigidbody>().useGravity = false;
		this.weaponToThrowTransform.GetComponent<Rigidbody>().isKinematic = true;

		this.GetComponent<PlayerControls>().setCurrentItem(this.weaponToThrowTransform.GetComponent<Weapon>().GetWeaponName());
    }

    // throw weapon
    public void TrasmitWeapon()
    {
        if(hasWeapon && !isLaunching)
        {
        	Debug.Log("call transmit 1");
        	this.isLaunching = true;

			GameObject.FindGameObjectWithTag("Sun").GetComponent<PlayerPickup2>().TrasmitWeapon();

			this.GetComponent<PlayerControls>().moonMirror.SetActive(false);

            hasWeapon = false;          
            // rest of throw physics here

            Vector3 launchPos = this.transform.localPosition;
           	Vector3 targetPos = GameObject.FindGameObjectWithTag("Sun").transform.localPosition;

			launchPos.y = this.transform.position.y + this.weaponToThrowTransform.lossyScale.y; // MAGIC NUMBERS YEAAAAAAAAAAAAAAAAAH

			targetPos.y = launchPos.y;

			this.weaponToThrowTransform.position = launchPos;

			weaponToThrowTransform.LookAt(targetPos);

			float distance = Vector3.Distance(launchPos, targetPos);
			float initialVel = Mathf.Sqrt((distance * -Physics.gravity.y) / (Mathf.Sin(Mathf.Deg2Rad * this.launchAngle * 2)));

			float yVel = initialVel * Mathf.Sin(Mathf.Deg2Rad * this.launchAngle);
			float zVel = initialVel * Mathf.Cos(Mathf.Deg2Rad * this.launchAngle);

			Vector3 yVelocity = yVel * weaponToThrowTransform.transform.up;
			Vector3 zVelocity = zVel * weaponToThrowTransform.transform.forward;

			Vector3 velocity = zVelocity + yVelocity;

			this.weaponToThrowTransform.GetComponent<Rigidbody>().useGravity = true;
			this.weaponToThrowTransform.GetComponent<Rigidbody>().isKinematic = false;
			this.weaponToThrowTransform.GetComponent<Rigidbody>().velocity = velocity;

			this.GetComponent<Rigidbody>().velocity = Vector3.zero;

			this.StartCoroutine(this.ResetWeapon());

			this.isLaunching = false;
        }
    }

	public IEnumerator ResetWeapon()
	{
		yield return new WaitForSeconds(0.15f);

		weaponManagerScript.moonWantsToSwitch = false;
		this.weaponToThrowTransform.parent = null;
		this.weaponToThrowTransform.GetComponent<CapsuleCollider>().enabled = true;

		yield return null;
	}
}
