using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Respawn : MonoBehaviour
{
    public float limite;
 


    private void FixedUpdate()
    {
        if(transform.position.y < limite)
        {
            
        }
    }
}
