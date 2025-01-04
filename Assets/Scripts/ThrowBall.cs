using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    public Rigidbody rb; // Reference to the Rigidbody
    public float throwForce = 10f; // Force multiplier
    public Vector3 throwDirection = new Vector3(1, 1, 0); // Direction of the throw

    public Transform gravityReference; // Reference to the moving object that defines gravity direction
    public float gravityStrength = 9.81f; // Gravity magnitude

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>(); // Get Rigidbody if not assigned
        }

        // Disable gravity at the start to keep the object in place
        rb.useGravity = false;
        rb.isKinematic = true; // Optional: Prevent other physics interactions
    }

    void Update()
    {
        // Throw the object when the space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Throw();
        }

        if (gravityReference != null)
        {
            // Update global gravity direction based on the reference object's "down" direction
            Physics.gravity = -gravityReference.up * gravityStrength;
        }
    }

    void Throw()
    {
        // Re-enable gravity and apply force
        rb.useGravity = true;
        rb.isKinematic = false; // Enable physics interactions
        rb.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);
    }
}
