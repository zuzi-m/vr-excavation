using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Excavation : MonoBehaviour {

    public float excavationSpeed;
    public float excavationReach;

    public GameObject excavator;
    public Terrain seafloor;

    private const int EXCAVATE_SIZE = 15;
    private const int EXCAVATE_SIZE_2 = EXCAVATE_SIZE / 2;

    private bool isExcavating = false;
    private float excavationModifier = -1.0f;

    private float[,] startHeightmap;

    // Use this for initialization
    void Start ()
    {
        GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);
        GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(DoGripPressed);

        // temporarily save the height map of terrain so that it wouldnt permanently change when excavating
        startHeightmap = seafloor.terrainData.GetHeights(0, 0, seafloor.terrainData.heightmapWidth, seafloor.terrainData.heightmapWidth);
    }
	
	// Update is called once per frame
	void Update ()
    {
        // EXCAVATION
        if (isExcavating)
        {
            // do the ray cast to see where excavating
            RaycastHit hit;
            Ray ray = new Ray(excavator.transform.position, excavator.transform.TransformDirection(Vector3.up));
            if (Physics.Raycast(ray, out hit, excavationReach))
            {
                //Debug.Log("ray cast hit at" + hit.point + " which is at uv " + hit.textureCoord + " and terrain has size " + seafloor.terrainData.size + " and heightmap width " + seafloor.terrainData.heightmapWidth);

                // calculate the point where the ray hit by using UV coordinates and heightmap resolution
                int hitpointX = Mathf.CeilToInt(Mathf.Max(Mathf.Min((hit.textureCoord.x * seafloor.terrainData.heightmapWidth) - EXCAVATE_SIZE_2, seafloor.terrainData.heightmapWidth), 0.0f));
                int hitpointY = Mathf.CeilToInt(Mathf.Max(Mathf.Min((hit.textureCoord.y * seafloor.terrainData.heightmapWidth) - EXCAVATE_SIZE_2, seafloor.terrainData.heightmapWidth), 0.0f));

                // get the height samples from the right spot
                float[,] samples = seafloor.terrainData.GetHeights(
                    hitpointX,
                    hitpointY,
                    EXCAVATE_SIZE,
                    EXCAVATE_SIZE);

                // compute the change in each height sample
                for (int i = 0; i < EXCAVATE_SIZE; i++)
                {
                    for (int j = 0; j < EXCAVATE_SIZE; j++)
                    {
                        // local sample coordinates in the patch, centered in the middle
                        int sample_x = i - EXCAVATE_SIZE_2;
                        int sample_y = j - EXCAVATE_SIZE_2;

                        // use quadratic function to get amount of change in this sample
                        float a = 1.5f;
                        float b = a;
                        float c = 1.0f / EXCAVATE_SIZE_2;
                        float z = ((sample_x * sample_x * c) / (a * a)) + ((sample_y * sample_y * c) / (b * b));

                        float sample_amount = Mathf.Clamp(1.0f - z, 0.0f, 1.0f);

                        // update samples by combining sample change amount, speed, time, and switch for add/subtract
                        samples[i, j] = samples[i, j] + (sample_amount * excavationModifier * excavationSpeed * Time.deltaTime);
                    }
                }

                // update the height samples
                seafloor.terrainData.SetHeights(
                    hitpointX,
                    hitpointY,
                    samples);
            }
        }
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        isExcavating = true;
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        isExcavating = false;
    }

    private void DoGripPressed(object sender, ControllerInteractionEventArgs e)
    {
        excavationModifier = -excavationModifier;
    }

    private void OnApplicationQuit()
    {
        // reset the height map back to how it was at start
        seafloor.terrainData.SetHeights(0, 0, startHeightmap);
    }
}
