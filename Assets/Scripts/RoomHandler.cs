using TMPro;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    public PersonalRandmValueManager personalRandmValueManager;
    private string correctPassword;
    private string enteredPassword = "";

    public TMP_InputField passwordInputField;

    public Animator animator;

    public PrograssTracker prograssTracker;

    void Start()
    {
        correctPassword = personalRandmValueManager.GetRandomValue("PigPen_password").ToString();

    }

    public void EnterPassword(string password)
    {
        // convert the password to uppercase and assign it to enteredPassword
        enteredPassword = password.ToUpper();
        if (enteredPassword == correctPassword)
        {
            Debug.Log("Correct Password");
            animator.SetBool("PasswordCorrect", true);
            prograssTracker.Pig_Pen_Solved();
        }
        else
        {
            Debug.Log("Entered Password: " + enteredPassword + " Correct Password: " + correctPassword);
            Debug.Log("Incorrect Password");
        }

        passwordInputField.gameObject.SetActive(false);
    }
}