using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    [Tooltip("The name of the scene you want to load")]
        [SerializeField] private string sceneName;
    [Tooltip("The canvas to be removed when game resumed")]
        [SerializeField] private Transform pauseCanvas;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void loadScene()
    {
        GameManager.reference.ButtonVibrate();
        GameManager.NextScene = sceneName;
        SceneManager.LoadScene("LoadingScreen");
    }

    public void StartGame()
    {
        GameManager.reference.ButtonVibrate();
        GameManager.NextScene = sceneName;
        SceneManager.LoadScene("LoadingScreen");
        GameManager.reference.StartWave();
    }

    public void ResumeGame()
    {
        GameManager.reference.ButtonVibrate();
        GameManager.reference.ChangeState(GameManager.gameState.playing);
        Time.timeScale = 1;
        pauseCanvas.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        GameManager.reference.ButtonVibrate();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit ();
        #endif
    }

}
