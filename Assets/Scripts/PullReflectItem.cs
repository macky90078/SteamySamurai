using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullReflectItem : MonoBehaviour 
{
	private delegate void PlayerAction();

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			PlayerAction playerAction = this.gameObject.tag == "Sun" ?  new PlayerAction(this.Pull) : new PlayerAction(this.Reflect);
			playerAction();
		}
	}

	private void Pull()
	{
		// enable/disable pull child object
		Debug.Log("Pull Action");
	}

	private void Reflect()
	{
		// enable/disable mirror child object
		Debug.Log("Reflect Action");
	}
}
