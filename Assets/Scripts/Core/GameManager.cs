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
    [HideInInspector] public int[] playerIds;
    [HideInInspector] public int buffIndex = 0;//+1 each time a player selects, moves to next player
    [HideInInspector] public int scrapMetal = 0;
    [HideInInspector] public int waveScrap = 0;
    [HideInInspector] public bool hasAttackBuff = false;
    [HideInInspector] public bool hasMoveBuff = false;

    public int[] samuraiPerWave;
    public int[] ninjasPerWave;
    [HideInInspector] public int samSpawned = 0;
    [HideInInspector] public int ninSpawned = 0;
    [HideInInspector] public int currWave = 0;

    //Private variables
    private int enemiesPerWave;
    private int numOfWaves;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        GetMasks();
    }

    // Use this for initialization
    void Start()
    {
        reference = gameObject.GetComponent<GameManager>();
        InitVars();
        ChangeState(gameState.mainMenu);
    }

    void InitVars()
    {
        playerIds = ReInput.players.GetPlayerIds();
        hasAttackBuff = false;
        hasMoveBuff = false;
        enemiesSpawned = 0;
        scrapMetal = 0;
        spawnEnemies = false;
        currWave = 1;
        waveScrap = 0;
        numOfWaves = samuraiPerWave.Length;
        enemiesPerWave = 0;
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
        //Debug.Log("Entering " + state);
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
    //Sets the map of player on or off, based on the bool. Categorys can be found in input manager editor
    void SetMap(int id, bool mapState, string category)
    {
        ReInput.players.GetPlayer(id).controllers.maps.SetMapsEnabled(mapState, category);
    }

    //--- Playing State---//
    //Starts a wave, and the main game loop
    public void StartWave()
    {
        samSpawned = 0;
        ninSpawned = 0;
        enemiesSpawned = 0;
        waveScrap = 0;
        enemiesPerWave = ninjasPerWave[currWave - 1] + samuraiPerWave[currWave - 1];
        spawnEnemies = true;
        ChangeState(gameState.playing);
    }

    //Update loop while playing in the wave, main game loop
    void CheckPlayState()
    {
        enemiesSpawned = ninSpawned + samSpawned;
        if (enemiesSpawned == enemiesPerWave)//Limit enemies per wave
            spawnEnemies = false;
        if (currWave >= numOfWaves && waveScrap == enemiesPerWave)//Check if game won, at max wave and all enemies defeated
            ChangeState(gameState.gameWon);
        else if (waveScrap == enemiesPerWave)//Check if all enemies have been killed this wave
        {
            currWave++;
            enemiesPerWave = ninjasPerWave[currWave - 1] + samuraiPerWave[currWave - 1];
            ChangeState(gameState.buffSelect);
        }
    }

    //----- Buff State -----//
    void SetupBuffSelect()
    {
        GameObject.FindGameObjectWithTag("CanvasContainer").GetComponent<CanvasRef>().buffCanvas.SetActive(true);
    }

    void GameOver(bool won)
    {
        InitVars();
        if(won == true)
        {
            GameObject.FindGameObjectWithTag("CanvasContainer").GetComponent<CanvasRef>().winCanvas.SetActive(true);
        }
        else
        {
            GameObject.FindGameObjectWithTag("CanvasContainer").GetComponent<CanvasRef>().loseCanvas.SetActive(true);
        }
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
