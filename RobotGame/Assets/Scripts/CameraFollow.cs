using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour {

	public GameObject target;
	public float angle = 30.0f;
	public float minDist = 2.0f;
	public float maxDist = 5.0f;
	public Vector3 offset = Vector3.zero;

	private float dist;
	private float angleOffset = 0.0f;
	private int colCount = 0;

	// Use this for initialization
	void Start () {
		dist = maxDist;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (colCount > 0) {
				if (dist > minDist) {
					dist -= 3 * Time.deltaTime;
					if (dist < minDist)
						dist = minDist;
				}
				if (angleOffset + angle < 88) {
					angleOffset += 180.0f * Time.deltaTime;
					if (angleOffset + angle > 88)
						angleOffset = 88 - angle;
				}
		} 
		else if(!Physics.Raycast (transform.position, transform.forward, dist - minDist) || Physics.OverlapSphere (transform.position, 1.2f).Length > 1) 
		{
			if(dist < maxDist)
			{
				dist += 3 * Time.deltaTime;
				if(dist > maxDist) dist = maxDist;
			}
			if (angleOffset > 0) {
				angleOffset -= 180.0f * Time.deltaTime;
				if (angleOffset < 0)
					angleOffset = 0.0f;
			}
		}

		float rads = (angle + angleOffset) * Mathf.Deg2Rad;
		Vector3 localOffset = offset.x * target.transform.right + 
							offset.y * target.transform.up + 
							offset.z * target.transform.forward;

		Vector3 newPos = -target.transform.forward * Mathf.Cos (rads) + target.transform.up * Mathf.Sin (rads);
		newPos = target.transform.position + localOffset + newPos * dist;
		transform.position = Vector3.Lerp(transform.position,newPos,0.5f);


		transform.LookAt (target.transform.position + localOffset);
		transform.eulerAngles -= transform.eulerAngles.z * transform.forward;
	
	}

	void OnTriggerEnter(Collider other)
	{
		++colCount;
	}

	void OnTriggerExit(Collider other)
	{
		--colCount;
	}


}
