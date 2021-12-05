using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    Animator animator;

    [SerializeField] float speed = 5;
    [SerializeField] float rotationSpeed = 1;

    [SerializeField] float scaleSpeed = 0.1f;

    [SerializeField] Camera camera3;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
     }

    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);
        camera3.transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, 0);

        Vector3 forward = transform.TransformDirection(-Vector3.forward);
        float currentSpeed = speed * Input.GetAxisRaw("Vertical");
        animator.SetFloat("speed", currentSpeed);

        characterController.SimpleMove(forward * transform.localScale.x * currentSpeed);

        if (Input.GetKey(KeyCode.Space))
            CorrectZ();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ScaleIn"))
            transform.localScale += new Vector3(scaleSpeed, scaleSpeed, scaleSpeed) * Time.deltaTime;
        else if (other.CompareTag("ScaleInX"))
            transform.localScale += new Vector3(scaleSpeed, 0, 0) * Time.deltaTime;
        // else if (other.CompareTag("ScaleInY"))
        //     transform.localScale += new Vector3(0, scaleSpeed, 0) * Time.deltaTime;
        // else if (other.CompareTag("ScaleInZ"))
        //     transform.localScale += new Vector3(0, 0, scaleSpeed) * Time.deltaTime;
    }

    void CorrectZ()
    {
        Debug.Log("Player CoorectZ");
        transform.LookAt(transform.TransformDirection(-transform.forward));
    }
}
