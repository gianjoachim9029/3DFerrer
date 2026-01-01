using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCollect : MonoBehaviour
{
    public int score = 0;
    public ScoreUI scoreUI;
    public GameManager gameManager;

    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public Quaternion startRotation;

    private CharacterController controller;
    private InputSystem_Actions inputActions;

    // Movement variables
    public float speed = 5f;
    public float jumpForce = 10f;
    public float gravity = -9.81f;
    public float rotationSpeed = 10f;
    public bool flipForward = false;
    private Vector3 velocity;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Jump.performed += ctx => Jump();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Save the exact starting transform of the player
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Jump()
    {
        if (controller.isGrounded)
        {
            velocity.y = jumpForce;
            Debug.Log("Jumping");
        }
    }

    void Update()
    {
        // Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 move;
        if (Camera.main != null)
        {
            // Get camera directions, flattened to horizontal plane
            Vector3 forward = Camera.main.transform.forward;
            if (flipForward) forward = -forward;
            forward.y = 0;
            forward.Normalize();
            Vector3 right = Camera.main.transform.right;
            right.y = 0;
            right.Normalize();
            
            move = right * horizontal + forward * vertical;
        }
        else
        {
            // Fallback to character-relative movement
            move = transform.right * horizontal + transform.forward * vertical;
        }
        
        controller.Move(move * speed * Time.deltaTime);

        // Rotate character to face movement direction (only if moving mostly forward/back)
        if (move != Vector3.zero && Mathf.Abs(vertical) > Mathf.Abs(horizontal))
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Gravity
        if (controller.isGrounded)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with " + other.name + " tag: " + other.tag);

        // ✅ Debug log to confirm what the player touches
        Debug.Log("Player touched: " + other.tag);

        if (other.CompareTag("Collectible"))
        {
            score += 10;
            if (scoreUI != null) scoreUI.UpdateScore(score);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("FinishLine"))
        {
            Debug.Log("✅ Finish line triggered!");
            if (gameManager != null) 
            {
                Debug.Log("Calling ShowLevelClear");
                gameManager.ShowLevelClear();
            }
        }

        if (other.CompareTag("DeathZone"))
        {
            Debug.Log("💀 Death zone triggered!");
            if (gameManager != null) 
            {
                Debug.Log("Calling ShowTryAgain");
                gameManager.ShowTryAgain();
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("OnCollisionEnter with " + other.gameObject.name + " tag: " + other.gameObject.tag);
    }

    // Called by GameManager when respawning
    public void Respawn()
    {
        if (controller != null)
        {
            controller.enabled = false; // disable movement collision
            transform.position = startPosition;
            transform.rotation = startRotation;
            controller.enabled = true;
        }
        else
        {
            transform.position = startPosition;
            transform.rotation = startRotation;
        }
    }
}
