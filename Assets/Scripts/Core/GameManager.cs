using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //Public variables
    public static string NextScene;
    public static int levelMask = LayerMask.GetMask("levelMask"); //Level architecture and obstacles
    public static int enemyMask = LayerMask.GetMask("enemyMask"); //Enemies

    // Prevent manager from being destroyed between scenes
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartLoadingScreen()
    {
        SceneManager.LoadScene("loadingScreen");
    }


    //Assigns the int value of all the masks that are used in the game
    void GetMasks()
    {
        levelMask = LayerMask.GetMask("levelMask");
        enemyMask = LayerMask.GetMask("enemyMask");
    }

    public string GetNextScene() { return NextScene; }
    public void SetNextScene(string nextSceneName) { NextScene = nextSceneName; }
}
