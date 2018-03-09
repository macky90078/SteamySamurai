using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRay : MonoBehaviour {


    private GameObject m_hitEnemy = null;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseFreeze();
            Debug.Log("FREEZE");
        }
    }

    public void UseFreeze()
    {
        RayCastCheck(-transform.forward);
    }

    private void RayCastCheck(Vector3 rayDirection)
    {
        Ray ray = new Ray(transform.position, rayDirection);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f))
        {
            if (hit.transform.tag == "enemy")
            {
                Debug.Log("hit!!");
                hit.transform.gameObject.GetComponent<Unit>().m_isFrozen = true;
            }
        }

    }
}
