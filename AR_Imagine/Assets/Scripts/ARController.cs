using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

#if UNITY_EDITOR
using Input = GoogleARCore.InstantPreviewInput;
#endif

public class ARController : MonoBehaviour
{
    public delegate void ClickAction();
    public static event ClickAction TargetCreated;

    public static bool InfoVisible;
    public static bool DropdownVisible;

    //Lista täytetään planeilla jotka ARCore huomaa framen aikana
    private List<DetectedPlane> m_NewDetectedPlane = new List<DetectedPlane>();

    public GameObject GridPrefab;
    public GameObject TargetPrefab;
    public GameObject ARCamera;
    public HUDSolarSystem HUD;

    public GameObject HelperScan;
    public GameObject HelperPlace;

    TrackableHit hit;
    float targetHeight;
    bool isCreated;
    float screenHeight;



    public float zoomSpeed;
    public float minPinchSpeed = 5.0F;
    public float varianceInDistances = 5.0F;
    public float rotationRate;

    float MINSCALE = 0.006f;
    float MAXSCALE = 0.2f;
    float touchDelta = 0.0F;
    Vector2 prevDist = new Vector2(0, 0);
    Vector2 curDist = new Vector2(0, 0);
    float speedTouch0 = 0.0F;
    float speedTouch1 = 0.0F;



    void Start()
    {
        Time.timeScale = 1f;
        HelperScan.SetActive(true);
        DropdownVisible = false;
        screenHeight = Screen.height;
    }

	void Update ()
    {       
        if (Session.Status != SessionStatus.Tracking)
            return;

        if (!isCreated)
        {
            if (m_NewDetectedPlane.Count != 0)
            {
                HelperScan.SetActive(false);
                HelperPlace.SetActive(true);
            }      

            //Täyttää m_NewDetectedPlanen planeilla jotka ARCore huomasi framen aikana
            Session.GetTrackables<DetectedPlane>(m_NewDetectedPlane, TrackableQueryFilter.New);

            //Luo grid jokaselle planelle m_NewDetectedPlane listassa. 
            for (int i = 0; i < m_NewDetectedPlane.Count; i++)
            {
                GameObject grid = Instantiate(GridPrefab, Vector3.zero, Quaternion.identity, transform);
                grid.GetComponent<GridVizualizer>().Initialize(m_NewDetectedPlane[i]);
            }

            CreateTarget();
        }
        else
        {
            if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved) 
            {
                if (DropdownVisible)
                    return;

                print("kosketus tuplatana");
                curDist = Input.GetTouch(0).position - Input.GetTouch(1).position; //current distance between finger touches
                prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition)); //difference in previous locations using delta positions
                touchDelta = curDist.magnitude - prevDist.magnitude;
                speedTouch0 = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
                speedTouch1 = Input.GetTouch(1).deltaPosition.magnitude / Input.GetTouch(1).deltaTime;

                if ((touchDelta + varianceInDistances < 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
                {
                    if (TargetPrefab.transform.localScale.x < MINSCALE)
                        return;
                    
                    TargetPrefab.transform.localScale += new Vector3(1 * -zoomSpeed, 1 * -zoomSpeed, 1 * -zoomSpeed);
                    print("zoom out" + TargetPrefab.transform.localScale);
                    HUD.AdjustScaleSlider(TargetPrefab.transform.localScale.x);
                }
                if ((touchDelta + varianceInDistances > 1) && (speedTouch0 > minPinchSpeed) && (speedTouch1 > minPinchSpeed))
                {
                    

                    if (TargetPrefab.transform.localScale.x > MAXSCALE)
                        return;
                    
                    TargetPrefab.transform.localScale += new Vector3(1 * zoomSpeed, 1 * zoomSpeed, 1 * zoomSpeed);
                    print("zoom in: " + TargetPrefab.transform.localScale);
                    HUD.AdjustScaleSlider(TargetPrefab.transform.localScale.x);
                }
            }

            if ((Input.touchCount == 1) && (Input.GetTouch(0).phase == TouchPhase.Moved))
            {
                if (DropdownVisible)
                    return;

                TargetPrefab.transform.Rotate(0, Input.GetTouch(0).deltaPosition.x * -rotationRate, 0, Space.World);
            }

            else if ((Input.touchCount == 1) && (Input.GetTouch(0).phase == TouchPhase.Began))
            {

                if (DropdownVisible)
                    return;

                // estää planeetan kosketuksen kun painaa dropdown nappia. pitää vielä hioa
                if (Input.GetTouch(0).position.y >= screenHeight - (screenHeight / 10))
                    return;

                Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit raycastHit;
                if (Physics.Raycast(raycast, out raycastHit))
                {
                    if (raycastHit.collider.tag == "Planet")
                    {
                        if (InfoVisible)
                            return;
                        
                        raycastHit.collider.GetComponentInParent<PlanetController>().Touched();
                    }

                    if (raycastHit.collider.tag == "Star")
                    {
                        if (InfoVisible)
                            return;

                        raycastHit.collider.GetComponent<PlanetController>().Touched();
                    }
                }
            }
        }            
    }

    void CreateTarget()
    {
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            return;

        //Tarkistetaan koskettiko käyttäjä mitään träkättyä planea
        
        if (Frame.Raycast(touch.position.x, touch.position.y, TrackableHitFlags.PlaneWithinPolygon, out hit))
        {
            Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);

            Vector3 offSet = new Vector3(0, .2f, 0);

            //Instantiate(TargetPrefab, hit.Pose.position + offSet, hit.Pose.rotation, anchor.transform);

            TargetPrefab.SetActive(true);
            TargetPrefab.transform.position = hit.Pose.position + offSet;
            //TargetPrefab.transform.rotation = hit.Pose.rotation;
            TargetPrefab.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
            TargetPrefab.transform.parent = anchor.transform;

            if (TargetCreated != null)
                TargetCreated();

            targetHeight = TargetPrefab.transform.position.y;

            isCreated = true;
            HelperPlace.SetActive(false);
        }
    }

    public void ChangeTimeScale(float value)
    {
        Time.timeScale = value;
    }

    public void ChangeTargetScale(float value)
    {
        TargetPrefab.transform.localScale =  new Vector3(value, value, value);
    }

    public void ChangeTargetHeight(float value)
    {
        float h = targetHeight + value;
        TargetPrefab.transform.position = new Vector3(TargetPrefab.transform.position.x, h, TargetPrefab.transform.position.z);
    }
}
