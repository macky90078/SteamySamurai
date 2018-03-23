using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBeam : MonoBehaviour {

	//[SerializeField]
	//private float maxDistance = 25.0f;

	[SerializeField]
	private LineRenderer lineRenderer;

	//[SerializeField]
	//private float lightSpeed = 5.0f;

	//private Vector3 nextPos = Vector3.zero;
	//private Vector3 nextNormal = Vector3.zero;

	//private int startIndex = 0;

	//private bool hasHitPoint = false;


	//[SerializeField]
	//private float updateFreq = 0.1f;
	[SerializeField]
	private float lightDistance = 25.0f;
	[SerializeField]
	private string bounceTag = "bounce";
	[SerializeField]
	private string splitTag = "split";
	[SerializeField]
	private string spawnedBeamTag = "spawned beam";
	[SerializeField]
	private int maxBounce = 5;
	[SerializeField]
	private int maxSplit = 5;
	//private float timer = 0;

	private bool loopActive = false;
	private bool isDrawing = false;

	private IEnumerator Start()
	{
		this.StartCoroutine(this.DrawLightBeam());
		yield return null;
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			this.StartCoroutine(this.DrawLightBeam());
		}
	}	

	public IEnumerator DrawLightBeam()
	{
		yield return new WaitForEndOfFrame();

		if(this.isDrawing)
			yield break;

		Debug.Log("Draw called");
		this.isDrawing = true;

		if(this.loopActive)
			yield break;

		Debug.Log("looping");

		int lightSplit = 1;
		int lightReflected = 1;
		int vertexCount = 1;

		loopActive = true;

		Vector3 lightDirection = this.transform.forward;
		Vector3 lightLastPosition = this.transform.localPosition;

		this.lineRenderer.SetVertexCount(1);
		this.lineRenderer.SetPosition(0, this.transform.position);

		RaycastHit hit;

		while(loopActive)
		{
			if(Physics.Raycast(lightLastPosition, lightDirection, out hit, this.lightDistance) &&
				(hit.transform.gameObject.tag == bounceTag || hit.transform.gameObject.tag == splitTag || hit.transform.tag == "moonMirror"))
			{
				Debug.Log("Bounce");
				lightReflected++;
				vertexCount += 3;
				lineRenderer.SetVertexCount(vertexCount);
				lineRenderer.SetPosition(vertexCount - 3, Vector3.MoveTowards(hit.point, lightLastPosition, 0.01f));
				lineRenderer.SetPosition(vertexCount - 2, hit.point);
				lineRenderer.SetPosition(vertexCount - 1, hit.point);
				lineRenderer.SetWidth(0.1f, 0.1f);

				lightLastPosition = hit.point;
				Vector3 prevDirection = lightDirection;
				lightDirection = Vector3.Reflect(lightDirection, hit.normal);

				if(hit.transform.tag == splitTag)
				{
					if(lightSplit >= maxSplit)
					{

					}
					else
					{
						lightSplit++;
						GameObject clone = Instantiate(gameObject, hit.point, Quaternion.LookRotation(prevDirection));
						clone.name = spawnedBeamTag;
						((GameObject)clone).tag = spawnedBeamTag;
					}
				}
			}
			else
			{
				lightReflected++;
				vertexCount++;
				lineRenderer.SetVertexCount(vertexCount);

				//Vector3 lastPos = lightLastPosition + (lightDirection * lightDistance);

				lineRenderer.SetPosition(vertexCount - 1, lightLastPosition + (lightDirection * lightDistance));

				loopActive = false;
			}

			if(lightReflected > maxBounce)
			{
				loopActive = false;
			}

			this.isDrawing = false;
			yield return new WaitForEndOfFrame();
		}
	}
}
