using UnityEngine;

public class HealthBarBillboard : MonoBehaviour
{
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0); // Flip to face properly
    }
}
