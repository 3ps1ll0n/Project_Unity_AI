using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musiques, bruitages;
    public AudioSource sourceMusique, sourceBruitage;

    private void Awake()
    {
        // Créer Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start(){
        JouerMusique("Menu");
    }

    public void JouerMusique(string nom){
        Sound s = Array.Find(musiques, x => x.nom == nom);

        if(s==null){
            Debug.Log("Musique introuvable!");
        }
        else{
            sourceMusique.clip = s.clip;
            sourceMusique.Play();
        }
    }

    public void JouerBruitage(string nom){
        Sound s = Array.Find(bruitages, x => x.nom == nom);

        if(s==null){
            Debug.Log("Bruitage introuvable!");
        }
        else{
            sourceBruitage.clip = s.clip;
            sourceBruitage.Play();
        }
    
    }
}
