
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController playerController;
    Vector3 inputVector;
    [SerializeField] float movementSpeed;
    float gravity = -9.81f;

    Vector3 gravityVector;
    private void Update()
    {
        GetInput();
      
    }
    public void GetInput()
    {
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
