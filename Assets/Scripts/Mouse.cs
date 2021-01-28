using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{

    [SerializeField] float rotationSpeed, rotationUpAndDownLimit;
    float mouseY, mouseX, rotationX;

    [SerializeField] Transform playerBody;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
    }
    void Rotation()
    {
        mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSpeed;
        mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -rotationUpAndDownLimit, rotationUpAndDownLimit);



       transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
