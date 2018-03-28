using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //Public variables
    public enum gameState { mainMenu, playing, paused, buffSelect, gameOver, gameWon };//Add state to update when adding new state
    public static gameState currState;

    public static string NextScene;
    public static int levelMask; //Level architecture and obstacles
    public static int enemyMask; //Enemies
    [HideInInspector] public int enemiesSpawned;
    [HideInInspector] public bool spawnEnemies;
    [HideInInspector] public int enemiesKilled;

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
        ChangeState(gameState.playing);
        spawnEnemies = true;
    }

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

    void CheckPlayState()
    {
        if (currWave >= numOfWaves && enemiesKilled == enemiesPerWave)//Check if game won, at max wave and all enemies defeated
            ChangeState(gameState.gameWon);
        if (enemiesSpawned == enemiesPerWave)//Limit enemies per wave
            spawnEnemies = false;
        if (enemiesKilled == enemiesPerWave)//Check if all enemies have been killed this wave
            ChangeState(gameState.buffSelect);
    }

    //Assigns the int value of all the masks that are used in the game
    void GetMasks()
    {
        levelMask = LayerMask.GetMask("levelMask");
        enemyMask = LayerMask.GetMask("enemyMask");
    }

    void ChangeState(gameState state)
    {
        currState = state;
    }
    
    public void StartLoadingScreen()
    {
        SceneManager.LoadScene("loadingScreen");
    }

    public string GetNextScene() { return NextScene; }
    public void SetNextScene(string nextSceneName) { NextScene = nextSceneName; }
}
