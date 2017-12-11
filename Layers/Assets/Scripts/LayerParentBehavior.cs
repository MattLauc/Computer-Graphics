using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerParentBehavior : MonoBehaviour {

    public int numCubes;

	public void decNumCubes()
    {
        numCubes--;
    }

    public void incNumCubes()
    {
        numCubes++;
    }

    public int getNumCubes()
    {
        return numCubes;
    }

    public void disable()
    {
        gameObject.SetActive(false);
    }
}
