using UnityEngine;

public class TouchDtect_4x4 : MonoBehaviour
{
    // public DebugLogManager debugLogManager; // Reference to DebugLogManager
    public Cube4x4Ctrl cube4x4Ctrl; // Reference to Cube9Ctrl

    void Start()
    {
        // cube9Ctrl = cube9Parent.GetComponent<Cube9Ctrl>();
    }



    void OnTriggerEnter(Collider other)
    {


        string tag1 = gameObject.name;
        string tag2 = other.name;
        string message = $"Collider: {tag1} collided with {tag2}";
        Debug.Log(message);
        cube4x4Ctrl.SetTrue(tag1, tag2);


    }

    void OnTriggerExit(Collider other)
    {


        string tag1 = gameObject.name;
        string tag2 = other.name;
        string message = $"Collider: {tag1} exited with {tag2}";
        Debug.Log(message);
        cube4x4Ctrl.SetFalse(tag1, tag2);
    }

    // Call the method in DebugLogManager to add this message to the log
    // debugLogManager.AddMessage(message);
}
