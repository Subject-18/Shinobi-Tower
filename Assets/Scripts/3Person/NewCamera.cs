using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCamera : MonoBehaviour
{
    
    public float minDistance = 1.0f;
	public float maxDistance = 4.0f;
	public float smooth = 10.0f;
	Vector3 dollyDir;
	public Vector3 dollyDirAdjusted;
	public float newdistance;

	public float dis_ray;

	// Use this for initialization
	void Awake()
	{
		dollyDir = transform.localPosition.normalized;
		newdistance = transform.localPosition.magnitude;
	}

	// Update is called once per frame
	void Update()
	{

		Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
		RaycastHit hit;

		if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit))
		{
			newdistance = Mathf.Clamp((hit.distance * dis_ray), minDistance, maxDistance);

		}
		else
		{
			newdistance = maxDistance;
		}

		transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * newdistance, Time.deltaTime * smooth);
	}



}
