using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] PlayerData playerData;
    [SerializeField] Transform Camera;

    PlayerRaycast praycast;
    CharacterController ccontroller;

    Vector2 moveInput;
    Vector2 LookInput;
    Vector3 movement;
    float rotationXAxis;
    float rotationYAxis;
    float yVelocity;
    bool isJumpPressed;
    bool isAttackPressed;


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context) {
        if (context.started)
        {
            isAttackPressed = true;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isJumpPressed = true;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ccontroller = GetComponent<CharacterController>();

        //Modulo Raycast
        if (praycast != null) return;
        praycast = GetComponent<PlayerRaycast>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = transform.TransformDirection(move);
        movement = move.normalized * playerData.force;

        //salto
        if (ccontroller.isGrounded)
        {
            yVelocity = -1f;
            if (isJumpPressed)
            {
                yVelocity = playerData.jumpForce;

                isJumpPressed = false;
            }

        }
        else
        {

            yVelocity += playerData.gravity * Time.deltaTime;

        }

        movement.y = yVelocity;
        ccontroller.Move(movement * Time.deltaTime);

        Movimento();
        if (isAttackPressed)
        {
            praycast.Puntatore();
            isAttackPressed=false;
        }
    }


    public void Movimento()
    {

        rotationYAxis += LookInput.x * playerData.sensibility;
        transform.rotation = Quaternion.Euler(0, rotationYAxis, 0);
        rotationXAxis -= LookInput.y * playerData.sensibility;
        rotationXAxis = Mathf.Clamp(rotationXAxis, -playerData.maxAngle, playerData.maxAngle);
        Camera.localRotation = Quaternion.Euler(rotationXAxis, 0, 0);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        var coll = hit.gameObject.GetComponent<IDoors>();
        if (coll != null)
        {
            coll.open();
        }
    }

}