using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioNavigator :Navigation
{
    private string left = "Left";
    private string right = "Right";
    private string straight = "Straight";

    private string NextDir;
    private Dictionary<Directions, string> Instructions;

 // Start is called before the first frame update
    void Start()
    {
        NextDir = straight;
        Instructions = new Dictionary<Directions, string>();
        Instructions.Add(Directions.left, left);
        Instructions.Add(Directions.right, right);
        Instructions.Add(Directions.straight, straight);
    }
    void Update()
    {
        if (TogiveInstruction_audio)
        {
            NextDir = Instructions[getNextDirection()];
            FindObjectOfType<AudioManager>().Play(NextDir);
            TogiveInstruction_audio = false;
        }

    }

}
