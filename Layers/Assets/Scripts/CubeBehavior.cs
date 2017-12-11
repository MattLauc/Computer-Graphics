using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeBehavior : MonoBehaviour {

	public int maxHealth;
	public int spreadRate;
	public int pauseTime;
    public int value;

    public GameObject prevLayer;
	public GameObject[] adjCubes;

    public Material[] materials;
    Renderer render;
	public Text winText;


    private int timeSinceHit;
	private int health;

	private AudioSource destroySound;
	private AudioSource regenSound;

	void Start () {
        health = maxHealth;
        timeSinceHit = 0;

        render = GetComponent<Renderer>();
        render.enabled = true;
        render.sharedMaterial = materials[health - 1];

		AudioSource[] sounds = GetComponents<AudioSource> ();
		destroySound = sounds[0];
		regenSound = sounds [1];

		winText.text = "";
	}

    void Update ()
    {
        if (prevLayer == null || prevLayer.activeSelf == false) {   // If there is no outside layer, then update
			if (timeSinceHit < pauseTime + health * spreadRate)
            {
                timeSinceHit++;
                if (timeSinceHit > pauseTime)
                {
                    if ((timeSinceHit - pauseTime) % spreadRate == 0)
                    {
						if (health != maxHealth) {
							health++;
							UpdateColor ();
						} else {
							Spread ();
						}
                    }
                }
            }
        }
    }

    public void Damage(int damage)
    {
        if (prevLayer == null || prevLayer.activeSelf == false)
        { 
            timeSinceHit = 0;
            health -= damage;
			destroySound.Play ();

			if (health <= 0) {
				//update adjacent cubes (set time since hit to 0)
				for (int i = 0; i < adjCubes.Length; i++) {
					adjCubes [i].GetComponent<CubeBehavior> ().timeSinceHit = 0;
				}
				if (gameObject.tag != "CubeHeart") {
					gameObject.GetComponentInParent<LayerParentBehavior> ().decNumCubes ();
				} else {
					winText.text = "You Won!";
				}
				gameObject.SetActive (false);
			} else {
				UpdateColor ();
			}
        }
    }

    public void UpdateColor()
    {
        if (health != 0)
        {
			if (this.tag != "CubeHeart"){
            	render.sharedMaterial = materials[health - 1];
			}
        }
    }

	public void Spread() {

		for (int i = 0; i < adjCubes.Length; i++) {
			if (adjCubes [i].activeSelf == false) {
				adjCubes [i].SetActive (true);
				CubeBehavior cb = adjCubes [i].GetComponent<CubeBehavior> ();
				cb.health = 1;
				cb.timeSinceHit = cb.pauseTime;
				gameObject.GetComponentInParent<LayerParentBehavior> ().incNumCubes ();
				regenSound.Play ();
			}
		}
	}

	public int getHealth() {
		return health;
	}

    public int getValue()
    {
        return value;
    }

}
