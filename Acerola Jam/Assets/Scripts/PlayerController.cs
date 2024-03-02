using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float speed;
   // PlayerControls input;
    PlayerControls controls;
    [SerializeField]Vector3 movement = new Vector3();
    [SerializeField]Camera camera;
    Vector2 move = new Vector2();
    float jumpHeight = 10;
    [SerializeField] float gravityValue = -9.8f;
    [SerializeField]Transform groundCheck;
    LayerMask groundLayer;
    bool isGrounded = false;

    void OnEnable(){
        controls.Enable();
    }
    void Awake(){
        controls = new PlayerControls();

    }
    // Start is called before the first frame update
    void Start(){
        move = new Vector2();
        controls.controls.Move.started += handleInput;
        controls.controls.Move.canceled += ctx => move = new Vector2();
        controls.controls.Jump.performed += handleJump;

        groundLayer = LayerMask.GetMask("Ground");
        if(camera == null){
            camera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position,0.1f,groundLayer);
        Debug.Log(isGrounded);
        movement = camera.transform.forward * move.y + camera.transform.right * move.x;
        movement = Vector3.Normalize(movement);
        movement *= speed; 
        movement.y = 0;
        controller.Move(movement * Time.deltaTime);

        // if(!isGrounded){
        //     move.y += gravityValue * Time.deltaTime;
        //     controller.Move(new Vector3(0,move.y* Time.deltaTime,0) );
        // }
    }


    void handleInput(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        
    }

    void handleJump(InputAction.CallbackContext context){
        
      //  move.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }

  
    void OnDisable(){
        controls.controls.Move.performed -= handleInput;
        controls.Disable();
    }

}
