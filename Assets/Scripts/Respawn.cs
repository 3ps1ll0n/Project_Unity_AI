using UnityEngine;
//Ram�ne le joueur � la position 0,0,0 si il tombe en dessous de la limite d�finie
public class Respawn : MonoBehaviour
{
    public float limite; //Limite en dessous de laquelle le joueur est respawn
    private void FixedUpdate()
    {
        if(transform.position.y < limite)
        {
            transform.position = new Vector3(0, 0, 0); //Position de respawn
        }
    }
}
