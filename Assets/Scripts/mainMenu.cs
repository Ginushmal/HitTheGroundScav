using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class mainMenu : MonoBehaviour
{
    public Button playButton;
    public Button loginButton;
    public TextMeshProUGUI welcomeText;


    private Firebase.Auth.FirebaseAuth auth;
    private FirebaseFirestore firestore;
    
    void Start()
    {
        // Initialize Firebase
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                firestore = FirebaseFirestore.DefaultInstance;
                UpdateUI();
            }
            else
            {
                Debug.LogError("Could not resolve Firebase dependencies.");
            }
        });
    }

    private void UpdateUI()
    {
        if (auth == null)
        {
            Debug.LogError("Firebase Auth not initialized.");
            return;
        }

        // Check if the user is logged in
        var currentUser = auth.CurrentUser;
        if (currentUser != null)
        {
            // User is logged in
            playButton.gameObject.SetActive(true);
            loginButton.gameObject.SetActive(false);

            // Update welcome message
            FetchUserNameAndUpdateUI(currentUser.UserId);
        }
        else
        {
            // User is not logged in
            playButton.gameObject.SetActive(false);
            loginButton.gameObject.SetActive(true);

            // Clear welcome message
            welcomeText.text = "Welcome! Please log in.";
        }
    }
    private void FetchUserNameAndUpdateUI(string userId)
    {
        DocumentReference userDoc = firestore.Collection("users").Document(userId);

        userDoc.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DocumentSnapshot snapshot = task.Result;

                if (snapshot.Exists && snapshot.TryGetValue("name", out string name))
                {
                    welcomeText.text = $"Welcome, {name}!";
                }
                else
                {
                    Debug.LogError("User document does not exist or 'name' field is missing.");
                    welcomeText.text = "Welcome!";
                }
            }
            else
            {
                Debug.LogError("Failed to fetch user document: " + task.Exception);
                welcomeText.text = "Welcome!";
            }
        });
    }
    public void login(){
        SceneManager.LoadSceneAsync(1);
    }
    
    public void play()
    {
        if (auth == null || auth.CurrentUser == null)
        {
            Debug.LogError("User not authenticated!");
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
                    // Check for 'startTime' field
                    if (!snapshot.TryGetValue("startTime", out Timestamp startTime) || startTime.ToDateTime() > DateTime.UtcNow)
                    {
                        // Set 'startTime' to current time if missing or invalid
                        Timestamp currentTimestamp = Timestamp.FromDateTime(DateTime.UtcNow);

                        userDoc.UpdateAsync(new Dictionary<string, object>
                        {
                            { "startTime", currentTimestamp }
                        }).ContinueWithOnMainThread(updateTask =>
                        {
                            if (updateTask.IsCompleted)
                            {
                                Debug.Log("Start time recorded successfully.");
                                // Load the game scene
                                SceneManager.LoadScene(3);
                            }
                            else
                            {
                                Debug.LogError("Failed to update start time: " + updateTask.Exception);
                            }
                        });
                    }
                    else
                    {
                        // 'startTime' is valid; proceed to load the game scene
                        SceneManager.LoadScene(3);
                    }
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
    public void leaderboard(){
        SceneManager.LoadSceneAsync(2);
    }
}
