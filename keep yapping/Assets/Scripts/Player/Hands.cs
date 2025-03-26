using UnityEngine;

public class Hands : MonoBehaviour
{
    public Transform player;       // Reference to the player's transform
    public Transform cameraTransform; // Reference to the camera's transform
    public float rotationSmoothTime = 0.1f; // Smooth time for the rotation
    public Vector3 offset; // Offset of the hands from the camera

    private Vector3 velocity = Vector3.zero; // Velocity for smooth movement
    private Quaternion targetRotation; // Target rotation for the hands
    private Quaternion currentRotation; // Current rotation of the hands

    void Start()
    {
        // Set the initial offset (if needed)
        offset = transform.position - cameraTransform.position;
    }

    void Update()
    {
        // Follow the player's position with no delay
        transform.position = player.position;

        // Calculate the desired rotation based on the camera's X and Y rotation
        targetRotation = Quaternion.Euler(cameraTransform.eulerAngles.x, cameraTransform.eulerAngles.y, 0);

        // Smoothly interpolate the rotation with some delay
        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSmoothTime);

        // Apply the rotation to the hands, but around the camera's position
        transform.position = cameraTransform.position + currentRotation * offset;
        transform.rotation = currentRotation;
    }
}
