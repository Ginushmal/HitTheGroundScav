using System;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;
using System.Collections.Generic;

public class CodeValidator : MonoBehaviour
{
    public TMP_InputField codeInputField; // Assign in the Unity Inspector
    public Button submitButton; // Assign in the Unity Inspector
    public TextMeshProUGUI feedbackText; // Assign in the Unity Inspector

    private FirebaseAuth auth;
    private FirebaseFirestore firestore;

    // Codes and scoring rules
    private readonly string[] stageCodes = { "pen", "sword", "fish", "bread" };
    private readonly int[] fixedScores = { 1000, 2000, 2000, 3000 };
    private readonly int[] timeDivisors = { 500000, 1000000, 1000000, 1000000 };

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        firestore = FirebaseFirestore.DefaultInstance;

        submitButton.onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
        string enteredCode = codeInputField.text.Trim();

        if (string.IsNullOrEmpty(enteredCode))
        {
            feedbackText.text = "Please enter a code word!";
            return;
        }
        var userId = auth.CurrentUser.UserId;
        ValidateAndProcessCode(userId, enteredCode);
    }

    private void ValidateAndProcessCode(string userId, string enteredCode)
    {
        DocumentReference userDoc = firestore.Collection("users").Document(userId);

        userDoc.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DocumentSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    // Fetch current state
                    int score = snapshot.GetValue<int>("score");
                    int currentStage = snapshot.GetValue<int>("currentState");
                    Timestamp startTime = snapshot.GetValue<Timestamp>("startTime");

                    // Check if entered code matches current stage
                    if (currentStage < stageCodes.Length && enteredCode.Equals(stageCodes[currentStage], StringComparison.OrdinalIgnoreCase))
                    {
                        // Calculate time taken
                        DateTime startDateTime = startTime.ToDateTime();
                        DateTime currentDateTime = DateTime.UtcNow;
                        double timeTakenSeconds = (currentDateTime - startDateTime).TotalSeconds;

                        // Calculate new score
                        int additionalScore = Mathf.FloorToInt((float)fixedScores[currentStage] + (timeDivisors[currentStage] / (float)timeTakenSeconds));
                        score += additionalScore;

                        // Update Firestore
                        int nextStage = currentStage + 1;
                        Timestamp newStartTime = Timestamp.FromDateTime(DateTime.UtcNow);

                        userDoc.UpdateAsync(new Dictionary<string, object>
                        {
                            { "score", score },
                            { "currentState", nextStage },
                            { "startTime", newStartTime }
                        }).ContinueWithOnMainThread(updateTask =>
                        {
                            if (updateTask.IsCompleted)
                            {
                                feedbackText.text = "Correct! Stage completed. Moving to the next stage!";
                            }
                            else
                            {
                                feedbackText.text = "Failed to update progress. Try again.";
                                Debug.LogError("Failed to update Firestore: " + updateTask.Exception);
                            }
                        });
                    }
                    else
                    {
                        feedbackText.text = "Incorrect code word! Try again.";
                    }
                }
                else
                {
                    feedbackText.text = "User data not found!";
                    Debug.LogError("User document does not exist.");
                }
            }
            else
            {
                feedbackText.text = "Failed to fetch user data!";
                Debug.LogError("Error fetching user document: " + task.Exception);
            }
        });
    }
}
