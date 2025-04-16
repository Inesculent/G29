using UnityEngine;
using System;

[DisallowMultipleComponent]
public class EnemyIdentity : MonoBehaviour
{
    [SerializeField]
    private string uniqueID;

    public string UniqueID => uniqueID;

    private void Awake()
    {
        // Always generate a new ID when entering Play Mode
        if (Application.isPlaying)
        {
            uniqueID = Guid.NewGuid().ToString();
        }
    }

    private void Start()
    {
        Debug.Log($"{gameObject.name} initialized with ID: {uniqueID}");
    }

    private void OnValidate()
    {
        // Optional: assign in editor (just for visualizing uniqueness)
        if (string.IsNullOrEmpty(uniqueID) && Application.isEditor && !Application.isPlaying)
        {
            uniqueID = Guid.NewGuid().ToString();
        }
    }
}
