using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"✅ TRIGGER DETECTED: {other.name} with tag {other.tag}");
    }
}
