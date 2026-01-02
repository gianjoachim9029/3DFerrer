using UnityEngine;
using System.Collections;

public class PlatformDestroyer : MonoBehaviour
{
    [Header("Destruction Settings")]
    public float destroyTime = 3f; // Seconds before breaking

    private Coroutine destructionCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player stepped on: Start the timer
            destructionCoroutine = StartCoroutine(CountDownToDestroy());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player jumped off: Cancel the timer
            if (destructionCoroutine != null)
            {
                StopCoroutine(destructionCoroutine);
                destructionCoroutine = null;
            }
        }
    }

    private IEnumerator CountDownToDestroy()
    {
        // Wait...
        yield return new WaitForSeconds(destroyTime);

        // Break!
        Destroy(gameObject);
    }
}