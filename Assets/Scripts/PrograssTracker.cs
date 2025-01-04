using System.Collections;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class PrograssTracker : MonoBehaviour
{
    // create a hash map to store the progress of the game
    private Hashtable progress = new Hashtable();
    private FirebaseFirestore firestore;
    private Firebase.Auth.FirebaseAuth auth;
    private readonly int[] fixedValues = { 100, 1000, 500, 500, 3000, 300, 600, 1000, 300, 2000, 100, 600 };
    private readonly int[] numerators = { 1000, 30000, 1000000, 1000000, 3000000, 1000000, 1000000, 1000000, 1000000, 5000000, 1000000, 1000000 };
    private readonly string[] taskKeys = { "Cube4x4_Found", "Cube4x4_Solved", "Door_Found", "Password_Lock_Found", "Pig_Pen_Solved", "Ball_Found", "Ball_Thrown", "Ball_Hit_Wicket", "Safe_Found", "Safe_Opened", "Gold_Key_Found", "Gold_Lock_Opened" };

    public TextMeshProUGUI scoreOutOfText;
    void Start()
    {
        firestore = FirebaseFirestore.DefaultInstance;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        // set the key value pairs for the progress
        progress.Add("Cube4x4_Found", false);
        progress.Add("Cube4x4_Solved", false);
        progress.Add("Door_Found", false);
        progress.Add("Password_Lock_Found", false);
        progress.Add("Pig_Pen_Solved", false);
        progress.Add("Ball_Found", false);
        progress.Add("Ball_Thrown", false);
        progress.Add("Ball_Hit_Wicket", false);
        progress.Add("Safe_Found", false);
        progress.Add("Safe_Opened", false);
        progress.Add("Gold_Key_Found", false);
        progress.Add("Gold_Lock_Opened", false);

        scoreOutOfText.text = "0/12";
    }


    public void Cube4x4_Found()
    {
        if ((bool)progress["Cube4x4_Found"])
        {
            return;
        }
        else
        {
            UpdateProgress("Cube4x4_Found");
        }
    }

    public void Cube4x4_Solved()
    {
        if ((bool)progress["Cube4x4_Solved"])
        {
            return;
        }
        else
        {
            UpdateProgress("Cube4x4_Solved");
        }
    }

    public void Door_Found()
    {
        if ((bool)progress["Door_Found"])
        {
            return;
        }
        else
        {
            UpdateProgress("Door_Found");
        }
    }

    public void Password_Lock_Found()
    {
        if ((bool)progress["Password_Lock_Found"])
        {
            return;
        }
        else
        {
            UpdateProgress("Password_Lock_Found");
        }
    }

    public void Pig_Pen_Solved()
    {
        if ((bool)progress["Pig_Pen_Solved"])
        {
            return;
        }
        else
        {
            UpdateProgress("Pig_Pen_Solved");
        }
    }

    public void Ball_Found()
    {
        if ((bool)progress["Ball_Found"])
        {
            return;
        }
        else
        {
            UpdateProgress("Ball_Found");
        }
    }

    public void Ball_Thrown()
    {
        if ((bool)progress["Ball_Thrown"])
        {
            return;
        }
        else
        {
            UpdateProgress("Ball_Thrown");
        }
    }

    public void Ball_Hit_Wicket()
    {
        if ((bool)progress["Ball_Hit_Wicket"])
        {
            return;
        }
        else
        {
            UpdateProgress("Ball_Hit_Wicket");
        }
    }

    public void Safe_Found()
    {
        if ((bool)progress["Safe_Found"])
        {
            return;
        }
        else
        {
            UpdateProgress("Safe_Found");
        }
    }

    public void Safe_Opened()
    {
        if ((bool)progress["Safe_Opened"])
        {
            return;
        }
        else
        {
            UpdateProgress("Safe_Opened");
        }
    }

    public void Gold_Key_Found()
    {
        if ((bool)progress["Gold_Key_Found"])
        {
            return;
        }
        else
        {
            UpdateProgress("Gold_Key_Found");
        }
    }

    public void Gold_Lock_Opened()
    {
        if ((bool)progress["Gold_Lock_Opened"])
        {
            return;
        }
        else
        {
            UpdateProgress("Gold_Lock_Opened");
        }
    }

    public Hashtable GetProgress()
    {
        return progress;
    }



    public void UpdateProgress(string key)
    {
        // Update the value of the key to true
        progress[key] = true;
        Debug.Log(key + " is set to true");

        // check how many values out of 12 are true in the progress hash map
        int score = 0;
        foreach (string task in taskKeys)
        {
            if ((bool)progress[task])
            {
                score++;
            }
        }




        // Update Firestore
        if (auth.CurrentUser == null)
        {
            Debug.LogError("User not authenticated.");
            return;
        }

        string userId = auth.CurrentUser.UserId;
        DocumentReference userDoc = firestore.Collection("users").Document(userId);

        userDoc.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DocumentSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    List<string> completedTasks = snapshot.TryGetValue("completedTasks", out List<string> tasks) ? tasks : new List<string>();
                    // get the length of the completed tasks list as the task count int
                    int taskCount = completedTasks.Count;


                    if (!completedTasks.Contains(key))
                    {
                        taskCount++;
                        // Calculate score
                        int taskIndex = Array.IndexOf(taskKeys, key);
                        if (taskIndex >= 0)
                        {

                            Timestamp startTime = snapshot.GetValue<Timestamp>("startTime");
                            DateTime startDateTime = startTime.ToDateTime();
                            DateTime currentDateTime = DateTime.UtcNow;
                            double elapsedTime = (currentDateTime - startDateTime).TotalSeconds;

                            int scoreIncrement = fixedValues[taskIndex] + Mathf.FloorToInt((float)numerators[taskIndex] / (float)elapsedTime);

                            // Update score and completed tasks
                            int currentScore = snapshot.TryGetValue("score", out int score) ? score : 0;
                            currentScore += scoreIncrement;
                            completedTasks.Add(key);

                            // Update Firestore
                            userDoc.UpdateAsync(new Dictionary<string, object>
                            {
                                { "score", currentScore },
                                { "completedTasks", completedTasks }
                            }).ContinueWithOnMainThread(updateTask =>
                            {
                                if (updateTask.IsCompleted)
                                {
                                    Debug.Log($"Task {key} completed and score updated by {scoreIncrement}.");
                                    bool allTasksCompleted = true;
                                    foreach (string task in taskKeys)
                                    {
                                        if (!completedTasks.Contains(task))
                                        {
                                            allTasksCompleted = false;
                                            break;
                                        }
                                    }
                                    // If all tasks are completed, load Scene 4
                                    if (allTasksCompleted)
                                    {
                                        Debug.Log("All tasks completed! Moving to Scene 4.");
                                        SceneManager.LoadSceneAsync(4);
                                    }
                                }
                                else
                                {
                                    Debug.LogError("Failed to update Firestore: " + updateTask.Exception);
                                }
                            });
                        }
                    }
                    else
                    {
                        Debug.Log($"Task {key} was already completed.");
                    }

                    // Update the score
                    scoreOutOfText.text = taskCount + "/12";
                }
                else
                {
                    Debug.LogError("User document does not exist.");
                }
            }
            else
            {
                Debug.LogError("Failed to fetch user document: " + task.Exception);
            }
        });
    }
}
