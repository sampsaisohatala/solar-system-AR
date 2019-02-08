using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    [SerializeField] float yearSpeed;
    [SerializeField] float daySpeed;

    Transform mesh;
    PlanetInformation info;

    bool infoOn;

    void Start()
    {
        mesh = transform.Find("Mesh");
        info = GetComponent<PlanetInformation>();
        
    }
    void Update()
    {
        if (this.tag == "Star")
            return;

        transform.localPosition = new Vector3(0f, 0f, 0f);

        AroundTheSun();
        AroundItself();
    }

    public void Touched()
    {
        info.ShowInfo();  
    }

    void AroundTheSun()
    {
        transform.Rotate(Vector3.up * -yearSpeed / 3 * Time.deltaTime);
    }

    void AroundItself()
    {
        mesh.position = mesh.GetComponent<Renderer>().bounds.center;
        mesh.Rotate(Vector3.up * -daySpeed / 3 * Time.deltaTime);
    }
}
