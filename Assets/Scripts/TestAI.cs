using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAI : MonoBehaviour {

    [HideInInspector] public bool m_isFrozen = false;
    [HideInInspector] public bool m_isDead = false;

    [SerializeField] private Material m_mattFrozen;
    [SerializeField] private Material m_mattNorm;

    [SerializeField] private GameObject m_changeMattObj;

    private float m_freezeCoolDown = 3f;

	// Update is called once per frame
	void Update ()
    {
        if (!m_isDead)
        {
            if (!m_isFrozen)
            {
                m_freezeCoolDown = 3f;
                m_changeMattObj.GetComponent<Renderer>().material.color = m_mattNorm.color;
            }
            else if (m_isFrozen)
            {
                m_freezeCoolDown -= Time.deltaTime;
                m_changeMattObj.GetComponent<Renderer>().material.color = m_mattFrozen.color;
                if (m_freezeCoolDown <= 0f)
                {
                    m_isFrozen = false;
                }
            }
        }

        if(m_isDead)
        {
            Destroy(gameObject);
        }

	}


}




