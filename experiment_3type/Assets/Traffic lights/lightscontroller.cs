using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightscontroller : MonoBehaviour
{
    public  MeshRenderer Redlight;
    public MeshRenderer YellowLight;
    public MeshRenderer GreenLight;

    public void turnRed()
    {
        GreenLight.enabled=false;
       
        Redlight.enabled=true;
    }
}
