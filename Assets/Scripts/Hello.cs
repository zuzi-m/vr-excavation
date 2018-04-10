using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hello : MonoBehaviour {
    [SerializeField]
    private int angle = 0;

	[SerializeField]
	private Terrain Ter;

	// Use this for initialization
	void Start () {
        Debug.Log("hi");
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("OMG" + angle);
        angle++;

		float xCoord = 1.5f;
		float yCoord = 1.5f;
		int SIZ = 3;

		float[,] samples = Ter.terrainData.GetHeights(Mathf.CeilToInt(xCoord), Mathf.CeilToInt(yCoord), SIZ, SIZ);
		for (int i = 0; i < SIZ; i++) {
			for (int j = 0; j < SIZ; j++) {
				samples [i,j] = samples [i,j] + 0.001f*Time.deltaTime;
			}
		}

		Ter.terrainData.SetHeights (Mathf.CeilToInt(xCoord), Mathf.CeilToInt(yCoord), samples);
    }
}
