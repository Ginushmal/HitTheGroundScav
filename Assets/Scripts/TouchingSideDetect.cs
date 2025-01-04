using System;
using UnityEngine;

public class TouchingSideDetect : MonoBehaviour
{
    // public DebugLogManager debugLogManager; // Reference to DebugLogManager
    private Cube9Ctrl cube9Ctrl; // Reference to Cube9Ctrl

    public GameObject cube9Parent; // Reference to the parent object of the cube9s
    private object combinedNumbers;

    void Start()
    {
        cube9Ctrl = cube9Parent.GetComponent<Cube9Ctrl>();
    }



    void OnTriggerEnter(Collider other)
    {
        string message = $"Collider: {gameObject.tag} collided with {other.tag}";
        Debug.Log(message);

        string tag1 = gameObject.tag;
        string tag2 = other.tag;

        cube9Ctrl.SetTrue(tag1, tag2);


    }

    void OnTriggerExit(Collider other)
    {
        string message = $"Collider: {gameObject.tag} exited with {other.tag}";
        Debug.Log(message);

        string tag1 = gameObject.tag;
        string tag2 = other.tag;

        cube9Ctrl.SetFalse(tag1, tag2);
    }

    // Call the method in DebugLogManager to add this message to the log
    // debugLogManager.AddMessage(message);
}


