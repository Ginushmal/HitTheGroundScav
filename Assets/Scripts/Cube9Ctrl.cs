using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using TMPro;

public class Cube9Ctrl : MonoBehaviour
{
    // create a dictionary 
    private Dictionary<string, bool> cube9Data = new Dictionary<string, bool>();
    private HashSet<string> validKeys = new HashSet<string> { "1-2", "2-3", "1-4", "2-5", "3-6", "4-5", "5-6", "4-7", "5-8", "6-9", "7-8", "8-9" };

    public TextMeshProUGUI dictText;

    private bool isPuzzelSolved = false;

    public GameObject debugLogManager; // Reference to DebugLogManager

    // set the initial value of the dictionary for each key
    void Start()
    {
        foreach (string key in validKeys)
        {
            cube9Data[key] = false; // Predefine keys with default value `false`
        }
        dictText.text = "{1-2 : " + cube9Data["1-2"] + ", 2-3 : " + cube9Data["2-3"] + ", 1-4 : " + cube9Data["1-4"] + ", 2-5 : " + cube9Data["2-5"] + ", 3-6 : " + cube9Data["3-6"] + ", 4-5 : " + cube9Data["4-5"] + ", 5-6 : " + cube9Data["5-6"] + ", 4-7 : " + cube9Data["4-7"] + ", 5-8 : " + cube9Data["5-8"] + ", 6-9 : " + cube9Data["6-9"] + ", 7-8 : " + cube9Data["7-8"] + ", 8-9 : " + cube9Data["8-9"] + "}";
    }

    // method to ocheck if the puzzle is solved by checking if all the values in the dictionary are true and set the isPuzzelSolved to true
    void checkPuzzleSolved()
    {
        dictText.text = "{1-2 : " + cube9Data["1-2"] + ", 2-3 : " + cube9Data["2-3"] + ", 1-4 : " + cube9Data["1-4"] + ", 2-5 : " + cube9Data["2-5"] + ", 3-6 : " + cube9Data["3-6"] + ", 4-5 : " + cube9Data["4-5"] + ", 5-6 : " + cube9Data["5-6"] + ", 4-7 : " + cube9Data["4-7"] + ", 5-8 : " + cube9Data["5-8"] + ", 6-9 : " + cube9Data["6-9"] + ", 7-8 : " + cube9Data["7-8"] + ", 8-9 : " + cube9Data["8-9"] + "}";
        // Check if all values in the dictionary are true if so , set the isPuzzelSolved to true
        if (cube9Data.Values.All(value => value == true))
        {
            isPuzzelSolved = true;
            Debug.Log("Puzzle Solved");
            debugLogManager.GetComponent<DebugLogManager>().AddMessage("Puzzle Solved");
        }
        else
        {
            isPuzzelSolved = false;
            Debug.Log("Puzzle Not Solved");
            debugLogManager.GetComponent<DebugLogManager>().AddMessage("Puzzle Not Solved");
        }
    }


    // create a public getter method to get the value of the isPuzzelSolved
    public bool GetIsPuzzelSolved()
    {
        return isPuzzelSolved;
    }

    // create two methods to set true or false of a certain key of the dictionary 
    public void SetTrue(string tag1, string tag2)
    {
        // Initialize the output variable
        string combinedNumbers = null;

        // Remove the "Cube_" prefix
        string cleanTag1 = tag1.Replace("Cube_", "");
        string cleanTag2 = tag2.Replace("Cube_", "");

        // Split the cleaned tags by the '-' delimiter
        string[] parts1 = cleanTag1.Split('-');
        string[] parts2 = cleanTag2.Split('-');

        // Ensure both tags have the correct format (2 parts)
        if (parts1.Length == 2 && parts2.Length == 2)
        {
            // Try parsing the numbers into integers
            if (int.TryParse(parts1[0], out int num1A) && int.TryParse(parts1[1], out int num1B) &&
                int.TryParse(parts2[0], out int num2A) && int.TryParse(parts2[1], out int num2B))
            {
                // Check if the numbers are mirrors (e.g., "1-2" and "2-1")
                if (num1A == num2B && num1B == num2A)
                {
                    // get the minimum number and the maximum number
                    int minNum = Math.Min(num1A, num1B);
                    int maxNum = Math.Max(num1A, num1B);
                    // Combine numbers into a single string
                    combinedNumbers = $"{minNum}-{maxNum}";

                    // print dict entry 
                    Debug.Log("TouchingSideDetect Dict Entry : " + combinedNumbers);

                    cube9Data[combinedNumbers] = true;

                    Debug.Log("Cube9Ctrl: " + combinedNumbers + " is true");

                    checkPuzzleSolved();

                    debugLogManager.GetComponent<DebugLogManager>().AddMessage("Cube9Ctrl: " + combinedNumbers + " is true");

                    //  print the updated dict inside {} all key value pairs i the same line 
                    foreach (KeyValuePair<string, bool> kvp in cube9Data)
                    {
                        Debug.Log("Key = " + kvp.Key + ", Value = " + kvp.Value);
                    }

                }
            }



        }
    }

    public void SetFalse(string tag1, string tag2)
    {
        // Initialize the output variable
        string combinedNumbers = null;

        // Remove the "Cube_" prefix
        string cleanTag1 = tag1.Replace("Cube_", "");
        string cleanTag2 = tag2.Replace("Cube_", "");

        // Split the cleaned tags by the '-' delimiter
        string[] parts1 = cleanTag1.Split('-');
        string[] parts2 = cleanTag2.Split('-');

        // Ensure both tags have the correct format (2 parts)
        if (parts1.Length == 2 && parts2.Length == 2)
        {
            // Try parsing the numbers into integers
            if (int.TryParse(parts1[0], out int num1A) && int.TryParse(parts1[1], out int num1B) &&
                int.TryParse(parts2[0], out int num2A) && int.TryParse(parts2[1], out int num2B))
            {
                // Check if the numbers are mirrors (e.g., "1-2" and "2-1")
                if (num1A == num2B && num1B == num2A)
                {
                    // get the minimum number and the maximum number
                    int minNum = Math.Min(num1A, num1B);
                    int maxNum = Math.Max(num1A, num1B);
                    // Combine numbers into a single string
                    combinedNumbers = $"{minNum}-{maxNum}";

                    // print dict entry 
                    Debug.Log("TouchingSideDetect Dict Entry : " + combinedNumbers);

                    cube9Data[combinedNumbers] = false;
                    Debug.Log("Cube9Ctrl: " + combinedNumbers + " is false");

                    checkPuzzleSolved();

                    debugLogManager.GetComponent<DebugLogManager>().AddMessage("Cube9Ctrl: " + combinedNumbers + " is false");

                    //  print the updated dict inside {} all key value pairs i the same line 
                    foreach (KeyValuePair<string, bool> kvp in cube9Data)
                    {
                        Debug.Log("Key = " + kvp.Key + ", Value = " + kvp.Value);
                    }

                }
            }



        }
    }
}
