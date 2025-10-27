using UnityEngine;

public class SpinCollectible : MonoBehaviour
{
    public float rotationSpeed = 100f; // degrees per second

    void Update()
    {
        // Rotate around the Y axis
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
