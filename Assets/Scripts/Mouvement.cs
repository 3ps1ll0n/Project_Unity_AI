using System;
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
    public int nombreSaut = 0;
    public float limite;

    //*========================{PRIVATE}========================
    private bool aSaute;
    private bool auSol;

    private bool atteris;
    private static bool canMove = true;
    private bool repos = false;
    private Vector3 velocite = Vector3.zero;
    private float mouvementHorizontal = 0f;
    private float vitesseMax = 5f;
    public Vector3 positionInitiale;

    private void Awake()
    {
        positionInitiale = this.transform.position;
    }

    public static void setCanMove(bool move) // Empecher le mouvement du personnage lorsque la fenetre pour sauvegarder est ouverte
    {
        canMove = move;
    }

    public void setCanMoveQuitter() // Re permettre le mouvement du personnage quand la fenetre se ferme
    {
        Mouvement.canMove = true;
    }
    public void respawn()
    {
        repos = true;
        transform.position = positionInitiale;
        Time.timeScale = 1f;

    }

    /// <summary>
    /// Update du personnage avec les controles du joueur
    /// </summary>
    void FixedUpdate()
    {
        if (transform.position.y < limite)
        {
            respawn();

        }


        auSol = Physics2D.Raycast(VerifierSolGauche.position, Vector2.down, 0.01f);
        //Debug.Log(VerifierSolDroite.position + " | " + VerifierSolGauche.position);
        if (Mouvement.canMove) // Si la fen�tre sauvegarde est pas ouverte
        {
        
                //Controlleur
                if (Input.GetKey(KeyCode.D))
                {
                    bougerDroite();
                }
                if (Input.GetKey(KeyCode.A))
                {
                    bougerGauche();
                }
                if (Input.GetKey(KeyCode.Space)){

                    sauter();

                }
                repos = false;
        }


        if (auSol)
        {
            if(!atteris){
                //AudioManager.instance.JouerBruitage("Atterissage");
                atteris = true;
            }
            if (nombreSaut != 2) repos = true;
            nombreSaut = 2;
        }
        else{
            atteris = false;
        }
            
        
        deplacerJoueur(mouvementHorizontal);
        animator.SetFloat("Vitesse", Math.Abs(rb.velocity.x));
        animator.SetInteger("nbreSaut", nombreSaut);
        animator.SetBool("aSaute", !auSol);
        animator.SetBool("Repos", repos);
        Flip(rb.velocity.x);

        mouvementHorizontal = 0f;
    }

    /// <summary>
    /// Actualise la position du joueur
    /// </summary>
    void deplacerJoueur(float _mouvementHorizontal)
    
    {
        Vector3 velociteCible = new Vector2(_mouvementHorizontal, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, velociteCible, ref velocite, .05f);

        if(rb.velocity.magnitude > vitesseMax)
            rb.velocity = rb.velocity.normalized * vitesseMax;
    
        if (aSaute)
        {
            rb.AddForce(new Vector2(0.0f, forceDeSaut));
            aSaute = false;
            auSol = false;
            nombreSaut--;
        }
  
    }

    /// <summary>
    /// Lance l'animation de flip
    /// </summary>
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

    //*========================={MOUVEMENTS}=========================
    public void bougerGauche(){
        mouvementHorizontal = -vitesseDeplacement * Time.deltaTime;
    }
    public void bougerDroite(){
        mouvementHorizontal = vitesseDeplacement * Time.deltaTime;
    }
    public void sauter(){
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

