
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static PlayerController _instance; 
  public  static PlayerController GetInstance => _instance; 
    [SerializeField] CharacterController playerController;
    [SerializeField]
    Vector3 inputVector;
    [SerializeField] float movementSpeed;
    [SerializeField] float speed;
    float gravity = -9.81f;
    RaycastHit hitInfo;
    Ray ray;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
  [SerializeField] Vector3 startPos;
    Vector3 gravityVector;
    bool isGrounded;
    private void Update()
    {
        GetInput();
        if (Input.GetMouseButtonDown(0))
            Interact();
    }
    private void Interact()
    {
        // show for a second the hand 
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, 20f) && hitInfo.collider.transform.TryGetComponent<InputButton>(out InputButton inpt))
            inpt.PushTheButton();
    }

    public void GetInput()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && gravityVector.y <0)
            gravityVector.y = -2f;
        
            inputVector = transform.right * Input.GetAxis("Horizontal") +
            transform.forward * Input.GetAxis("Vertical");

        inputVector *= movementSpeed * Time.deltaTime;
        
        if (playerController != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                playerController.Move(inputVector * speed);
            else
            playerController.Move(inputVector);


            gravityVector.y += gravity * Time.deltaTime;
            playerController.Move(gravityVector * Time.deltaTime);
        }
    }

    private void Awake()
    {
        _instance = this;
    }


    public void RestartPosition() {

        transform.position = startPos;
    }

}
