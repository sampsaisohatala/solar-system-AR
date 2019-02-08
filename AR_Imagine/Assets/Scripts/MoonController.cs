using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonController : MonoBehaviour
{
    [SerializeField] float speed = 4.0f;

    public Transform PlanetMesh;

    void Update()
    {
        transform.position = PlanetMesh.position;
        transform.Rotate(0, -speed / 3 * Time.deltaTime, 0);
    }
}
