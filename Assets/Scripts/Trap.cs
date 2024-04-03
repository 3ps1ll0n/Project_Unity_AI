using UnityEngine;

public class Trap : MonoBehaviour
{
    public GameObject player; // Reference to the player GameObject
    private Vector3 initialPosition; // Initial position of the player

    void Start()
    {
        // Find the player GameObject in the scene and store its initial position
        player = GameObject.FindGameObjectWithTag("Player");
        initialPosition = player.transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Player has been killed by a trap!");
            // Despawn the player
            player.SetActive(false);

            // Respawn the player after 3 seconds
            Invoke("RespawnPlayer", 3f);
        }
    }

    void RespawnPlayer()
    {
        // Respawn the player at the initial position
        player.transform.position = initialPosition;

        // Reactivate the player
        player.SetActive(true);
    }
}
