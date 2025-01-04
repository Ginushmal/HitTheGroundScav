using UnityEngine;

public class checkSolved : MonoBehaviour
{
    private Cube9Ctrl cube9Ctrl; // Reference to Cube9Ctrl

    public GameObject cube9Parent; // Reference to the parent object of the cube9s
    void Start()
    {
        cube9Ctrl = cube9Parent.GetComponent<Cube9Ctrl>();
    }

    // Update is called once per frame
    void Update()
    {
        // get the value of the isPuzzelSolved from the Cube9Ctrl
        bool isPuzzelSolved = cube9Ctrl.GetIsPuzzelSolved();
        // if the puzzle is solved , show this object hide othervise
        if (isPuzzelSolved)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
