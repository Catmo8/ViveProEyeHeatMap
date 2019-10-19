using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollision : MonoBehaviour
{
    LineRenderer line;
    public static bool colliding;
    public static Ray laserRay;
    public static RaycastHit colliderInfo;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        colliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Set up a ray, and make it's direction go forward
        laserRay = new Ray(line.GetPosition(0), line.GetPosition(1));
        //If our ray hits something, we want to set the next index of the line renderer to be the exact vector3 where the ray hit, which you can get by
        if (Physics.Raycast(laserRay, out colliderInfo, Mathf.Infinity))
        {
            line.SetPosition(1, colliderInfo.point);
            colliding = true;
        }
        else
        {
            colliding = false;
        }

    }
}
