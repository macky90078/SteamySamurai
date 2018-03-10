using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PauseGame : MonoBehaviour {

    [SerializeField] private int playerID;
    [SerializeField] private Transform pauseCanvas;
    private GameObject pauseMenu;
    private Player player;

	// Use this for initialization
	void Start () {
        player = ReInput.players.GetPlayer(playerID);
        pauseMenu = pauseCanvas.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		if(player.GetButtonDown("Pause"))
        {
            if (pauseMenu.activeInHierarchy == true)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            if (pauseMenu.activeInHierarchy == false)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }
	}
}
