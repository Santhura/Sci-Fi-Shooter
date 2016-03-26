using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [Header("movement variable")]
    public float speed;                                // The speed that the player walks with                                        
    public float jumpForce;                            // force of how high the player jumps
    public float gravity;                              // gravity
    private Vector3 _moveDirection;                    // Direction in which the player is moving
    private CharacterController _controller;           // Charactercontroller

    [Header("Input")]
    public string horizotalInput;                      // horizontal input
    public string verticalInput;                       // vertical input
    public string jumpButtonInput;                     // jump button input
    public string mouseXInput;                         // mouse X input
    public string mouseYInput;                         // mouse Y input
    public string lookXInput;                          // controller input for looking X axis
    public string lookYInput;                          // controller input for looking Y axis

    [Header("Mouse variables")]
    public float mouseSensitivity = 100;               // sensitivity of the mouse 
    public float clampAngle = 80;                      // angle to how far you can rotate
    private float _rotY = 0.0f;                        // Y rotation
    private float _rotX = 0.0f;                        // X rotation


	// Use this for initialization
	void Start () {
        _controller = GetComponent<CharacterController>();
        _moveDirection = Vector3.zero;
        Vector3 rot = transform.localRotation.eulerAngles;
        _rotY = rot.y;
        _rotX = rot.x;
        //Cursor.visible = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Movement(horizotalInput, verticalInput, jumpButtonInput);
        FPLook(mouseXInput, mouseYInput);
	}

    /// <summary>
    /// Method for the movement of the player and gravity
    /// </summary>
    /// <param name="horizotalInput"></param>
    /// <param name="verticalInput"></param>
    /// <param name="jumpButton"></param>
    void Movement(string horizotalInput, string verticalInput, string jumpButton)
    {
        if (_controller.isGrounded)
        {
            // get x and z axis input
            _moveDirection = new Vector3(Input.GetAxis(horizotalInput), 0, Input.GetAxis(verticalInput));
            // change the transform from local space to world space
            _moveDirection = transform.TransformDirection(_moveDirection);
            // move the player times the speed
            _moveDirection *= speed;

            if (Input.GetButton(jumpButton))
            {
                // when the jump button is pressed, player jumps
                _moveDirection.y = jumpForce;
            }
        }
        // set the gravity for the player
        _moveDirection.y -= gravity * Time.deltaTime;
        //move the character around
        _controller.Move(_moveDirection * Time.deltaTime);
    }

    /// <summary>
    /// First person mouse input to rotate
    /// </summary>
    /// <param name="mouseXInput"></param>
    /// <param name="mouseYInput"></param>
    void FPLook(string mouseXInput, string mouseYInput)
    {
        // get mouse input
     /*   float mouseX = Input.GetAxis(mouseXInput);
        float mouseY = Input.GetAxis(mouseYInput);

        // rotate along the x and y axis
        _rotY += mouseX * mouseSensitivity * Time.deltaTime;
        _rotX += -mouseY * mouseSensitivity * Time.deltaTime;*/

        // controller input
        float controllerX = Input.GetAxis(lookXInput);
        float controllerY = Input.GetAxis(lookYInput);

        _rotY += controllerX * mouseSensitivity * Time.deltaTime;
        _rotX += -controllerY * mouseSensitivity * Time.deltaTime;

        // can't rotate futher then 90 degrees up and down
        _rotX = Mathf.Clamp(_rotX, -clampAngle, clampAngle);

        // set the new local rotation and rotate the character
        Quaternion localRotationController = Quaternion.Euler(_rotX, _rotY, 0.0f);
        Camera.main.transform.rotation = localRotationController;

        Quaternion localRotationPlayer = Quaternion.Euler(0, _rotY, 0.0f);
        transform.rotation = localRotationPlayer;
    }
}