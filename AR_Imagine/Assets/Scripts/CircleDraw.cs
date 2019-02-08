using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDraw : MonoBehaviour
{
    [SerializeField] float theta_scale = 0.001f;        //Set lower to add more points

    int size;                                           //Total number of points in circle
    float radius;
    float thickness = .002f;

    public Material lineMaterial;

    LineRenderer lr;
    Transform planet;

    void Awake()
    {
        planet = transform.Find("Mesh");

        Material circleMaterial = Resources.Load("CircleMaterial", typeof(Material)) as Material;

        float sizeValue = (2.0f * Mathf.PI) / theta_scale;
        size = (int)sizeValue;
        size++;
        lr = gameObject.AddComponent<LineRenderer>();
        lr.material = circleMaterial;
        lr.startWidth = thickness;
        lr.endWidth = thickness;
        lr.positionCount = size;
    }

    void Update()
    {
        radius = Vector3.Distance(transform.position, planet.position);

        Vector3 pos;
        float theta = 0f;
        for (int i = 0; i < size; i++)
        {
            theta += (2.0f * Mathf.PI * theta_scale);
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);
            x += gameObject.transform.position.x;
            z += gameObject.transform.position.z;
            pos = new Vector3(x, planet.transform.position.y, z);
            lr.SetPosition(i, pos);
        }
    }
}
