using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;
    public Transform orientation;

    float xRotation;
    float yRotation;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void LateUpdate()
    {
        // Use the new Input System to get mouse delta
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        // Apply sensitivity and time delta
        float mouseX = mouseDelta.x * Time.deltaTime * sensX;
        float mouseY = mouseDelta.y * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 51f);

        // Apply rotations to the camera and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void SyncCameraRotation()
    {
        Vector3 newEuler = transform.eulerAngles;
        xRotation = newEuler.x;
        yRotation = newEuler.y;
        orientation.rotation = Quaternion.Euler(0, newEuler.y, 0);
    }
}