using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pedestriancontroller : MonoBehaviour {
   
    [SerializeField] private Transform waypoints;


    private static float speed;

    private void Awake()
    {
        speed = 1.7f;
        transform.position = waypoints.GetChild(0).transform.position;
        transform.rotation = waypoints.GetChild(0).transform.rotation;
    }
    public void Wakling()
    {
        this.GetComponent<Animator>().SetBool("istriggered", true);
        transform.Translate(new Vector3(0,0,1) * speed * Time.deltaTime);// forward array?
    }
    public float CalculateTTA(Transform cartrans, int numofhazard,float targetspeed)
    {
         Transform curhazard= waypoints.GetChild(numofhazard).transform;
        float distance = Vector3.Distance(cartrans.position, curhazard.position);

        return distance / targetspeed;
    }

    public void moveToNextPoint(int numofHazards)
    {
        Transform nextPoint = waypoints.GetChild(numofHazards).transform;
        transform.position = new Vector3(nextPoint.position.x, transform.position.y, nextPoint.position.z);
        transform.rotation = nextPoint.rotation;
        this.GetComponent<Animator>().SetBool("istriggered", false);
        
      
    }

}
