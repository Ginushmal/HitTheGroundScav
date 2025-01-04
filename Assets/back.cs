using UnityEngine;
using UnityEngine.SceneManagement;
public class back : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void backToMain(){
        SceneManager.LoadSceneAsync(0);
    }
}
