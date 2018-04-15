using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBlink : MonoBehaviour {

    [SerializeField] private Renderer[] meshes;
    [SerializeField] private float speed = 0.5f;
    private float timer;

	// Use this for initialization
	void Start () {
        timer = speed;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            foreach(Renderer x in meshes)
            {
                x.enabled = !x.enabled;
            }
            timer = speed;
        }
	}
}
