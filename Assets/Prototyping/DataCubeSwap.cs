using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCubeSwap : MonoBehaviour {

    [SerializeField] private GameObject cubeHolder;
    [SerializeField] private bool hasCube = false;
    private GameObject dataCube;

    // Use this for initialization
    void Start () {
        dataCube = GameObject.FindGameObjectWithTag("dataCube");
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(hasCube == true)
            {
                hasCube = false;
            }
            else if(hasCube == false)
            {
                hasCube = true;
            }
        }
        if()

		if(hasCube == true)
        {
            dataCube.transform.position = cubeHolder.transform.position;
        }
	}
}
