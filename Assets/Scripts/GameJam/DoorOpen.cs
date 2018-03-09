using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public void OpenDoor()
    {
        this.transform.position -= new Vector3(0.0f, 5.0f * Time.deltaTime, 0.0f);
        Destroy(gameObject, 3.5f);
    }
}
