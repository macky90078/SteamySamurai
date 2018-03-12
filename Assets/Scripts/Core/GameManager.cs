using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //GameJam variables
    public Vector3 m_deadEnemyLastPos;
    public Quaternion m_deadEnemyLastRot;

    public bool m_enemyDied = false;

    [SerializeField]
    private GameObject m_shattedEnemyObj;
    [SerializeField]
    private Material m_mattFrozen;

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
        //GetMasks();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_enemyDied)
        {
            GameObject tempEnemyShatter;

            tempEnemyShatter = Instantiate(m_shattedEnemyObj, m_deadEnemyLastPos, m_deadEnemyLastRot) as GameObject;

            MeshRenderer[] meshRenderers = tempEnemyShatter.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer g in meshRenderers)
            {
                g.material.color = m_mattFrozen.color;
            }

            m_enemyDied = false;
        }
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
