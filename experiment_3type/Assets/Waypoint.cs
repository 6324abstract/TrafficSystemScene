using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{

    public int numofWaypoint;//quoted by hazard and navigation
    [SerializeField] private Transform positions;
    [SerializeField] private GameObject navigation;
    private readonly int total_waypoint=6;
    void Start()
    {
        numofWaypoint = 0;
        moveWaypoint();
       
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider colider)
    {
        if (colider.tag == "Player")
        {
            
            moveWaypoint();//update the waypoint

            //increment navigation 
            Navigation.TogiveInstruction = true;
           
            navigation.GetComponent<Navigation>().StartNavigation();
            navigation.GetComponent<Navigation>().waypointIncrement();
            navigation.GetComponent<Navigation>().getNextModal();
          
        }
    }
    void moveWaypoint()
    {
        if (numofWaypoint <= total_waypoint)
        {
            Transform nextPoint = positions.GetChild(numofWaypoint++).transform;
            transform.position = new Vector3(nextPoint.position.x, transform.position.y, nextPoint.position.z);
            transform.rotation = nextPoint.rotation;
        }
    }
}
