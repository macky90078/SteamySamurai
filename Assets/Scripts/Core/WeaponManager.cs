using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public bool moonWantsToSwitch;
    public bool sunWantsToSwitch;


    private void Start()
    {
        moonWantsToSwitch = false;
        sunWantsToSwitch = false;
    }
}
