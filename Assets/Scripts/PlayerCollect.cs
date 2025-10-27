using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    public int score = 0;
    public ScoreUI scoreUI;
    public GameManager gameManager;

    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public Quaternion startRotation;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Save the exact starting transform of the player
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ✅ Debug log to confirm what the player touches
        Debug.Log("Player touched: " + other.tag);

        if (other.CompareTag("Collectible"))
        {
            score += 10;
            scoreUI.UpdateScore(score);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("FinishLine"))
        {
            Debug.Log("✅ Finish line triggered!");
            gameManager.ShowLevelClear();
        }

        if (other.CompareTag("DeathZone"))
        {
            Debug.Log("💀 Death zone triggered!");
            gameManager.ShowTryAgain();
        }
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
