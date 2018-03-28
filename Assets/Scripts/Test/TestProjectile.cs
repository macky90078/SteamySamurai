using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectile : MonoBehaviour {

    private Rigidbody m_rb;

    public GameObject m_projectile;

    public float force;

    // Use this for initialization
    void Start ()
    {
        m_rb = m_projectile.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.G))
        {
            force = CalculateMovementForce(10, .2f, 1);
            GameObject bullet = Instantiate(m_projectile, m_projectile.transform.position, m_projectile.transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(1000 * Vector3.forward);
        }
	}

    private float CalculateMovementForce(float distance, float time, float initVelocity)
    {
        float finalVelocity = CalculateFinalVelocity(distance, time, initVelocity);
        float acceleration = CalculateAcceleration(finalVelocity, initVelocity, time);
        return CalculateForce(m_rb.mass, acceleration);
    }

    float CalculateFinalVelocity(float dist, float time, float initVelocity)
    {
        return (dist / time) - initVelocity / 2;
    }
    float CalculateAcceleration(float finalVelocity, float initVelocity, float time)
    {
        return (finalVelocity - initVelocity) / time;
    }
    float CalculateForce(float mass, float acceleration)
    {
        return mass * acceleration;
    }
}
