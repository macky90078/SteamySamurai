using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullObject : MonoBehaviour 
{
	[SerializeField]
	private float pullSpeed = 10.0f;

	[SerializeField]
	//private float pullRange = 10.0f;

	private bool isPulling = false;
	private bool isrunningCoroutine = false;

	private IEnumerator Start()
	{
		//this.StartCoroutine(this.PullObjectIn());
		yield return null;
	}

	public IEnumerator PullObjectIn()
	{
		if(this.isrunningCoroutine)
			yield break;

		this.isrunningCoroutine = true;

		GameObject[] pullableList = GameObject.FindGameObjectsWithTag("BounceSurface");
		Debug.Log("length: " + pullableList.Length.ToString());
		//List<Transform> pullableInfrontList = null;

		//check if object is infront of you
		/*
		if(pullableList != null)
		{
			for(int i = 0; i < pullableList.Length; i++)
			{
				Vector3 distance = this.transform.localPosition - pullableList[i].transform.localPosition;
				if(Vector3.Dot(distance, pullableList[i].transform.forward) < 0.0f)
				{
					Debug.Log("Add");
					if(pullableInfrontList == null)
					{
						pullableInfrontList = new List<Transform>();
					}

					pullableInfrontList.Add(pullableList[i].transform);
				}
			}
		}
		*/
		if(pullableList != null)
		{
			int closestIndex = 0;
			float closestDistance = Vector3.Distance(this.transform.localPosition, pullableList[0].transform.localPosition);

			// find the closest
			for(int i  = 0; i < pullableList.Length; i++)
			{
				float distance = Vector3.Distance(this.transform.localPosition, pullableList[i].transform.localPosition);

				if(distance <= closestDistance)
				{
					closestIndex = i;
					closestDistance = distance;
				}
			}

			Vector3 temp = this.transform.localPosition;
			temp.y = pullableList[0].transform.position.y;

			while(this.isPulling)
			{
				Debug.Log("is pulling");

				pullableList[closestIndex].transform.position = Vector3.Lerp(pullableList[closestIndex].transform.position, temp, this.pullSpeed * Time.deltaTime);

				if(Vector3.Distance(pullableList[closestIndex].transform.position, this.transform.localPosition) <= 1.25f)
				{
					this.isPulling = false;
				}

				yield return null;
			}
		}

		this.isrunningCoroutine = false;
		//this.StartCoroutine(this.PullObjectIn());
	}

	public void SetIsPulling(bool value)
	{
		this.isPulling = value;
	}

	public bool GetIsPulling()
	{
		return this.isPulling;
	}

}
