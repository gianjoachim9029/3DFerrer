using UnityEngine;
using System.Collections.Generic;

public class CollectibleManager : MonoBehaviour
{
    [System.Serializable]
    private class CollectibleData
    {
        public GameObject prefab;
        public Vector3 position;
        public Quaternion rotation;
    }

    private List<CollectibleData> collectibles = new List<CollectibleData>();

    void Start()
    {
        // Find all collectibles in the scene and store their data
        GameObject[] found = GameObject.FindGameObjectsWithTag("Collectible");

        foreach (GameObject c in found)
        {
            // Save a *reference copy* of the prefab before it's destroyed
            CollectibleData data = new CollectibleData
            {
                prefab = c, // we’ll instantiate a clone later
                position = c.transform.position,
                rotation = c.transform.rotation
            };
            collectibles.Add(data);
        }
    }

    public void ResetCollectibles()
    {
        // Destroy all existing collectibles in the scene
        foreach (GameObject c in GameObject.FindGameObjectsWithTag("Collectible"))
        {
            Destroy(c);
        }

        // Respawn them using the stored prefab data
        foreach (var data in collectibles)
        {
            if (data.prefab != null)
            {
                Instantiate(data.prefab, data.position, data.rotation);
            }
        }

        Debug.Log("✅ All collectibles respawned successfully!");
    }
}
