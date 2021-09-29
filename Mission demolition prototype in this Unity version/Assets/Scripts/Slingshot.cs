using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {

	static private Slingshot S;

	[Header("Set in inspector")]
	public GameObject prefabProjectile;
	public float velocityMult = 8f;

	[Header("Set dynamically")]
	public GameObject launchPoint;
	public Vector3 launchPos;
	public GameObject projectile;
	public bool aimingmode;

	private Rigidbody projectileRigidbody;

	static public Vector3 LAUNCH_POS
    {
		get
        {
			if (S == null)
            {
				return Vector3.zero;
            }
			return S.launchPos;
        }
    }

	void Awake()
    {
		S = this;
		Transform launchPointTrans = transform.Find("LaunchPoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive(false);
		launchPos = launchPointTrans.position;
    }

	void OnMouseEnter()
    {
		//print("Slingshot:OnMouseEnter()");
		launchPoint.SetActive(true);
    }

	void OnMouseExit()
    {
		//print("Slingshot:OnMouseExit()");
		launchPoint.SetActive(false);
    }

	void OnMouseDown()
    {
		aimingmode = true;
		projectile = Instantiate(prefabProjectile) as GameObject;
		projectile.transform.position = launchPos;
		projectileRigidbody = projectile.GetComponent<Rigidbody>();
		projectileRigidbody.isKinematic = true;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!aimingmode) return;

		Vector3 mousePos2D = Input.mousePosition;
		mousePos2D.z = - Camera.main.transform.position.z;
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

		Vector3 mouseDelta = mousePos3D - launchPos;
		float maxMagnitude = this.GetComponent<SphereCollider>().radius;

		if (mouseDelta.magnitude > maxMagnitude)
        {
			mouseDelta.Normalize();
			mouseDelta *= maxMagnitude;
        }

		Vector3 projPos = launchPos + mouseDelta;
		projectile.transform.position = projPos;

		if (Input.GetMouseButtonUp(0))
        {
			aimingmode = false;
			projectileRigidbody.isKinematic = false;
			projectileRigidbody.velocity = - mouseDelta * velocityMult;
			FollowCam.POI = projectile;
			projectile = null;
			MissionDemolition.ShotFired();
			ProjectileLine.S.poi = projectile;
        }
	}
}
