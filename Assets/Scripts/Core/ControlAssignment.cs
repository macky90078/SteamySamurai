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
            }
            if (x.type == ControllerType.Joystick && x.enabled == true)
            {
                ReInput.players.GetPlayer(count).controllers.AddController(x, true);
                count++;
                if (count == numOfPlayers)
                    break;
            }
        }
    }

}
