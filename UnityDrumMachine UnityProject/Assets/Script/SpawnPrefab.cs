using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
    public GameObject[] prefabs; // Array of prefabs to spawn
    public bool spawnEnabled = true;

    private void Update()
    {
        // Check for number keys
        for (int i = 1; i <= prefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i) && spawnEnabled)
            {
                // Get the mouse position in world coordinates
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0; // Ensure z position is 0 in 2D

                GameObject prefab = prefabs[i - 1]; // Index adjustment
                Instantiate(prefab, mousePosition, Quaternion.identity);
            }
        }
    }

    public void ChangeSpawnEnabled(bool enable)
    {
        spawnEnabled = enable;
    }
}
