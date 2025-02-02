using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CharacterController : MonoBehaviour
{
    
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 360;
    [SerializeField] private float playerHealth = 90f;
    [SerializeField] private float enemyDamage = 55;
    
    [SerializeField] AudioClip walkSound;

    private Vector3 vectorInput;
    private Rigidbody rb;
    private Animator animator;
    private AudioSource audioSource;

    private bool isMoving;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked; //fare imlecini 


    }

    // Update is called once per frame
    void Update()
    {
        GatherInput();

        Look();

        Sword();

    }

    private void Sword()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Attack", true);
            audioSource.Stop();
           



        }
        else
        {
            animator.SetBool("Attack", false);

        }
    }

    private void FixedUpdate()
    {
        CharacterMovement();
    }

    private void GatherInput()
    {
        vectorInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        
        
    }
    

    private void Look ()
    {
        if (vectorInput != Vector3.zero)
        {
            var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

            var skewedInput = matrix.MultiplyPoint3x4(vectorInput);

            var relative = (transform.position + skewedInput) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);

            animator.SetBool("isMoving", true);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(walkSound);
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            audioSource.Stop();
            Sword();
            
        }
    }

    
    



    void CharacterMovement()
    {
        rb.MovePosition(transform.position + (transform.forward * vectorInput.magnitude) * moveSpeed *Time.deltaTime);        
    }
}
