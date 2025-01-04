using System.Collections;
using UnityEngine;

public class HintHandler : MonoBehaviour
{

    public PrograssTracker prograssTracker;

    private Hashtable progress = new Hashtable();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        progress = prograssTracker.GetProgress();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
