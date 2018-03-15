using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  Rewired;

public class DataCubeSwapper : MonoBehaviour
{
    //critical rewired variables
    private Player[] players;
    private int numPlayers = 2;

    //shouldSwap button
    private bool[] swapButton; //the player's 'y' button
    private bool[] shouldSwap; //containing the bool for whether or not the player wants to swap the cube

    public GameObject[] playerPrefabs; //reference to both players
    public GameObject dataCube;
    private GameObject dataCubeInstance;



	// Use this for initialization
	void Awake ()
    {
        players = new Player[numPlayers];

        shouldSwap = new bool[numPlayers];
        playerPrefabs = new GameObject[numPlayers];
        swapButton = new bool[numPlayers];
        for (int i = 0; i < numPlayers; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);
            
        }
	}

    void Start()
    {

        playerPrefabs[0] = GameObject.FindGameObjectWithTag("Player"); //perhaps dangerous. But we'll deal with this
        dataCubeInstance = Instantiate(dataCube, playerPrefabs[0].transform.Find("AttachPoint").transform.position, Quaternion.identity);
        dataCubeInstance.transform.parent = playerPrefabs[0].transform;
        playerPrefabs[0].GetComponent<PlayerController>().containsCube = true; // default value
        playerPrefabs[1] = GameObject.FindGameObjectWithTag("Player2");
    }

    void GetInput()
    {
        for (int i = 0; i < numPlayers; i++)
        {
            swapButton[i] = players[i].GetButtonDown("Swap");
        }
    }

    void ProcessInput()
    {
        for (int i = 0; i < numPlayers; i++)
        {
            if (swapButton[i])
            {
                shouldSwap[i] = !shouldSwap[i];
                //Debug.Log("Player ID: " + i + ", " + shouldSwap[i]);
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		GetInput();
        ProcessInput();
        if (shouldSwap[0] && shouldSwap[1])
        {
            for (int i = 0; i < numPlayers; i++)
            {
                shouldSwap[i] = false;
            }

            if (playerPrefabs[0].GetComponent<PlayerController>().containsCube)
            {
                playerPrefabs[0].GetComponent<PlayerController>().containsCube = false;
                playerPrefabs[1].GetComponent<PlayerController>().containsCube = true;
                dataCubeInstance.transform.parent = playerPrefabs[1].transform;
                dataCubeInstance.transform.position = playerPrefabs[1].transform.Find("AttachPoint").transform.position; //inefficient way to do this. Should replace
            }
            else
            { 
                playerPrefabs[1].GetComponent<PlayerController>().containsCube = false;
                playerPrefabs[0].GetComponent<PlayerController>().containsCube = true;
                dataCubeInstance.transform.parent = playerPrefabs[0].transform;
                dataCubeInstance.transform.position = playerPrefabs[0].transform.Find("AttachPoint").transform.position;
            }

        }
	}
}
