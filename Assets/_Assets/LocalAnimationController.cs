using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalAnimationController : MonoBehaviour
{
    [SerializeField] private Transform FirstPersonCameraRoot;
    [SerializeField] private Transform HumanoidModelRoot;
    [SerializeField] private Animator HumanoidAnimator;
    [SerializeField] private  float rotationSpeed = 5.0f;

public bool EnableDebugAnimation;

public float raycastDistance = 1f; // Distance of the raycast from the object's position.
private Quaternion previousRotation;
public float maxRotationSpeed = 360.0f; // Change this value as needed
    public LayerMask groundLayerMask; // The LayerMask to determine which layers the raycast should interact with.
private Vector3 previousCameraRotation;
private Vector3 previousPositionForward;
    private float timeSinceLastUpdate;

    // Call this function to get the velocity relative to the forward direction
    public Vector3 GetRelativeVelocity()
    {
        // Calculate the displacement since the last update
        Vector3 displacement = transform.position - previousPositionForward;

        // Calculate the time since the last update
        float deltaTime = Time.time - timeSinceLastUpdate;

        // Calculate velocity as displacement divided by time
        Vector3 velocity = displacement / deltaTime;

        // Project the velocity onto the forward direction of the transform
        float relativeVelocityForward = Vector3.Dot(velocity, transform.forward);
    float relativeVelocityHort = Vector3.Dot(velocity, transform.right);

        // Update the previous position and time
        previousPositionForward = transform.position;
        timeSinceLastUpdate = Time.time;

        return new Vector3(relativeVelocityHort,0f,relativeVelocityForward);
    }


    public bool CheckRaycastBelow()
    {
        var rayOrigin = Vector3.up + transform.position;
        // Create a raycast from the object's position directly downwards.
        Ray ray = new Ray(rayOrigin, Vector3.down);
        RaycastHit hit;

        // Perform the raycast.
        if (Physics.Raycast(ray, out hit, raycastDistance,groundLayerMask ))
        {
            // If the raycast hits an object, return true.
            return true;
        }

        // If the raycast didn't hit anything, return false.
        return false;
    }

    // Draw the ray in the Scene view for debugging purposes.
    private void OnDrawGizmos()
    {
                var rayOrigin = Vector3.up + transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(rayOrigin, Vector3.down * raycastDistance);
    }

void FixedUpdate()
{
    var groundHit = CheckRaycastBelow();
        HumanoidAnimator.SetBool("IsGrounded",groundHit);
}
void Update()
{
    if (FirstPersonCameraRoot == null || HumanoidModelRoot == null)
        return;

    // Set the humanoid model's rotation to match the camera rotation on the horizontal axis
    Vector3 cameraEulerAngles = FirstPersonCameraRoot.eulerAngles;
    Vector3 humanoidRotation = new Vector3(0, cameraEulerAngles.y, 0);

    // Lerp the current rotation to the target rotation
    Quaternion targetRotation = Quaternion.Euler(humanoidRotation);
    HumanoidModelRoot.rotation = Quaternion.Lerp(HumanoidModelRoot.rotation, targetRotation, rotationSpeed * Time.deltaTime);

 // Calculate the current speed of rotation
    float currentRotationSpeed = Quaternion.Angle(HumanoidModelRoot.rotation, previousRotation) / Time.deltaTime;
    float normalizedRotationSpeed = currentRotationSpeed / maxRotationSpeed;
    
    previousRotation = HumanoidModelRoot.rotation;

    // Store the current camera rotation for the next frame
    previousCameraRotation = cameraEulerAngles;

    if (EnableDebugAnimation)
        return;

    Vector3 velocity = GetRelativeVelocity();
    var zVel = velocity.z;
    var xVel = velocity.x;
    // Debug.Log("zVel: "+zVel);
    // Debug.Log("xVel: "+xVel);
    // Debug.Log("currentRotationSpeed: "+currentRotationSpeed);

    HumanoidAnimator.SetBool("IsCrouching",false);

    if(Mathf.Abs(velocity.magnitude) > 0.1f)
        HumanoidAnimator.SetBool("IsMoving",true);
    else
        HumanoidAnimator.SetBool("IsMoving",false);
        
    if(zVel < 10)
        HumanoidAnimator.SetFloat("ForwardVelocity",zVel);
    if(xVel<10)
        HumanoidAnimator.SetFloat("HorizontalVelocity",xVel);
    HumanoidAnimator.SetFloat("RotationSpeed",currentRotationSpeed);
    
}

}
