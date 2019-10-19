// Alan Zucconi
// www.alanzucconi.com
using UnityEngine;
using System.Collections;

public class Heatmap : MonoBehaviour
{
    public Vector4[] positions;
    public Vector4[] properties;
    public float[] radiuses;
    public float[] intensities;

    public LineRenderer line;
    public Material material;

    public int count = 50;
    int i = 0;



    void Start()
    {
        positions = new Vector4[count];
        properties = new Vector4[count];
        radiuses = new float[count];
        intensities = new float[count];

        InvokeRepeating("SetNewPos", 1f, 1f);
        /*
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = new Vector4(Random.Range(-0.4f, +0.4f), Random.Range(-0.4f, +0.4f), 0, 0);
            properties[i] = new Vector4(Random.Range(0f, 0.25f), Random.Range(-0.25f, 1f), 0, 0);
        }
        */
        /*
        for (int j = 0; j < positions.Length; j++)
        {
            positions[j] = new Vector4(Random.Range(-0.4f, +0.4f), Random.Range(-0.4f, +0.4f));
            radiuses[j] = Random.Range(0f, 0.25f);
            intensities[j] = Random.Range(-0.25f, 1f);
            properties[j] = new Vector4(radiuses[j], intensities[j], 0, 0);
        }
        */
    }

    void Update()
    {
        material.SetInt("_Points_Length", positions.Length);
        material.SetVectorArray("_Points", positions);
        material.SetVectorArray("_Properties", properties);
        if (i == count)
        {
            CancelInvoke("SetNewPos");
        }
    }
    
    void SetNewPos()
    {
        if (LineCollision.colliding == true && i < count)
        {
            Debug.Log("Hit");
            //positions[i] += new Vector4(Random.Range(-0.1f, +0.1f), Random.Range(-0.1f, +0.1f), 0, 0) * Time.deltaTime;
            positions[i] += new Vector4(LineCollision.colliderInfo.point.x, LineCollision.colliderInfo.point.y, 0, 0) * Time.deltaTime;
            properties[i] += new Vector4(LineCollision.colliderInfo.point.x, LineCollision.colliderInfo.point.y, 0, 0) * Time.deltaTime;
            /*
            material.SetInt("_Points_Length", count);
            material.SetVectorArray("_Points", positions);
            material.SetVectorArray("_Properties", properties);
            */
            i++;
        }
    }
}