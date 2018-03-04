﻿using System.Collections;
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
	private bool select;
    private Camera cam;
    private float maxTilt = 45f;
    private float minTilt = 45f; //to stop player from looking up in circles

    private float moveSpeed = 10.0f; //how fast player traverses the terrain
    private float lookSpeed = 20.0f; //how fast the player will look

    void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
    }

    void GetInput()
	{
		moveVector.x = player.GetAxis("MoveHorizontal"); //the left stick on a controller, or WASD
		moveVector.z = player.GetAxis("MoveVertical"); //'a' button on a controller, or the return key

	    lookVector.x = player.GetAxis("LookHorizontal");
	    lookVector.y = player.GetAxis("LookVertical");

		select = player.GetButtonDown("Select");

	}

	void ProcessInput()
	{
		transform.Translate(moveVector * Time.deltaTime * moveSpeed); //moving the player
	}
	
	// Update is called once per frame
	void Update() 
	{
		GetInput();
		ProcessInput();
		
	}
}
