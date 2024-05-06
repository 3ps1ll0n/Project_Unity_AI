using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    public float limite;

    private void FixedUpdate()
    {
        if(transform.position.y < limite)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
