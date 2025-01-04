using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class DebugLogManager : MonoBehaviour
{
    private Queue<string> debugMessages = new Queue<string>(); // To store the last messages
    public const int maxMessages = 15; // Maximum lines to display
    public TextMeshProUGUI debugText; // Reference to TextMeshPro component

    /// <summary>
    /// Adds a message to the debug log and updates the TextMeshPro display.
    /// </summary>
    /// <param name="message">The message to add to the log.</param>
    public void AddMessage(string message)
    {
        // Add the new message to the queue
        debugMessages.Enqueue(message);

        // Remove the oldest message if the queue exceeds the maximum allowed
        if (debugMessages.Count > maxMessages)
        {
            debugMessages.Dequeue();
        }

        // Update the TextMeshPro text to show the latest messages
        debugText.text = string.Join("\n", debugMessages);
    }
}
