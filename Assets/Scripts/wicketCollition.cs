using Unity.VisualScripting;
using UnityEngine;

public class wicketCollition : MonoBehaviour
{

    public BallHandler ballHandler;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("wicket got hit by : " + other.gameObject.tag);
        if (other.gameObject.tag == "CloseUpBall")
        {
            ballHandler.Ball_Hit_Wicket();
        }
    }
}
