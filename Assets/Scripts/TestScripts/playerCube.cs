using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCube : MonoBehaviour {
    public bool hasCube;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Cube switched to other player.");

            if (hasCube == true)
                hasCube = false;
            else if (hasCube == false)
                hasCube = true;
        }
    }
}
