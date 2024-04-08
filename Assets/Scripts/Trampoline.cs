using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
  public Animator animator;
   [SerializeField] public float jumpForce = 2f;

   private void OnCollisionEnter2D(Collision2D collision){
    if (collision.transform.CompareTag("Player")){
        
        collision.gameObject.GetComponent<Rigidbody2D>().velocity = (Vector2.up * jumpForce);
        animator.Play("Jump");
    }
   }

}
