using UnityEngine;

public class Pi�ge : MonoBehaviour
{
    // Tableau pour stocker les r�f�rences � tous les GameObjects des joueurs
    public GameObject[] joueurs;

    /// <summary>
    /// Start est appel� avant la premi�re frame update
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
    /// OnTriggerEnter2D est appel� lorsque l'objet entre en collision avec un trigger 2D
    /// </summary>
    /// <param name="other">Le collider de l'objet entrant en collision</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var j in joueurs)
        {
            if (other.gameObject == j)
            {
                Debug.Log($"Le joueur {j.name} a �t� tu� par un pi�ge !");
                // D�sactive le joueur
                j.SetActive(false);

                // Fait r�appara�tre le joueur apr�s 3 secondes
                Invoke(nameof(ReapparaitreJoueur), 3f);
            }
        }
    }

    /// <summary>
    /// R�appara�t le joueur � sa position initiale
    /// </summary>
    void ReapparaitreJoueur()
    {
        foreach (var j in joueurs)
        {
            // R�appara�t le joueur � sa position initiale
            j.transform.position = j.GetComponent<Mouvement>().positionInitiale;

            // R�active le joueur
            j.SetActive(true);
        }
    }
}


