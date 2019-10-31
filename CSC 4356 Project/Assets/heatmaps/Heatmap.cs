using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Heatmap : MonoBehaviour
{
    public Vector4[] positions;  // x is from [-0.4, 0.4] and y is from [-0.4, 0.4]
    public Vector4[] properties; // x is radius [0.0, 0.25] and y is intensity [0.0, 1.0]

    public Material material; // Gives access to shader in material
    public Text text;         // Gives access the text above the heatmap

    int count = 81; // Size of Vector Array; Higher size results in precise measurements but higher tax on gpu

    // Initial x and y positions to start initializing heatmap
    // Would need to mess with shader to change these values
    float xPosition = -0.4f;
    float yPosition = -0.4f;

    public bool heatMapStarted = false;      // Keeps track of when the heat map is started or stopped
    public float startingIntensity = 0.4f;   // Intensity that each properties vector starts with; Ranges from [0.0, 1.0]
    public float drawTime = 0.1f;            // Speed at which the heat map is being drawn overall
    public float radiusIncrement = .01f;     // Speed that the radius expands; Must be set between [0.01, 0.10] for this program
    public float intensityIncrement = 0.01f; // Speed that the intensity increases; Must be set between [0.01, 1.0] for this program
    public float radiusMax = 0.10f;          // Maximum size of the radius
    public float intensityMax = 1.0f;        // Maximum size of the intensity

    // Current position, radius, and intensity of the vector being looked at
    // These variables do not need to be public
    public float xCurrentPosition;
    public float yCurrentPosition;
    public float currentRadius;
    public float currentIntensity;

    void Start()
    {
        // Initializes the heatmap positions, radiuses, and intensities
        positions = new Vector4[count];
        properties = new Vector4[count];

        for (int i = 0; i < count; i++)
        {
            if (i % 9 == 0 && i != 0) // Makes a 9 x 9 grid for the heat map; Numbers inputted manually to make program less complex
            {
                xPosition = xPosition + 0.1f;
                yPosition = -0.4f;
            }

            xPosition = (float) Mathf.Round(xPosition * 10) / 10; // Rounds to the nearest tenth
            yPosition = (float) Mathf.Round(yPosition * 10) / 10; // Rounds to the nearest tenth
            positions[i] = new Vector4(xPosition, yPosition, 0, 0);
            properties[i] = new Vector4(0, startingIntensity, 0, 0);

            yPosition = yPosition + 0.1f;
        }

        // Sets the initial vector arrays to the shader float arrays
        material.SetInt("_Points_Length", positions.Length);
        material.SetVectorArray("_Points", positions);
        material.SetVectorArray("_Properties", properties);
    }

    void Update()
    {
        // Whenever the space key or the controller triggers are pressed, it starts and stops the heat map.
        if (Input.GetKeyDown(KeyCode.Space) || (Input.GetButtonDown("Left Trackpad Press") || Input.GetButtonDown("Right Trackpad Press")))
        {
            Debug.Log("Button pressed!");
            if (!heatMapStarted)
            {
                heatMapStarted = true;
                text.text = "Heat Map: Started"; // Gives confirmation that the heat map has started drawing
                text.color = Color.green;
                InvokeRepeating("SetNewProperty", 0.0f, drawTime); // Starts drawing the heatmap based off given drawTime
            }
            else
            {
                heatMapStarted = false;
                text.text = "Heat Map: Stopped"; // Gives confirmation that the heat map has stopped drawing
                text.color = Color.red;
                CancelInvoke("SetNewProperty"); // Stops drawing on the heat map
            }
        }
    }
    
    void SetNewProperty()
    {
        // If the Line Renderer collides with an object, the points at which it collides are given.
        if (LineCollision.colliding)
        {
            Debug.Log("Hit");
            // Rounds x and y of current position that is being looked at to nearest tenth
            xCurrentPosition = (float) Mathf.Round(LineCollision.colliderInfo.point.x * 10) / 10;
            yCurrentPosition = (float) Mathf.Round(LineCollision.colliderInfo.point.y * 10) / 10;
            
            // Makes sure the current position is within the correct parameters
            if ((xCurrentPosition >= -0.4f && xCurrentPosition <= 0.4f) && (yCurrentPosition >= -0.4f && yCurrentPosition <= 0.4f))
            {
                int currentPositionIndex = SearchIndex(xCurrentPosition, yCurrentPosition); //Sets index of the current position

                if (currentPositionIndex == -1) // Keeps an error from happening if an index is not found
                {
                    Debug.Log("Index not found!");
                }
                else
                {
                    Debug.Log("Index: " + currentPositionIndex + " X: " + xCurrentPosition + " Y: " + yCurrentPosition);

                    // Retrieves current radius and intensity from properties vector at the current position index
                    currentRadius = properties[currentPositionIndex].x;
                    currentIntensity = properties[currentPositionIndex].y;

                    // This condition keeps the radius and instensity from becoming too large
                    if (currentRadius < radiusMax || currentIntensity < intensityMax)
                    {
                        if (currentRadius < radiusMax)
                        {
                            currentRadius = currentRadius + radiusIncrement;
                        }
                        if (currentIntensity < intensityMax)
                        {
                            currentIntensity = currentIntensity + intensityIncrement;
                        }

                        // Sets properties vector at the current position index to the updated radius and intensity values
                        // and then updates the shader to draw on heat map
                        properties[currentPositionIndex] = new Vector4(currentRadius, currentIntensity, 0, 0);
                        material.SetVectorArray("_Properties", properties);
                    }
                }
            }
        }
    }

    // Searches linearly for the index of the current position being looked at
    int SearchIndex(float x, float y)
    {
        for (int i = 0; i < count; i++)
        {
            if (positions[i].x == x && positions[i].y == y)
            {
                return i;
            }
        }
        return -1; // Should never happen, but returns -1 in case the position vector is not found
    }
}