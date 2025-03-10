using UnityEngine;

public class PlayerOrient : MonoBehaviour
{
    // Assign the camera Transform in the Inspector
    public Transform cameraTransform;
    
    // Minimum input threshold to consider the player as moving
    private float inputThreshold = 0.1f;

    void Update()
    {
        // Check if player is providing movement input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) > inputThreshold || Mathf.Abs(vertical) > inputThreshold)
        {
            if (cameraTransform != null)
            {
                // Get the camera's forward direction and ignore vertical rotation
                Vector3 forward = cameraTransform.forward;
                forward.y = 0;

                // Only update rotation if the direction is not zero
                if (forward != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(forward);
                }
            }
        }
    }
}