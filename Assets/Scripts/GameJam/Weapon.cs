using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	[SerializeField]
	private string weaponName = "";

	private void Start()
	{
		GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");

		for(int i = 0; i < weapons.Length; i++)
		{
			Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), weapons[i].GetComponent<Collider>());
		}
	}

	public string GetWeaponName()
	{
		return this.weaponName;
	}
}
