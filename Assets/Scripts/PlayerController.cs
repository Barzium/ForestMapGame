
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController playerController;
    Vector3 inputVector;
    [SerializeField] float movementSpeed;
    float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 gravityVector;
    bool isGrounded;
    private void Update()
    {
        GetInput();
      
    }
    public void GetInput()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && gravityVector.y <0)
        {
            gravityVector.y = -2f;
        }
            inputVector = transform.right * Input.GetAxis("Horizontal") +
            transform.forward * Input.GetAxis("Vertical");

        inputVector *= movementSpeed*Time.deltaTime;
        
        if (playerController != null)
        {
            playerController.Move(inputVector);
            gravityVector.y += gravity * Time.deltaTime;
            playerController.Move(gravityVector * Time.deltaTime);
        }
    }



}
