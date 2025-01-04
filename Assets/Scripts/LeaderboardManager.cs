using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import for TextMeshPro
using Firebase.Firestore; // Import for Firestore
using Firebase.Extensions;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    [System.Serializable]
    public class Participant
    {
        public string userId;
        public string Name;
        public int Score;
    }
    private string currentUserId;

    public Transform content; // Drag the Content object of ScrollView here in the Inspector
    public GameObject participantPrefab; // Drag the ParticipantEntry prefab here in the Inspector

    private FirebaseFirestore firestore;
    private Firebase.Auth.FirebaseAuth auth;
    private List<GameObject> leaderboardEntries = new List<GameObject>();

    public void HandleBackNavigation()
    {
        SceneManager.LoadSceneAsync(0);
    }

    void Start()
    {
        // Initialize Firebase
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                firestore = FirebaseFirestore.DefaultInstance;
                if (auth.CurrentUser != null)
                {
                    currentUserId = auth.CurrentUser.UserId;
                }
                else
                {
                    Debug.LogError("User not authenticated.");
                }
                ListenForLeaderboardUpdates();
            }
            else
            {
                Debug.LogError("Could not resolve Firebase dependencies.");
            }
        });
    }

    // private void ListenForLeaderboardUpdates()
    // {
    //     firestore.Collection("users")
    //         .OrderByDescending("score")
    //         .Limit(10) // Limit to top 10 participants
    //         .Listen(snapshot =>
    //         {
    //             if (snapshot != null && snapshot.Documents.Any())
    //             {
    //                 // Clear existing entries
    //                 foreach (var entry in leaderboardEntries)
    //                 {
    //                     Destroy(entry);
    //                 }
    //                 leaderboardEntries.Clear();

    //                 // Populate leaderboard
    //                 int rank = 1;
    //                 foreach (var document in snapshot.Documents)
    //                 {
    //                     if (document.TryGetValue("name", out string name) &&
    //                         document.TryGetValue("score", out int score))
    //                     {
    //                         AddLeaderboardEntry(rank, name, score);
    //                         rank++;
    //                     }
    //                 }
    //             }
    //             else
    //             {
    //                 Debug.LogError("Leaderboard snapshot is null or empty.");
    //             }
    //         });
    // }

    // private void AddLeaderboardEntry(int rank, string name, int score)
    // {
    //     GameObject entry = Instantiate(participantPrefab, content);
    //     if (entry == null)
    //     {
    //         Debug.LogError("Failed to instantiate ParticipantEntry prefab.");
    //         return;
    //     }

    //     leaderboardEntries.Add(entry);

    //     // Set rank
    //     entry.transform.Find("ranktext").GetComponent<TextMeshProUGUI>().text = rank.ToString();
    //     // Set name
    //     entry.transform.Find("name").GetComponent<TextMeshProUGUI>().text = name;
    //     // Set score
    //     entry.transform.Find("score").GetComponent<TextMeshProUGUI>().text = score.ToString();
    // }
    private void ListenForLeaderboardUpdates()
    {
        firestore.Collection("users")
            .OrderByDescending("score")
            .Limit(20) // Limit to top 10 participants
            .Listen(snapshot =>
            {
                if (snapshot != null && snapshot.Documents.Any())
                {
                    // Clear existing entries
                    foreach (var entry in leaderboardEntries)
                    {
                        Destroy(entry);
                    }
                    leaderboardEntries.Clear();

                    // Populate leaderboard
                    int rank = 1;
                    foreach (var document in snapshot.Documents)
                    {
                        string userId = document.Id; // Document ID corresponds to the user ID
                        string name = document.GetValue<string>("name");
                        int score = document.GetValue<int>("score");

                        AddLeaderboardEntry(rank, name, score, userId == currentUserId);
                        rank++;
                    }
                }
                else
                {
                    Debug.LogError("Leaderboard snapshot is null or empty.");
                }
            });
    }

    private void AddLeaderboardEntry(int rank, string name, int score, bool isCurrentUser)
    {
        GameObject entry = Instantiate(participantPrefab, content);
        if (entry == null)
        {
            Debug.LogError("Failed to instantiate ParticipantEntry prefab.");
            return;
        }

        leaderboardEntries.Add(entry);

        // Set rank
        entry.transform.Find("ranktext").GetComponent<TextMeshProUGUI>().text = rank.ToString();
        // Set name
        entry.transform.Find("name").GetComponent<TextMeshProUGUI>().text = name;
        // Set score
        entry.transform.Find("score").GetComponent<TextMeshProUGUI>().text = score.ToString();

        // Highlight self-entry
        if (isCurrentUser)
        {
            Color selfColor;
            if (ColorUtility.TryParseHtmlString("#911B1B", out selfColor))
            {
                entry.GetComponent<Image>().color = selfColor; // Assumes the prefab has an Image component
            }
        }
    }
}

// using Firebase.Firestore;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
// using Firebase.Extensions;
// using UnityEngine.SceneManagement;

// public class LeaderboardManager : MonoBehaviour
// {
//     [System.Serializable]
//     public class Participant
//     {
//         public string userId;
//         public string Name;
//         public int Score;
//     }

//     private string currentUserId;

//     public Transform content; // Drag the Content object of ScrollView here in the Inspector
//     public GameObject participantPrefab; // Drag the ParticipantEntry prefab here in the Inspector

//     private FirebaseFirestore firestore;
//     private Firebase.Auth.FirebaseAuth auth;
//     private Dictionary<string, GameObject> leaderboardEntries = new Dictionary<string, GameObject>();
//     private List<Participant> participants = new List<Participant>();

//     void Start()
//     {
//         // Initialize Firebase
//         Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
//         {
//             if (task.Result == Firebase.DependencyStatus.Available)
//             {
//                 auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
//                 firestore = FirebaseFirestore.DefaultInstance;
//                 if (auth.CurrentUser != null)
//                 {
//                     currentUserId = auth.CurrentUser.UserId;
//                 }
//                 ListenToLeaderboard();
//             }
//             else
//             {
//                 Debug.LogError("Could not resolve Firebase dependencies.");
//             }
//         });
//     }

//     public void HandleBackNavigation()
//     {
//         SceneManager.LoadSceneAsync(0);
//     }
//     private void ListenToLeaderboard()
//     {
//         firestore.Collection("users")
//             .OrderByDescending("score")
//             .Limit(20) 
//             .Listen(snapshot =>
//         {
//             if (snapshot == null) return;

//             // Create a temporary set to track participants in the snapshot
//             HashSet<string> updatedUserIds = new HashSet<string>();

//             // Iterate through the snapshot
//             foreach (var document in snapshot.Documents)
//             {
//                 string userId = document.Id;
//                 Debug.Log(userId);
//                 string name = document.GetValue<string>("name");
//                 int score = document.GetValue<int>("score");
//                 updatedUserIds.Add(userId);

//                 // Check if the participant already exists
//                 var existingParticipant = participants.Find(p => p.userId == userId);

//                 if (existingParticipant != null)
//                 {
//                     // Update score if it has changed
//                     if (existingParticipant.Score != score)
//                     {
//                         existingParticipant.Score = score;
//                     }
//                 }
//                 else
//                 {
//                     // Add new participant
//                     participants.Add(new Participant
//                     {
//                         userId = userId,
//                         Name = name,
//                         Score = score
//                     });
//                 }
//             }
//             // Sort participants by score
//             participants.Sort((a, b) => b.Score.CompareTo(a.Score));

//             // Update the leaderboard UI
//             UpdateLeaderboard();
//         });
//     }


//     private void UpdateLeaderboard()
//     {
//         // Clear all existing entries in the UI
//         foreach (var entry in leaderboardEntries.Values)
//         {
//             Destroy(entry);
//         }
//         leaderboardEntries.Clear();

//         // Populate the leaderboard
//         for (int i = 0; i < participants.Count; i++)
//         {
//             var participant = participants[i];
//             bool isCurrentUser = participant.userId == currentUserId;

//             // Create or update the leaderboard entry
//             AddLeaderboardEntry(i + 1, participant.Name, participant.Score, isCurrentUser);
//         }
//     }

//     private void AddLeaderboardEntry(int rank, string name, int score, bool isCurrentUser)
//     {
//         GameObject entry = Instantiate(participantPrefab, content);
//         if (entry == null)
//         {
//             Debug.LogError("Failed to instantiate ParticipantEntry prefab.");
//             return;
//         }

//         leaderboardEntries[name] = entry;

//         // Set rank
//         entry.transform.Find("ranktext").GetComponent<TextMeshProUGUI>().text = rank.ToString();
//         // Set name
//         entry.transform.Find("name").GetComponent<TextMeshProUGUI>().text = name;
//         // Set score
//         entry.transform.Find("score").GetComponent<TextMeshProUGUI>().text = score.ToString();

//         // Highlight self-entry
//         if (isCurrentUser)
//         {
//             Color selfColor;
//             if (ColorUtility.TryParseHtmlString("#911B1B", out selfColor))
//             {
//                 entry.GetComponent<Image>().color = selfColor; // Assumes the prefab has an Image component
//             }
//         }
//     }
// }
