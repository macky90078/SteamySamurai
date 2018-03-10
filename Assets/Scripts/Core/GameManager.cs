﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Vector3 m_deadEnemyLastPos;
    public Quaternion m_deadEnemyLastRot;

    public bool m_enemyDied = false;

    [SerializeField]
    private GameObject m_shattedEnemyObj;
    [SerializeField]
    private Material m_mattFrozen;


    public static string NextScene;

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

    public string GetNextScene() { return NextScene; }
    public void SetNextScene(string nextSceneName) { NextScene = nextSceneName; }
}
