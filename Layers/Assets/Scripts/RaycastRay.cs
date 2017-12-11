using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastRay : MonoBehaviour {

	public float weaponRange = 25f;

	private Camera fpsCamera;

	void Start () {
		fpsCamera = GetComponentInParent<Camera> ();
	}
	
	void Update () {
		Vector3 rayOrigin = fpsCamera.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0)); // Centered on screen
		Debug.DrawRay(rayOrigin, fpsCamera.transform.forward * weaponRange, Color.green);
	}
}
