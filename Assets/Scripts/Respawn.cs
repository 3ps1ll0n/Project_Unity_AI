using UnityEngine;

public class Respawn : MonoBehaviour
{
    public float limite;
 


    private void FixedUpdate()
    {
        if(transform.position.y < limite)
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }
}
