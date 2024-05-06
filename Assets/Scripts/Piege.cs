using UnityEngine;

public class Trap : MonoBehaviour
{
    // Array to store references to all player GameObjects
    public GameObject[] players;

    void Start()
    {
        // Find all player GameObjects with the "Player" tag
        players = GameObject.FindGameObjectsWithTag("Player");

        // Store initial positions for each player
        foreach (var p in players)
        {
            // You can adjust this based on your game design
            p.GetComponent<Mouvement>().positionInitiale = p.transform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var p in players)
        {
            if (other.gameObject == p)
            {
                Debug.Log($"Player {p.name} has been killed by a trap!");
                // Despawn the player
                p.SetActive(false);

                // Respawn the player after 3 seconds
                Invoke(nameof(RespawnPlayer), 3f);
            }
        }
    }

    void RespawnPlayer()
    {
        foreach (var p in players)
        {
            // Respawn the player at their initial position
            p.transform.position = p.GetComponent<Mouvement>().positionInitiale;

            // Reactivate the player
            p.SetActive(true);
        }
    }
}

