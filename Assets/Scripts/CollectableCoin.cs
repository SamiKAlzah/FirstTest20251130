using UnityEngine;

public class CollectableCoin : MonoBehaviour
{
    [SerializeField] private AudioClip coinCollectClip;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float coinValue = 1f;
    [SerializeField] private float collectSoundVolume = 0.7f;

    private bool isCollected = false;

    void Update()
    {
        if (!isCollected)
        {
            // Rotate coin continuously
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && !isCollected)
        {
            CollectCoin();
        }
    }

    void CollectCoin()
    {
        isCollected = true;

        // Play collection sound
        if (coinCollectClip != null)
        {
            AudioSource.PlayClipAtPoint(coinCollectClip, transform.position, collectSoundVolume);
        }

        // You can add coin counter logic here later
        // Example: GameManager.instance.AddCoin(coinValue);

        // Destroy the coin after collection
        Destroy(gameObject);
    }
}