using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    public Transform Mycar;
    public GameObject wp;//array of waypoints
    public static Transform Curwaypoint;
    public static Vector3 CurForward;
    public static bool TogiveInstruction;
    public static bool TogiveInstruction_visual;
    public static bool TogiveInstruction_audio;
    public static int numofWaypoint;
    protected Directions nextdirection;

    [SerializeField]
    private GameObject visualguide;
    [SerializeField]
    private GameObject audioguide;
    [SerializeField]
    private int participant_number;

    private Directions[] dirs;
    private Directions[] dirs_test;
    private GpsModal[] modals;
    public GpsModal modal;

   
    private GameObject curWaypoint;

    //instruction order by lattin balance 


    public enum Directions
    {
        straight,
        left,
        right,
        back
    }
    public enum GpsModal
    {
        visual,
        audio,
        compound,
        None
    }
    private void Awake()
    {
        //group1 vehicle=audio
        // modals = new GpsModal[] { GpsModal.visual, GpsModal.compound, GpsModal.audio,GpsModal.audio, GpsModal.compound,GpsModal.visual,GpsModal.audio,GpsModal.compound };
        //group2 vehicle=visual 
       // modals = new GpsModal[] { GpsModal.audio, GpsModal.visual, GpsModal.compound, GpsModal.visual, GpsModal.audio, GpsModal.compound, GpsModal.visual, GpsModal.compound };
        //group3 vehicle=compound
         modals= new GpsModal[] { GpsModal.compound, GpsModal.audio, GpsModal.visual, GpsModal.compound, GpsModal.audio, GpsModal.compound, GpsModal.visual, GpsModal.audio };
        numofWaypoint = 0;
        modal = modals[numofWaypoint];

        dirs = new Directions[] { Directions.straight, Directions.left, Directions.straight, Directions.left, Directions.straight, Directions.right, Directions.straight };
        dirs_test = new Directions[] { Directions.straight, Directions.left, Directions.straight, Directions.left, Directions.straight, Directions.right, Directions.straight };
    }
    public void getNextModal()
    {
        modal = modals[numofWaypoint];
    }

    public void waypointIncrement()
    {
        numofWaypoint++;
    }
    private void Start()
    {
        StartNavigation();
        TogiveInstruction = false;
        TogiveInstruction_audio = false;
        TogiveInstruction_visual = false;
    }
    protected Directions getNextDirection()
    {
        return dirs[numofWaypoint - 1];
    }


    public void StartNavigation()
    {
        Debug.Log("modal" + modal + " waypoint" + numofWaypoint);
        switch (modal)
        {
            case (GpsModal.audio):
                TogiveInstruction_audio = true;

                break;
            case (GpsModal.visual):
                TogiveInstruction_visual = true;

                break;
            case (GpsModal.compound):
                TogiveInstruction_audio = true;
                TogiveInstruction_visual = true;

                break;
        }
    }

    private GpsModal[] generate_instruction_order()
    {    
        return  new GpsModal[] { 
            GpsModal.visual, GpsModal.compound, GpsModal.audio, 
            GpsModal.audio, GpsModal.compound, GpsModal.visual, 
            GpsModal.audio, GpsModal.compound 
        }; 
    }
   
       
      
        
    

    class Route
    {
        public GameObject waypoint;
        private Vector3 forward;

    }
}


