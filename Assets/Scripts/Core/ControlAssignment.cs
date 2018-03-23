using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ControlAssignment : MonoBehaviour {

    [SerializeField] private int numOfPlayers = 2;
    private int count = 0;

    void Awake()
    {
        foreach (Controller x in ReInput.controllers.Controllers)
        {
            if (x.enabled == false)
            {
                x.enabled = true;
                Debug.Log(x.name + " enabled");
            }
            if (x.type == ControllerType.Joystick && x.enabled == true)
            {
                ReInput.players.GetPlayer(count).controllers.AddController(x, true);
                Debug.Log("David - Controller " + x.name + " assigned to player ID " + count);
                count++;
                if (count == numOfPlayers)
                    break;
            }
        }
    }

}
