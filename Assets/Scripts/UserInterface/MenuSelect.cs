using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Rewired;

public class MenuSelect : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject selectedButton;
    private bool buttonSelected;
    Player player;

	// Use this for initialization
	void Start () {
        player = ReInput.players.GetPlayer(0);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(player.GetAxisRaw("UIVertical") != 0 && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedButton);
            buttonSelected = true;
        }
	}

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
