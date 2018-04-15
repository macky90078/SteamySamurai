using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour {

    private Quaternion rotation;

    private void Awake()
    {
        rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update () {
        transform.rotation = rotation;
	}
}
