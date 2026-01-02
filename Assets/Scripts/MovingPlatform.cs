using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum MoveDirection { LeftRight, UpDown, ForwardBack }

    [Header("Movement Settings")]
    public MoveDirection direction = MoveDirection.LeftRight;
    public float distance = 5f; 
    public float speed = 2f;    

    private Vector3 startPosition;
    private Vector3 lastPosition; 
    private CharacterController playerController;

    void Start()
    {
        startPosition = transform.position;
        lastPosition = transform.position;
    }

    void Update()
    {
        // 1. Calculate Target Position
        float offset = Mathf.Sin(Time.time * speed) * distance;
        Vector3 targetPosition = startPosition;

        if (direction == MoveDirection.LeftRight)
            targetPosition += new Vector3(offset, 0, 0);
        else if (direction == MoveDirection.UpDown)
            targetPosition += new Vector3(0, offset, 0);
        else if (direction == MoveDirection.ForwardBack)
            targetPosition += new Vector3(0, 0, offset);

        // 2. Move the Platform
        transform.position = targetPosition;

        // 3. Calculate how much we actually moved
        Vector3 platformMovement = transform.position - lastPosition;

        // 4. Push Player (Only if we actually moved!)
        if (playerController != null && platformMovement.sqrMagnitude > 0.0001f)
        {
            playerController.Move(platformMovement);
        }

        lastPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<CharacterController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = null;
        }
    }
}