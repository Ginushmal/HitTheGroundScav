using System.Collections.Generic;
using UnityEngine;


public class SafeHandler : MonoBehaviour
{
    public GameObject[] i_List;

    public GameObject closeupSafe;
    public GameObject[] knobs;
    public PersonalRandmValueManager personalRandmValueManager;

    public PrograssTracker prograssTracker;

    public GameObject animetionSafeObject;

    private Dictionary<string, float> i_rotations = new Dictionary<string, float>();
    // Dictionary to track each knob's current rotation angle
    private Dictionary<string, float> knobRotations = new Dictionary<string, float>();
    void Start()
    {
        foreach (GameObject i in i_List)
        {
            string name = i.name;
            int value = (int)personalRandmValueManager.GetRandomValue(name + "_Rotation");
            i.transform.Rotate(0, 0, value, Space.Self);
            i_rotations[name] = value;

        }

        // initialize the knob rotations to 0
        foreach (GameObject knob in knobs)
        {
            knobRotations[knob.name] = 0f;
        }
    }



    public void KnobClicked(string knobName)
    {
        foreach (GameObject knob in knobs)
        {
            if (knob.name == knobName)
            {
                if (!knobRotations.ContainsKey(knobName))
                {
                    knobRotations[knobName] = 0f; // Initialize rotation if not already tracked
                }

                // Add 45 to the tracked rotation
                knobRotations[knobName] = (knobRotations[knobName] + 45f) % 360f;

                // Apply the rotation to the knob
                knob.transform.localRotation = Quaternion.Euler(knobRotations[knobName], 0, 0);

                Debug.Log(knob.name + " rotation set to " + (knobRotations[knobName] - 90 + 360) % 360);
            }
        }
    }


    void checkForRotationMatch()
    {
        if (Mathf.Round((knobRotations["SafeKnob_1"] - 90 + 360) % 360) == Mathf.Round(i_rotations["i_1"]) &&
            Mathf.Round((knobRotations["SafeKnob_2"] - 90 + 360) % 360) == Mathf.Round(i_rotations["i_2"]) &&
            Mathf.Round((knobRotations["SafeKnob_3"] - 90 + 360) % 360) == Mathf.Round(i_rotations["i_3"]) &&
            Mathf.Round((knobRotations["SafeKnob_4"] - 90 + 360) % 360) == Mathf.Round(i_rotations["i_4"]))
        {
            Debug.Log("Safe Opened");
            prograssTracker.Safe_Opened();
            animetionSafeObject.GetComponent<Animator>().SetTrigger("SafeOpened");
        }
        else
        {
            Debug.Log("Safe Not Opened");
            // Debug.Log("Knob 1: " + Mathf.Round((knobs[0].transform.eulerAngles.x - 90 + 360) % 360) + " i_ 1: " + Mathf.Round(i_List[0].transform.eulerAngles.z) +
            //     " Knob 2: " + Mathf.Round((knobs[1].transform.eulerAngles.x - 90 + 360) % 360) + " i_ 2: " + Mathf.Round(i_List[1].transform.eulerAngles.z) +
            //     " Knob 3: " + Mathf.Round((knobs[2].transform.eulerAngles.x - 90 + 360) % 360) + " i_ 3: " + Mathf.Round(i_List[2].transform.eulerAngles.z) +
            //     " Knob 4: " + Mathf.Round((knobs[3].transform.eulerAngles.x - 90 + 360) % 360) + " i_ 4: " + Mathf.Round(i_List[3].transform.eulerAngles.z));

            // print corrected values
            Debug.Log("Knob 1: " + Mathf.Round((knobRotations["SafeKnob_1"] - 90 + 360) % 360) + " i_ 1: " + Mathf.Round(i_rotations["i_1"]) +
            " Knob 2: " + Mathf.Round((knobRotations["SafeKnob_2"] - 90 + 360) % 360) + " i_ 2: " + Mathf.Round(i_rotations["i_2"]) +
            " Knob 3: " + Mathf.Round((knobRotations["SafeKnob_3"] - 90 + 360) % 360) + " i_ 3: " + Mathf.Round(i_rotations["i_3"]) +
            " Knob 4: " + Mathf.Round((knobRotations["SafeKnob_4"] - 90 + 360) % 360) + " i_ 4: " + Mathf.Round(i_rotations["i_4"]));

        }

    }

    public void closeSafe()
    {
        closeupSafe.SetActive(false);
    }

    public void safeClicked()
    {
        closeupSafe.SetActive(true);
    }

    public void safeWheelClicked()
    {
        checkForRotationMatch();
    }


}
