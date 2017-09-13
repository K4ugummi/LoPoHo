// The PlayerController is used to handle userinput

using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    #region Modifier
    private bool isKeyShift = false;    // player is running
    private bool isKeyCtrl = false;
    private bool isKeyAlt = false;
    #endregion
    #region MovementSettings
    [Header("Movement settings:")]
    [SerializeField]
    private float movementSpeed = 3f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;
    #endregion
    #region StaminaSettings
    [Header("Stamina settings:")]
    [SerializeField]
    private float staminaBurnSpeed = 15f;
    [SerializeField]
    private float staminaRegenSpeed = 10f;
    private float maxStaminaAmount = 100f;
    private float currentStaminaAmount;
    [SerializeField]
    private float runningSpeedMult = 1.75f;
    #endregion


    // Private cashing
    private PlayerMotor motor;

    void Start() {
        motor = GetComponent<PlayerMotor>();
    }
    
    void Update() {
        if (PauseMenu.isPauseMenu) {
            motor.Move(Vector3.zero);
            motor.Rotate(Vector3.zero);
            motor.RotateCamera(0f);
            return;
        }
        #region Get Shift-Ctrl-Alt modifier
        // Key "Shift" modifier
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            isKeyShift = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            isKeyShift = false;
        }
        // Key "Ctrl" modifier
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            isKeyCtrl = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl)) {
            isKeyCtrl = false;
        }
        // Key "Alt" modifier
        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            isKeyAlt = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt)) {
            isKeyAlt = false;
        }
        #endregion

        // Calculate movement velocity as a 3D vector
        float _xMovement = Input.GetAxis("Horizontal");
        float _zMovement = Input.GetAxis("Vertical");
        Vector3 _moveHorizontal = transform.right * _xMovement;
        Vector3 _moveVertical = transform.forward * _zMovement;

        // Jump
        if (Input.GetButtonDown("Jump")) {
            motor.Jump(jumpForce);
        }

        // Calculate movement vector
        Vector3 _velocity = (_moveHorizontal + _moveVertical) * (1/Mathf.Sqrt(2)) * movementSpeed;
        
        // Calculate stamina + final movement vector depending on running or not
        if (isKeyShift) {
            if (_zMovement > 0.0005) {
                if (currentStaminaAmount > 0 && (_xMovement != 0f || _zMovement != 0f)) {
                    currentStaminaAmount -= staminaBurnSpeed * Time.deltaTime;
                    if (currentStaminaAmount <= 0) {
                        isKeyShift = false;
                    }
                    _velocity *= runningSpeedMult;
                }
            }
        }
        else {
            currentStaminaAmount += staminaRegenSpeed * Time.deltaTime;
        }
        currentStaminaAmount = Mathf.Clamp(currentStaminaAmount, 0f, 100f);

        // Apply movement
        motor.Move(_velocity);

        if (GUIInventory.isInventory) {
            motor.Rotate(Vector3.zero);
            motor.RotateCamera(0f);
            return;
        }

        // Calculate player rotation as a 3D vector for turning the character
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _rotation = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        // Apply player rotation
        motor.Rotate(_rotation);

        // Calculate camera rotation as a 3D vector for turning the character
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        // Apply camera rotation
        motor.RotateCamera(_cameraRotationX);
    }

    public float GetCurrentStaminaAmount() {
        return currentStaminaAmount;
    }
    public float GetMaxStaminaAmount() {
        return maxStaminaAmount;
    }
    public void ResetStamina() {
        currentStaminaAmount = maxStaminaAmount;
    }
}
