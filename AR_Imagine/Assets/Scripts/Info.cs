using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    public string planetName;
    public Sprite img;
    public string diameter;
    public string mass;
    public string moons;
    public string type;
    public string tempeture;
    public string year;
    public string day;
    public string distance;
    

    
    void Start()
    {
        transform.Find("Panel/Name").GetComponent<Text>().text = planetName;
        transform.Find("Panel/Image").GetComponent<Image>().sprite = img;
        transform.Find("Panel/Diameter/Answer").GetComponent<Text>().text = diameter;
        transform.Find("Panel/Mass/Answer").GetComponent<Text>().text = mass;
        transform.Find("Panel/Moons/Answer").GetComponent<Text>().text = moons;
        transform.Find("Panel/Type/Answer").GetComponent<Text>().text = type;
        transform.Find("Panel/Tempeture/Answer").GetComponent<Text>().text = tempeture;
        transform.Find("Panel/Year/Answer").GetComponent<Text>().text = year;
        transform.Find("Panel/Day/Answer").GetComponent<Text>().text = day;
        transform.Find("Panel/Distance/Answer").GetComponent<Text>().text = distance;
    }
}
