using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f;
    public float waitTime = 1f; // How long to wait at each point
    public bool faceTarget = true; // Should enemy rotate to face the point?

    [Header("Patrol Points")]
    // Drag your empty GameObjects here in the Inspector
    public Transform[] waypoints;

    private int currentPointIndex = 0;
    private float waitCounter = 0f;
    private bool isWaiting = false;

    void Update()
    {
        // Safety check: Don't do anything if no points are assigned
        if (waypoints.Length == 0) return;

        // 1. Check if we are waiting
        if (isWaiting)
        {
            waitCounter += Time.deltaTime;
            if (waitCounter >= waitTime)
            {
                isWaiting = false;
                waitCounter = 0f;
                // Move to next point (Loop back to 0 if at end)
                currentPointIndex = (currentPointIndex + 1) % waypoints.Length;
            }
            return; // Stop here, don't move while waiting
        }

        // 2. Identify target
        Transform target = waypoints[currentPointIndex];

        // 3. Move towards target
        // MoveTowards ensures we stop EXACTLY at the point
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // 4. Face the target (Optional)
        if (faceTarget)
        {
            transform.LookAt(target);
        }

        // 5. Check if we reached the point
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // We arrived! Start waiting.
            isWaiting = true;
        }
    }
}