// Allows for camera orbit around the cube
// Credit: Star Space Era; https://youtu.be/bVo0YLLO43s
using UnityEngine;

public class CameraOrbit : MonoBehaviour {

	private Transform XForm_Camera;
	private Transform XForm_Parent;

	private Vector3 LocalRotation;
	private float CameraDistance = 20f;

	public float MouseSensitivity = 6f;
	public float ScrollSensitivity = 2f;
	public float OrbitDampening = 5f;
	public float ScrollDampening = 6f;

	public bool CameraDisabled = true;

	// Use this for initialization
	void Start () {
		this.XForm_Camera = this.transform;
		this.XForm_Parent = this.transform.parent;
	}
	
	// LateUpdate is called once per frame after Update()
	void LateUpdate () {
		if (Input.GetKey (KeyCode.LeftShift) && Input.GetMouseButton(0))
			CameraDisabled = false;
		else
			CameraDisabled = true;


		if (!CameraDisabled) {
			//Rotation of the Camera based on Mouse Coordinates
			if (Input.GetAxis ("Mouse X") != 0 || Input.GetAxis ("Mouse Y") != 0)
			{
				LocalRotation.x += Input.GetAxis ("Mouse X") * MouseSensitivity;
				LocalRotation.y -= Input.GetAxis ("Mouse Y") * MouseSensitivity;

				// Clamp the y Rotation to horizon and not flipping over at the top
				LocalRotation.y = Mathf.Clamp (LocalRotation.y, -90f, 90f);
			}
		}

		//Zooming input from our Mouse Scroll Wheel
		if (Input.GetAxis ("Mouse ScrollWheel") != 0f)
		{
			float ScrollAmount = Input.GetAxis ("Mouse ScrollWheel") * ScrollSensitivity;

			//Makes camera zoom faster the farther away it is from the target
			ScrollAmount *= (this.CameraDistance * 0.3f);

			this.CameraDistance += ScrollAmount * -1f;

			this.CameraDistance = Mathf.Clamp (this.CameraDistance, 8f, 100f);
		}

		//Actual Camera Rig Transformations
		Quaternion QT = Quaternion.Euler(LocalRotation.y, LocalRotation.x, 0);
		this.XForm_Parent.rotation = Quaternion.Lerp (this.XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

		if (this.XForm_Camera.localPosition.z != this.CameraDistance * -1f)
		{
			this.XForm_Camera.localPosition = new Vector3 (0f, 0f, Mathf.Lerp (this.XForm_Camera.localPosition.z, this.CameraDistance * -1f, Time.deltaTime * ScrollDampening));
		}
	}
}
