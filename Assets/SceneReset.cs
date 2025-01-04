using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneReset : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Function to reload the current scene
    public void ReloadCurrentScene()
    {
        // Get the name of the current scene and reload it
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
