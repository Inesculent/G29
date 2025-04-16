using UnityEngine;

public class PlayerOrient : MonoBehaviour
{
    // Assign the camera Transform in the Inspector
    public Transform cameraTransform;

    void LateUpdate()
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