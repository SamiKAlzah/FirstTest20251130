using UnityEngine;

public class ResetPoint : MonoBehaviour
{
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform player;

    void Start()
    {
        // Auto-find player if not assigned
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Store initial spawn point if not assigned
        if (playerSpawnPoint == null)
        {
            playerSpawnPoint = transform;
        }
    }
    // Trigger reset on player collision
    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            ResetPlayerPosition();
        }
    }

    void ResetPlayerPosition()
    {
        // Reset player position
        player.position = playerSpawnPoint.position;

        // Reset player velocity
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
        }

        // Reset jump count if you have it
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            // You might need to add a public reset method to PlayerMovement
        }
    }
}