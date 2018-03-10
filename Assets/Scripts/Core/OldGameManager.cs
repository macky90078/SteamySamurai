using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldGameManager : MonoBehaviour {
    
    // THIS IS THE OLD GAME MANAGER FROM THE GAME JAM
    // I PUT IT IN THIS FILE TO PRESERVE THE CODE
    // -- DAVID LAFANTAISIE

    public Vector3 m_deadEnemyLastPos;
    public Quaternion m_deadEnemyLastRot;

    public bool m_enemyDied = false;

    [SerializeField]
    private GameObject m_shattedEnemyObj;
    [SerializeField]
    private Material m_mattFrozen;

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
}
