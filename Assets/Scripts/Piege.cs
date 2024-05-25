using UnityEngine;

public class Piège : MonoBehaviour
{
    // Tableau pour stocker les références à tous les GameObjects des joueurs
    public GameObject[] joueurs;

    /// <summary>
    /// Start est appelé avant la première frame update
    /// </summary>
    void Start()
    {
        // Trouve tous les GameObjects des joueurs avec le tag "Player"
        joueurs = GameObject.FindGameObjectsWithTag("Player");

        // Stocke les positions initiales pour chaque joueur
        foreach (var j in joueurs)
        {
            // Vous pouvez ajuster cela en fonction de la conception de votre jeu
            j.GetComponent<Mouvement>().positionInitiale = j.transform.position;
        }
    }

    /// <summary>
    /// OnTriggerEnter2D est appelé lorsque l'objet entre en collision avec un trigger 2D
    /// </summary>
    /// <param name="other">Le collider de l'objet entrant en collision</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var j in joueurs)
        {
            if (other.gameObject == j)
            {
                Debug.Log($"Le joueur {j.name} a été tué par un piège !");
                // Désactive le joueur
                j.SetActive(false);

                // Fait réapparaître le joueur après 3 secondes
                Invoke(nameof(ReapparaitreJoueur), 3f);
            }
        }
    }

    /// <summary>
    /// Réapparaît le joueur à sa position initiale
    /// </summary>
    void ReapparaitreJoueur()
    {
        foreach (var j in joueurs)
        {
            // Réapparaît le joueur à sa position initiale
            j.transform.position = j.GetComponent<Mouvement>().positionInitiale;

            // Réactive le joueur
            j.SetActive(true);
        }
    }
}


