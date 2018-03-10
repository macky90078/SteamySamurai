using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

    [SerializeField] private Text loadingText;

    private int dots = 0;
    private float timer = 0.3f;

    // Use this for initialization
    void Start () {
        Time.timeScale = 1;
        StartCoroutine(LoadNewScene());
    }
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            switch (dots)
            {
                case 0:
                    loadingText.text = "Loading\n .";
                    dots++;
                    break;
                case 1:
                    loadingText.text = "Loading\n . .";
                    dots++;
                    break;
                case 2:
                    loadingText.text = "Loading\n . . .";
                    dots = 0;
                    break;
            }
            timer = 0.3f;
        }
    }

    IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(3);

        AsyncOperation async = SceneManager.LoadSceneAsync(GameManager.NextScene);//GameManager.NextScene);

        while (!async.isDone)
        {
            yield return null;
        }
    }
}
