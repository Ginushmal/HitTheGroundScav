using System.Collections;
using TMPro;
using UnityEngine;

public class RaycastHandler : MonoBehaviour
{
    // textmesh pro input field as a public variable name passwordInputField
    public TMP_InputField passwordInputField;

    public PrograssTracker prograssTracker;

    public BallHandler ballHandler;

    public SafeHandler safeHandler;

    public GoldKeyHandler GoldKeyHandler;

    // Start is called before the first frame update
    void Start()
    {
        // set the passwordInputField inactive
        passwordInputField.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for touch input or mouse click (useful for testing in editor)
        if (Input.GetMouseButtonDown(0)) // 0 = left mouse button or single touch
        {
            Ray ray;
#if UNITY_EDITOR
            // In the editor, cast a ray from the camera through the mouse position
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#else
            // On a device, cast a ray from the camera through the touch position
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
#endif

            // Perform the raycast
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Check if the object hit has the target tag or name
                if (hit.collider.gameObject.CompareTag("PasswordTypeThing"))
                {
                    Debug.Log("Target object touched: " + hit.collider.gameObject.name);
                    // set the passwordInputField active
                    passwordInputField.gameObject.SetActive(true);

                    prograssTracker.Password_Lock_Found();

                }
                else if (hit.collider.gameObject.CompareTag("CloseUpBall"))
                {
                    Debug.Log("Target object touched: " + hit.collider.gameObject.name);

                    ballHandler.closeupBallClicked();

                    prograssTracker.Ball_Thrown();



                }
                else if (hit.collider.gameObject.CompareTag("Ball"))
                {
                    Debug.Log("Target object touched: " + hit.collider.gameObject.name);
                    // set the ball to inactive
                    ballHandler.mouth_ball_clicked();

                    prograssTracker.Ball_Found();


                }
                else if (hit.collider.gameObject.CompareTag("SafeKnob"))
                {
                    Debug.Log("Target object touched: " + hit.collider.gameObject.name);
                    safeHandler.KnobClicked(hit.collider.gameObject.name);
                }
                else if (hit.collider.gameObject.CompareTag("CloseSafe"))
                {
                    Debug.Log("Target object touched: " + hit.collider.gameObject.name);
                    safeHandler.closeSafe();
                }
                else if (hit.collider.gameObject.CompareTag("Safe"))
                {
                    Debug.Log("Target object touched: " + hit.collider.gameObject.name);
                    prograssTracker.Safe_Found();
                    safeHandler.safeClicked();
                }
                else if (hit.collider.gameObject.CompareTag("SafeWheel"))
                {
                    Debug.Log("Target object touched: " + hit.collider.gameObject.name);
                    safeHandler.safeWheelClicked();
                }
                else if (hit.collider.gameObject.CompareTag("GoldKey"))
                {
                    Debug.Log("Target object touched: " + hit.collider.gameObject.name);
                    GoldKeyHandler.GoldKeyCollected();
                }
                else if (hit.collider.gameObject.CompareTag("GoldLock"))
                {
                    Debug.Log("Target object touched: " + hit.collider.gameObject.name);
                    GoldKeyHandler.clickGoldLock();
                }
            }


        }
    }
}
