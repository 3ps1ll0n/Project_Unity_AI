using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mouvement : MonoBehaviour
{
    //*========================{PUBLIC}========================
    public float forceDeSaut;
    public float vitesseDeplacement;
    public Rigidbody2D rb;
    public Transform VerifierSolGauche;
    public Transform VerifierSolDroite;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public int nombreSaut;

    //*========================{PRIVATE}========================
    private bool aSaute;
    private bool auSol;
    private static bool canMove = true;
    private bool repos = false;
    private Vector3 velocite = Vector3.zero;
    public Vector3 positionInitiale;

    private void Awake()
    {
        positionInitiale = this.transform.position;
    }
    public static void setCanMove(bool move) // Emp�cher le mouvement du personnage lorsque la fen�tre pour sauvegarder est ouverte
    {
        canMove = move;
    }
    public void setCanMoveQuitter() // Re permettre le mouvement du personnage quand la fen�tre se ferme
    {
        Mouvement.canMove = true;
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        auSol = Physics2D.Raycast(VerifierSolGauche.position, Vector2.down, 0.01f);
        //Debug.Log(VerifierSolDroite.position + " | " + VerifierSolGauche.position);
        float mouvementHorizontal = 0f;
        canMove = true;
        if (Mouvement.canMove) // Si la fen�tre sauvegarde est pas ouverte
        {
         if (Input.GetKey(KeyCode.R)){
                    this.transform.position = positionInitiale;
           
                }
                //Controlleur
                if (Input.GetKey(KeyCode.D))
                {
                    mouvementHorizontal = vitesseDeplacement * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    mouvementHorizontal = -vitesseDeplacement * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.Space)){
                    Debug.Log("HERE");
                    if (auSol){
                        //AudioManager.Instance.JouerBruitage("Saut");
                        aSaute = true;
                      }
                    if (nombreSaut > 0 && rb.velocity.y <= 0){
                        //AudioManager.Instance.JouerBruitage("DoubleSaut");
                        aSaute = true;
                      }
                }
        }


        repos = false;

        if (auSol)
        {
            if (nombreSaut != 2) repos = true;
            nombreSaut = 2;
        }
        deplacerJoueur(mouvementHorizontal);
        animator.SetFloat("Vitesse", Math.Abs(rb.velocity.x));
        animator.SetInteger("nbreSaut", nombreSaut);
        animator.SetBool("aSaute", !auSol);
        animator.SetBool("Repos", repos);
        Flip(rb.velocity.x);
    }

    void deplacerJoueur(float _mouvementHorizontal)
    {
        Vector3 velociteCible = new Vector2(_mouvementHorizontal, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, velociteCible, ref velocite, .05f);

        if (aSaute)
        {
            rb.velocity = new Vector3(0.0f, forceDeSaut);
            aSaute = false;
            auSol = false;
            nombreSaut--;
        }
    }

    void Flip(float _vitesse)
    {
        if (_vitesse > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (_vitesse < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }
}

