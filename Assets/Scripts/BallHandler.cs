using System.Collections;
using UnityEngine;

public class BallHandler : MonoBehaviour
{

    public GameObject mouthBall;
    public GameObject closeupBall;

    public GameObject wicket;

    public GameObject wallPiece;

    public PrograssTracker prograssTracker;

    public void mouth_ball_clicked()
    {
        mouthBall.SetActive(false);
        // set the closeUpBall to active
        closeupBall.SetActive(true);
    }

    public void closeupBallClicked()
    {
        closeupBall.GetComponent<Animator>().SetBool("BallThrown", true);
        // Start the coroutine to wait 1.5 seconds before setting the animation parameter to false
        StartCoroutine(ResetBallAnimation(closeupBall.GetComponent<Animator>()));
    }

    // Coroutine to wait 1.5 seconds before resetting the animation
    IEnumerator ResetBallAnimation(Animator animator)
    {
        // Wait for 1.5 seconds
        yield return new WaitForSeconds(0.7f);

        // Set the animation parameter to false
        animator.SetBool("BallThrown", false);
        // Set the ball to inactive
        closeupBall.SetActive(false);
        mouthBall.SetActive(true);

    }

    public void Ball_Hit_Wicket()
    {
        // set the ball to inactive
        mouthBall.SetActive(false);
        // trigger the wikets animation Parameter to true
        wicket.GetComponent<Animator>().SetTrigger("WicketHit");

        // deactivate collider of the wicket and the rigidbody of the wicket
        wicket.GetComponent<Collider>().enabled = false;


        wallPiece.GetComponent<Animator>().SetTrigger("WallBreak");

        prograssTracker.Ball_Hit_Wicket();
    }
}
