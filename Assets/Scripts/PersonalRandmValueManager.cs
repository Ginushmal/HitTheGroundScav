using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Firestore;

public class PersonalRandmValueManager : MonoBehaviour
{
    public int seed = 0;
    private Firebase.Auth.FirebaseAuth auth;
    private FirebaseFirestore firestore;

    // create a dictionary to keep the random values
    private Dictionary<string, object> randomValues = new Dictionary<string, object>();
    private int GetAsciiSum(string input)
    {
        if (string.IsNullOrEmpty(input))
            return 0; // Return 0 for null or empty strings.

        int asciiSum = 0;

        foreach (char c in input)
        {
            asciiSum += (int)c; // Add the ASCII value of the character to the sum.
        }

        return asciiSum;
    }

    void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                firestore = FirebaseFirestore.DefaultInstance;
                var currentUser = auth.CurrentUser;
                if (currentUser != null)
                {
                    seed = GetAsciiSum(currentUser.UserId);
                    Random.InitState(seed);
                    Debug.Log("Seed: " + seed);
                }
                else
                {
                    Debug.LogError("Current user is null.");
                    Random.InitState(seed);
                }
            }
            else
            {
                Debug.LogError("Could not resolve Firebase dependencies.");
            }
        });



        // based on the seed define 4 random 90 degree rotation values and store them in the dictionary
        randomValues.Add("Cube_1_Par_Rotation", Random.Range(0, 4) * 90);
        randomValues.Add("Cube_2_Par_Rotation", Random.Range(0, 4) * 90);
        randomValues.Add("Cube_3_Par_Rotation", Random.Range(0, 4) * 90);
        randomValues.Add("Cube_4_Par_Rotation", Random.Range(0, 4) * 90);

        Debug.Log("All cube rotation values : " + randomValues["Cube_1_Par_Rotation"] + " " + randomValues["Cube_2_Par_Rotation"] + " " + randomValues["Cube_3_Par_Rotation"] + " " + randomValues["Cube_4_Par_Rotation"]);

        // // set the random rotation values for the cubes as 0 for now
        // randomValues.Add("Cube_1_Par_Rotation", 0);
        // randomValues.Add("Cube_2_Par_Rotation", 0);
        // randomValues.Add("Cube_3_Par_Rotation", 0);
        // randomValues.Add("Cube_4_Par_Rotation", 0);

        randomValues.Add("PigPen_offset", Random.Range(0, 10));

        // generate a random 5 character Sriting with Capital letters with the same seed 
        string generatedPassword = GeneratePassword(5);
        randomValues.Add("PigPen_password", generatedPassword);
        Debug.Log($"Generated Password: {generatedPassword}");

        // based on the seed define 4 random 45 degree rotation values and store them in the dictionary
        randomValues.Add("i_1_Rotation", Random.Range(0, 8) * 45);
        randomValues.Add("i_2_Rotation", Random.Range(0, 8) * 45);
        randomValues.Add("i_3_Rotation", Random.Range(0, 8) * 45);
        randomValues.Add("i_4_Rotation", Random.Range(0, 8) * 45);

        Debug.Log("All i rotation values : " + randomValues["i_1_Rotation"] + " " + randomValues["i_2_Rotation"] + " " + randomValues["i_3_Rotation"] + " " + randomValues["i_4_Rotation"]);

    }

    string GeneratePassword(int length)
    {
        const string start = "ABCDEFGH";  // Start part of the alphabet
        const string middle = "IJKLMNOP"; // Middle part of the alphabet
        const string end = "QRSTUVWXYZ";  // End part of the alphabet

        string password = "";
        for (int i = 0; i < length; i++)
        {
            string source = i % 3 == 0 ? start : (i % 3 == 1 ? middle : end); // Alternate between start, middle, and end
            int randomIndex = Random.Range(0, source.Length);
            password += source[randomIndex];
        }

        return password;
    }


    // create a public getter method to get the value of the random value
    public object GetRandomValue(string key)
    {
        return randomValues[key];
    }
}
