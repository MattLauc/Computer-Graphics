using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RaycastShoot : MonoBehaviour {

	public int gunDamage = 1;
	public float fireRate = 0.25f;
	public float weaponRange = 25f;
	public Transform gunEnd;
    public Text statText;
    public int cubits;

	private Camera fpsCamera;
	private WaitForSeconds shotDuration = new WaitForSeconds(.07f);
	private AudioSource gunAudio;
	private LineRenderer laserLine;
	private float nextFire;

    private string scoreText = "Cubits: ";

	void Start ()
	{
		laserLine = GetComponent<LineRenderer> ();
		gunAudio = GetComponent<AudioSource> ();
		fpsCamera = GetComponentInParent<Camera> ();
        statText.text = scoreText + "0\n";
	}
	
	void Update ()
	{
		if (!Input.GetKey(KeyCode.LeftShift))
		{
			
			if (Input.GetButton ("Fire1") && Time.time > nextFire)
			{
				nextFire = Time.time + fireRate;
				StartCoroutine (shotEffect ());

				Vector3 rayOrigin = fpsCamera.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0)); // Centered on screen
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);	// Gets location of mouse
				RaycastHit hit;

				laserLine.SetPosition (0, gunEnd.position); // Starts laser at end of gun

				if (Physics.Raycast (ray, out hit, weaponRange)) // If hit something
				{ 
					laserLine.SetPosition (1, hit.point); // Fire to hit object
                    CubeBehavior cube = hit.collider.GetComponent<CubeBehavior>();
                    LayerParentBehavior layer = hit.transform.parent.GetComponent<LayerParentBehavior>();
                    if (cube != null) {
						cube.Damage (gunDamage);    // Damage cube
						if (cube.getHealth() <= 0) {    // If cube is destroyed
                            cubits += cube.getValue();
                            if (layer.getNumCubes() == 0)
                            {
                                layer.disable();
                            }
						}
                        statText.text = scoreText + cubits + "\n";  // Update score
					}
				}
				else // If hit nothing
				{
					laserLine.SetPosition(1, rayOrigin + (ray.direction * weaponRange)); // Fire to weaponRange
				}
			}
		}
	}

	public void IncreaseFireRate(){
		fireRate = fireRate * 0.8f;
	}

	public void IncreaseDamage(){
		gunDamage++;
	}

	private IEnumerator shotEffect()
	{
		gunAudio.Play ();
		laserLine.enabled = true;
		yield return shotDuration; // wait .07 seconds
		laserLine.enabled = false;
	}
}
