using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBelt : MonoBehaviour
{
    [SerializeField] float speed = 4.0f;

	void Update ()
    {
        transform.Rotate(0, -speed * Time.deltaTime, 0);
    }
}
