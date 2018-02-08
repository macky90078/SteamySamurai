using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    private GameObject m_hitEnemy = null;
    [SerializeField] private Material m_mattFrozen;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseFreeze();
            Debug.Log("INPUT");
        }
    }

    private void UseFreeze()
    {
        RayCastCheck(transform.forward);
    }

    private void RayCastCheck(Vector3 rayDirection)
    {
        Ray ray = new Ray(transform.position, rayDirection);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f))
        {
            if (hit.transform.tag == "enemy")
            {
                Renderer enemyColour;
                enemyColour = hit.transform.gameObject.GetComponent<Renderer>();
                enemyColour.material = m_mattFrozen;
            }
        }

    }

}
