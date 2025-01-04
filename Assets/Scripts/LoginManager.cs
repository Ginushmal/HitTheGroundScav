using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro; // For TextMeshPro
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Extensions;

public class FirebaseLoginManager : MonoBehaviour
{
    // Firebase variables
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;

    // UI elements
    public TMP_InputField emailInputField;  // Drag the EmailInputField here
    public TMP_InputField passwordInputField;  // Drag the PasswordInputField here
    public TextMeshProUGUI statusText;  // Drag the StatusText here
    public UnityEngine.UI.Button loginButton;  // Drag the LoginButton here

    // Firebase initialization status
    private Firebase.DependencyStatus dependencyStatus;

    void Start()
    {
        // Initialize Firebase
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                statusText.text = "";
                loginButton.onClick.AddListener(SignInWithEmailAsync); // Attach login method to button
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                statusText.text = "Firebase Initialization Failed.";
            }
        });
    }
    public void HandleBackNavigation()
    {
        // If we're already on the main scene, exit the app

        // Otherwise, load the main scene
        SceneManager.LoadSceneAsync(0);
    }


    // Sign in with email and password
    public void SignInWithEmailAsync()
{
    string email = emailInputField.text.Trim();
    string password = passwordInputField.text.Trim();

    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
    {
        statusText.text = "Email and Password cannot be empty.";
        return;
    }

    statusText.text = "Signing in...";

    auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
    if (task.IsCanceled || task.IsFaulted)
    {
        Debug.LogError("Login failed: " + task.Exception);
        statusText.text = "Login Failed!";
        return;
    }
    // Successful login
    user = task.Result.User;
    Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.Email);
    statusText.text = "Login Successful! Welcome " + user.Email;
    SceneManager.LoadSceneAsync(0);
    });
}


    // Utility function to get detailed error messages
    private string GetErrorMessage(AggregateException exception)
    {
        foreach (Exception inner in exception.Flatten().InnerExceptions)
        {
            Firebase.FirebaseException firebaseEx = inner as Firebase.FirebaseException;
            if (firebaseEx != null)
            {
                var errorCode = (Firebase.Auth.AuthError)firebaseEx.ErrorCode;
                switch (errorCode)
                {
                    case Firebase.Auth.AuthError.MissingEmail:
                        return "Missing Email.";
                    case Firebase.Auth.AuthError.MissingPassword:
                        return "Missing Password.";
                    case Firebase.Auth.AuthError.InvalidEmail:
                        return "Invalid Email Address.";
                    case Firebase.Auth.AuthError.WrongPassword:
                        return "Incorrect Password.";
                    case Firebase.Auth.AuthError.UserNotFound:
                        return "User Not Found.";
                    default:
                        return "Unknown Error: " + firebaseEx.Message;
                }
            }
        }
        return "An error occurred.";
    }
}

