// The PlayerMotor is used to move the player, according to user inputs
// and environmental effects on the player
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float cameraRotationLimit = 85f;

    // For jumping
    [SerializeField]
    private LayerMask canStandOnMask;
    private float lastJump = 0f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;

    private Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // Gets a movement vector
    public void Move(Vector3 _velocity) {
        velocity.x = _velocity.x;
        velocity.y = rb.velocity.y;
        velocity.z = _velocity.z;
    }

    public void Jump(float _jumpForce) {
        RaycastHit _hit;
        if (Physics.Raycast(transform.position, Vector3.down, out _hit, 1.1f, canStandOnMask)) {
            if (lastJump > .1f) {
                // We are standing on something and can jump now
                rb.AddForce(new Vector3(0, _jumpForce, 0), ForceMode.VelocityChange);
                lastJump = 0f;
            }
        }
    }

    // Gets a rotational vector for the player
    public void Rotate(Vector3 _rotation) {
        rotation = _rotation;
    }

    // Gets a rotational vector for the camera
    public void RotateCamera(float _cameraRotationX) {
        cameraRotationX = _cameraRotationX;
    }

    // Runs every physics iteration
    void FixedUpdate() {
        PerformMovement();
        PerformRotation();
        lastJump += Time.fixedDeltaTime;
    }

    // Perform movement, based on velocity variable
    void PerformMovement() {
        if (velocity != Vector3.zero) {
            // MovePosition() does all the physics colision checks
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    // Perform rotation
    void PerformRotation() {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (cam != null) {
            // Set our rotation and clamp it
            if (cameraRotationX != 0f) {
                currentCameraRotationX -= cameraRotationX;
                currentCameraRotationX = Mathf.Clamp(currentCameraRotationX,
                                                     -cameraRotationLimit,
                                                     cameraRotationLimit);
                // Apply rotation to the transform of the player camera
                cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
            }
        }
    }
}
