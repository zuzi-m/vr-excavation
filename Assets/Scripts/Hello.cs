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
        /*if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 100.0f, 0))
            {
                Debug.Log("OMG" + angle);
                angle++;
                int SIZ = 5;

                Vector2 hitpoint = hit.textureCoord;

                float xCoord = 1f;
                float yCoord = 1f;

                //float[,] samples = Ter.terrainData.GetHeights(Mathf.CeilToInt(hitpoint[0] * Ter.terrainData.detailWidth), Mathf.CeilToInt(Mathf.CeilToInt(hitpoint[1] * Ter.terrainData.detailHeight)), SIZ, SIZ);
                float[,] samples = Ter.terrainData.GetHeights(Mathf.CeilToInt(xCoord), Mathf.CeilToInt(yCoord), SIZ, SIZ);
                for (int i = 0; i < SIZ; i++)
                {
                    for (int j = 0; j < SIZ; j++)
                    {
                        samples[i, j] = samples[i, j] + 0.001f * Time.deltaTime;
                    }
                }

                Ter.terrainData.SetHeights(Mathf.CeilToInt(xCoord), Mathf.CeilToInt(yCoord), samples);
            }
        }*/
		
    }
}
