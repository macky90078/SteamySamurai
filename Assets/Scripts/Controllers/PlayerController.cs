using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour {

/*This class can later be used for controlling the player and otherwise. 
For now, I've decided not to edit the existing PlayerController.cs file.
		                                                -Andrew
*/

	//Rewired critical variables
	public int playerId = 0;
	private Player player;

	//movement related variables
	private Vector2 moveVector;
	private bool select;

    private float moveSpeed = 3.0f;

    void Awake () 
	{
		player = ReInput.players.GetPlayer(playerId);
	}

	void GetInput()
	{
		moveVector.x = player.GetAxis("MoveHorizontal"); //the left stick on a controller, or WASD
		moveVector.y = player.GetAxis("MoveVertical"); //'a' button on a controller, or the return key

		select = player.GetButtonDown("Select");

	}

	void ProcessInput()
	{
		transform.Translate(new Vector2(moveVector.x * Time.deltaTime * moveSpeed, moveVector.y * Time.deltaTime * moveSpeed));
	}
	
	// Update is called once per frame
	void Update() 
	{
		GetInput();
		ProcessInput();
		
	}
}
