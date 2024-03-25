using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject personnage; // Reference to the player GameObject
    private Vector3 initialPosition; // Initial position of the player

    void Start()
    {
        // Store the initial position of the player
        initialPosition = personnage.transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Piege"))
        {
            // Despawn the player
            personnage.SetActive(false);

            // Respawn the player after 3 seconds
            Invoke("RespawnPlayer", 3f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Comportement>() != null)
        {
            // Despawn the player
            personnage.SetActive(false);

            // Respawn the player after 3 seconds
            Invoke("RespawnPlayer", 3f);
        }
    }

    void RespawnPlayer()
    {
        // Respawn the player at the initial position
        personnage.transform.position = initialPosition;

        // Reactivate the player
        personnage.SetActive(true);
    }
}
