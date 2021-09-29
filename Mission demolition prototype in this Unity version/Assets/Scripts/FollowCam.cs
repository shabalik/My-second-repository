﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

	static public GameObject POI;

	[Header("Set Dynamically")]
	public float camZ;

	[Header("Set in inspector")]
	public float easing = 0.1f;
	public Vector2 MinXY = Vector2.zero;

	void Awake()
    {
		camZ = this.transform.position.z;
    }

	void FixedUpdate()
    {
		//if (POI == null) return;

		//Vector3 destination = POI.transform.position;
		Vector3 destination;
		if (POI == null)
        {
			destination = Vector3.zero;
        } else
        {
			destination = POI.transform.position;
			if (POI.tag == "Projectile")
            {
				if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
					POI = null;
					return;
                }
            }
        }
		destination.x = Mathf.Max(MinXY.x, destination.x);
		destination.y = Mathf.Max(MinXY.y, destination.y);
		destination = Vector3.Lerp(transform.position, destination, easing);
		destination.z = camZ;
		transform.position = destination;
		Camera.main.orthographicSize = destination.y + 10;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
 