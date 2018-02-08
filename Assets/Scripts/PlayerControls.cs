using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour
{
    public int m_PlayerNumber;
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;
    public float m_damping = 10.0f;

    private string m_MovementAxisName;
    private string m_TurnAxisHorizontalName;
    private string m_TurnAxisVerticalName;
    private Rigidbody m_Rigidbody;
    private float m_MovementInputValue;
    private float m_TurnInputXValue;
    private float m_TurnInputYValue;
    private float m_LookAngleInDegrees;

    private Transform heldWeapon;

    private string currentItem = string.Empty;

    [SerializeField]
    public GameObject moonMirror;

    [SerializeField]
    private AudioManager audioManager;

    private bool isUpdating = false;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // called after awake but before updates
    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
    }

    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }

    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisHorizontalName = "RHorizontal" + m_PlayerNumber;
        m_TurnAxisVerticalName = "RVertical" + m_PlayerNumber;
    }

    private void Update()
    {
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputXValue = Input.GetAxis(m_TurnAxisHorizontalName);
        m_TurnInputYValue = Input.GetAxis(m_TurnAxisVerticalName);

		if(Input.GetButtonDown("UseItem1"))
        {
        	this.UseItemMoon();
        }
		if(Input.GetButtonDown("UseItem2"))
		{
			this.UseItemSun();
		}
		if(Input.GetButtonUp("UseItem2"))
		{
			this.ResetIsPulling();
		}

		if(this.moonMirror != null && this.moonMirror.activeSelf && this.gameObject.tag == "Moon")
		{
			if(!isUpdating)
			{
				this.isUpdating = true;
				this.StartCoroutine(this.UpdateOnMirrorUp());
			}

		}
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        // Adjust the position of player
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        // handling the rotation of the player
        if (m_TurnInputXValue != 0.0f || m_TurnInputYValue != 0.0f)
        {
            m_LookAngleInDegrees = Mathf.Atan2(m_TurnInputYValue, m_TurnInputXValue) * Mathf.Rad2Deg;
            Quaternion eulerAngle = Quaternion.Euler(0.0f, m_LookAngleInDegrees, 0.0f);
            m_Rigidbody.rotation = Quaternion.Lerp(m_Rigidbody.rotation, eulerAngle, Time.deltaTime * m_damping);
        }
    }

    private void UseItemMoon()
    {
		if(this.gameObject.tag != "Moon")
    		return;

    	if(currentItem == "Amulet")
    	{
    		this.moonMirror.SetActive(!this.moonMirror.activeSelf);
			GameObject.FindGameObjectWithTag("SpawnedBeam").GetComponent<LightBeam>().StartCoroutine(GameObject.FindGameObjectWithTag("SpawnedBeam").GetComponent<LightBeam>().DrawLightBeam());

    		Debug.Log("Amulet Moon");
    	}
    	else if (currentItem == "Staff")
    	{
			this.moonMirror.SetActive(false);
			this.audioManager.PlayFreeze();
			this.GetComponent<FreezeRay>().UseFreeze();
			Debug.Log("Staff Moon");
    	}
    }

	private void UseItemSun()
    {
    	if(this.gameObject.tag != "Sun")
    		return;

    	if(currentItem == "Amulet")
    	{
    		Debug.Log("Amulet Sun");

			this.GetComponent<PullObject>().SetIsPulling(!this.GetComponent<PullObject>().GetIsPulling());
			this.GetComponent<PullObject>().StartCoroutine(this.GetComponent<PullObject>().PullObjectIn());

			this.audioManager.PlayPull();
    	}
    	else if (currentItem == "Staff")
    	{
			Debug.Log("Staff Sun");
			this.audioManager.PlaySmash();
			this.GetComponent<Smash>().UseSmash(this.transform.forward);
    	}
    }

    private void ResetIsPulling()
    {
		if(this.gameObject.tag != "Sun")
    		return;

    	if(currentItem == "Amulet")
    	{
			this.GetComponent<PullObject>().SetIsPulling(false);
			GameObject.FindGameObjectWithTag("SpawnedBeam").GetComponent<LightBeam>().StartCoroutine(GameObject.FindGameObjectWithTag("SpawnedBeam").GetComponent<LightBeam>().DrawLightBeam());
    	}
    }

    public void setCurrentItem(string name)
    {
    	this.currentItem = name;
    }

    public IEnumerator UpdateOnMirrorUp()
    {
		GameObject.FindGameObjectWithTag("SpawnedBeam").GetComponent<LightBeam>().StartCoroutine(GameObject.FindGameObjectWithTag("SpawnedBeam").GetComponent<LightBeam>().DrawLightBeam());
   		yield return new WaitForSeconds(0.5f);
		this.isUpdating = false;
    }
}