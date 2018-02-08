using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForPlayer : MonoBehaviour {

    public bool m_bPlayerInRange = false;

    [SerializeField] private GameObject m_parentRefObj = null;
    [SerializeField] private GameObject m_player1Obj = null;
    [SerializeField] private GameObject m_player2Obj = null;

    public float m_player1Dist;
    public float m_player2Dist;

    public GameObject m_closestPlayer = null;

    // Use this for initialization
    void Start ()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        m_player1Dist = Vector3.Distance(transform.position, m_player1Obj.transform.position);
        m_player2Dist = Vector3.Distance(transform.position, m_player2Obj.transform.position);

        if(m_player1Dist <= 18 || m_player2Dist <= 18)
        {
            m_bPlayerInRange = true;
        }
        else
        {
            m_bPlayerInRange = false;
        }

        if(m_bPlayerInRange)
        {
            if(m_player1Dist < m_player2Dist)
            {
                m_closestPlayer = m_player1Obj;
            }
            else if(m_player2Dist < m_player1Dist)
            {
                m_closestPlayer = m_player2Obj;
            }

            m_parentRefObj.GetComponent<Unit>().target = m_closestPlayer.transform;
        }
    }
}
