using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class GameManager : MonoBehaviour {

    //Public variables
    public static GameManager reference;
    public enum gameState { mainMenu, playing, paused, buffSelect, gameOver, gameWon };//Add state to update when adding new state
    public gameState currState;

    public static string NextScene;
    public static int levelMask; //Level architecture and obstacles
    public static int enemyMask; //Enemies
    [HideInInspector] public int enemiesSpawned;
    [HideInInspector] public bool spawnEnemies;
    [HideInInspector] public int enemiesKilled;
    [HideInInspector] public int[] playerIds;
    [HideInInspector] public int buffIndex = 0;//+1 each time a player selects, moves to next player

    //Private variables
    [SerializeField] private int enemiesPerWave;
    [SerializeField] private int numOfWaves;
    private int currWave;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        GetMasks();
    }

    // Use this for initialization
    void Start()
    {
        reference = gameObject.GetComponent<GameManager>();
        playerIds = ReInput.players.GetPlayerIds();
        ChangeState(gameState.mainMenu);
    }

    //***** GAME LOOP *****//
    // Update is called once per frame
    void Update()
    {
        //Put least likely state at bottom, most likely at top
        switch(currState)
        {
            case gameState.playing:
                CheckPlayState();
                break;
            case gameState.buffSelect:
                CheckBuffState();
                break;
            case gameState.mainMenu:
                break;
            case gameState.gameOver:
                break;
            case gameState.gameWon:
                break;
            case gameState.paused:
                break;
        }
    }

    public void ChangeState(gameState state)
    {
        currState = state;
        SetControllerMap();
        switch(currState)
        {
            case gameState.playing:
                break;
            case gameState.buffSelect:
                SetupBuffSelect();
                break;
            case gameState.mainMenu:
                break;
            case gameState.gameOver:
                GameOver(false);
                break;
            case gameState.gameWon:
                GameOver(true);
                break;
            case gameState.paused:
                break;
        }
    }

    void SetControllerMap()
    {
        switch(currState)
        {
            case gameState.playing:
                foreach(int x in playerIds)
                {
                    SetMap(x, true, "Default");
                    SetMap(x, false, "Menu");
                }
                break;
            case gameState.buffSelect:
            case gameState.mainMenu:
            case gameState.gameOver:
            case gameState.gameWon:
            case gameState.paused:
                foreach (int x in playerIds)
                {
                    SetMap(x, false, "Default");
                    SetMap(x, true, "Menu");
                }
                break;
        }
    }

    void SetMap(int id, bool mapState, string category)
    {
        ReInput.players.GetPlayer(id).controllers.maps.SetMapsEnabled(mapState, category);
    }

    //--- Playing State---//
    //Starts a wave, and the main game loop
    public void StartWave()
    {
        spawnEnemies = true;
        ChangeState(gameState.playing);
    }

    //Update loop while playing in the wave, main game loop
    void CheckPlayState()
    {
        if (currWave >= numOfWaves && enemiesKilled == enemiesPerWave)//Check if game won, at max wave and all enemies defeated
            ChangeState(gameState.gameWon);
        if (enemiesSpawned == enemiesPerWave)//Limit enemies per wave
            spawnEnemies = false;
        if (enemiesKilled == enemiesPerWave)//Check if all enemies have been killed this wave
            ChangeState(gameState.buffSelect);
    }

    //----- Buff State -----//
    void SetupBuffSelect()
    {
        GameObject.FindGameObjectWithTag("BuffCanvas").SetActive(true);
        buffIndex = 0;
    }

    void CheckBuffState()
    {
        if (buffIndex > 1)//Need to change this so its dynamic, for possibility of more than one player
        {
            StartWave();
            GameObject.FindGameObjectWithTag("BuffCanvas").SetActive(false);
            buffIndex = 0;
        }
    }

    void GameOver(bool won)
    {
        enemiesSpawned = 0;
        spawnEnemies = false;
        enemiesKilled = 0;
    }

    //***** INTIALIZATION *****//
    //Assigns the int value of all the masks that are used in the game
    void GetMasks()
    {
        levelMask = LayerMask.GetMask("levelMask");
        enemyMask = LayerMask.GetMask("enemyMask");
    }

    public void StartLoadingScreen()
    {
        SceneManager.LoadScene("loadingScreen");
    }
}
