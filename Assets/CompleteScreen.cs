using UnityEngine;
using Firebase.Extensions;
using Firebase.Firestore;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CompleteScreen : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private Firebase.Auth.FirebaseAuth auth;
    private FirebaseFirestore firestore;
    public void backToMain(){
        SceneManager.LoadSceneAsync(0);
    }
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        firestore = FirebaseFirestore.DefaultInstance;
        var currentUser = auth.CurrentUser;
        if (currentUser != null){
            DocumentReference userDoc = firestore.Collection("users").Document(currentUser.UserId);
            userDoc.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DocumentSnapshot snapshot = task.Result;

                    if (snapshot.Exists && snapshot.TryGetValue("score", out int score))
                    {
                        scoreText.text = $"with {score} points";
                    }
                    else
                    {
                        Debug.LogError("User document does not exist or 'score' field is missing.");
                        scoreText.text = "";
                    }
                }
                else
                {
                    Debug.LogError("Failed to fetch user document: " + task.Exception);
                    scoreText.text = "";
                }
            });
        }
    }
}
