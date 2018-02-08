using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorProgressManager : MonoBehaviour
{
    public bool door1Open = false;
    public bool door2Open = false;
    public bool door3Open = false;
    public bool door4Open = false;
    public bool door5Open = false;

    public GameObject door1;
    public GameObject door2;
    public GameObject door3;
    public GameObject door4;
    public GameObject door5;

    public GameObject staff;

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (door1Open)
        {
            door1.GetComponent<DoorOpen>().OpenDoor();
        }
        if (door2Open)
        {
            door2.GetComponent<DoorOpen>().OpenDoor();
        }
        if (door3Open)
        {
            door3.GetComponent<DoorOpen>().OpenDoor();
        }
        if (door4Open)
        {
            door4.GetComponent<DoorOpen>().OpenDoor();
        }
        if (door5Open)
        {
            door5.GetComponent<DoorOpen>().OpenDoor();
        }
    }
}
