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
                GameManager.reference.ChangeState(GameManager.gameState.playing);
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            if (pauseMenu.activeInHierarchy == false)
            {
                GameManager.reference.ChangeState(GameManager.gameState.paused);
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }
	}
}
