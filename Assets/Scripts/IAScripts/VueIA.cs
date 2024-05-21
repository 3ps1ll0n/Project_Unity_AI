using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VueIA : MonoBehaviour{

    //*==================={PUBLIC}===================
    public GameObject joueur;
    public int resolutionHauteur;
    public int resolutionLongueur;
    public int tailleAffichageCellule;
    public int camDecalageY;
    //*==================={PRIVATE}===================
    private Tilemap levelTiledMap;
    private int[,] tilesData;
    private int[,] aiView;
    private bool montrerAI = false;
    private Vector3Int relativeAIView;
    void Start()
    {
        levelTiledMap = GameObject.Find("Terrain").GetComponent<Tilemap>();
        //levelTiledMap = GetComponent<Tilemap>();
        if(levelTiledMap == null) Debug.Log("CAN'T FIND REQUESTED TILEDMAP !");

        for(int i = 0; i < Display.displays.Length; i++){
                Display.displays[i].Activate(Display.displays[i].systemHeight, Display.displays[i].systemWidth, new RefreshRate());
        }
    }

    // Update is called once per frame
    void Update()
    {
        tilesData = getTiledMapData(levelTiledMap);
        aiView = copyTilesDataToView();
        aiView = readTilesObject(getEveryTileObject(levelTiledMap), levelTiledMap, aiView);
        //DessinerData(tilesData);

        if(Input.GetKeyDown("p")) montrerAI = !montrerAI;
        if(Input.GetKeyDown(KeyCode.RightArrow)) {
            Time.timeScale += 0.2f;
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            Time.timeScale -= 0.2f;
        }
    }

    int[,] getTiledMapData(Tilemap map){
        BoundsInt vueTotal = levelTiledMap.cellBounds;

        TileBase[] allTiles = map.GetTilesBlock(vueTotal);

        tilesData = tableauAMatrice(allTiles, vueTotal.size.x, vueTotal.size.y);

        return tilesData;
    }

    //*==================={Matrix Manipulation}===================

    int[,] tableauAMatrice(TileBase[] tab, int longueur, int hauteur){
        int[,] m = new int[hauteur, longueur];

        for(int i = 0; i < hauteur; i++){
            for(int j = 0; j < longueur; j++){
                int valeur = 0;
                if(tab[j + (i * longueur)] != null) {
                    valeur = 1;
                }
                m[m.GetLength(0) - 1 - i, j] = valeur;
            }
        }
        return m;
    }

    int[,] renverserMatrice(int[,] m){
        int[,] invM = new int[m.GetLength(0), m.GetLength(1)];

        

        return invM;
    }

    int[,] copyTilesDataToView(){
        int[,] copiedMatrix = new int[resolutionHauteur, resolutionLongueur];

        float longueurJoueur = joueur.GetComponent<BoxCollider2D>().bounds.size.x;
        float hauteurJoueur = joueur.GetComponent<BoxCollider2D>().bounds.size.y;

        Vector3Int relativeTileMapPos = levelTiledMap.layoutGrid.WorldToCell(levelTiledMap.transform.position);
        relativeTileMapPos.x -= tilesData.GetLength(1)/2;
        relativeTileMapPos.y += (int)Math.Floor((double)tilesData.GetLength(0)/2);

        Vector3Int relativePlayerPos = levelTiledMap.layoutGrid.WorldToCell(
                                                                        new Vector3 (
                                                                            joueur.transform.position.x, 
                                                                            joueur.transform.position.y + hauteurJoueur/2
                                                                            )
        );
        relativeAIView = new Vector3Int  (
                                                    relativePlayerPos.x - (int)Math.Floor((double)resolutionLongueur/2) + 1,
                                                    relativePlayerPos.y + resolutionHauteur + camDecalageY
                                                    );

        Vector3Int deltaPos = new Vector3Int(
                                                relativeTileMapPos.x - relativeAIView.x,
                                                relativeTileMapPos.y - relativeAIView.y
                                            );

        int i = deltaPos.y >= 0 ? 0 : -deltaPos.y;

        for(; i < resolutionHauteur; i++){
            int j = deltaPos.x < 0 ? 0 : deltaPos.x;
            for(; j < resolutionLongueur; j++){
                int valueToCopy = 0;
                if(i + deltaPos.y < tilesData.GetLength(0) && j - deltaPos.x < tilesData.GetLength(1) &&
                    i + deltaPos.y >= 0 && j - deltaPos.x >= 0
                ) {
                    valueToCopy = tilesData[i + deltaPos.y, j - deltaPos.x];
                }
                if(copiedMatrix.GetLength(0) - i < copiedMatrix.GetLength(0) && copiedMatrix.GetLength(0) - i >= 0 
                && i >= 0 && i < copiedMatrix.GetLength(0)){
                    copiedMatrix[i, j] = valueToCopy;
                }
            } 
        }
        return copiedMatrix;
    }

    int[,] readTilesObject(GameObject[] tilesObject, Tilemap map, int[,] tileData){
        foreach(GameObject gObj in tilesObject){

            Vector3Int relPos = map.layoutGrid.WorldToCell(gObj.transform.position); //Pour avoir la position relatives des objects
            Vector3 cellSize = map.layoutGrid.cellSize; //Pour avoir la taille relative des objects
            
            Vector3Int relSize = new Vector3Int(
                (int)Math.Ceiling(gObj.GetComponent<BoxCollider2D>().bounds.size.x / cellSize.x),
                (int)Math.Ceiling(gObj.GetComponent<BoxCollider2D>().bounds.size.y / cellSize.y)
            );

            relPos.x -= (int)((double)relSize.x/2);
            relPos.y += (int)Math.Ceiling((double)relSize.y/2) + 4;


            Vector3Int deltaPos = new Vector3Int(
                                                relPos.x - relativeAIView.x,
                                                relPos.y - relativeAIView.y
                                            );

            int i = deltaPos.y >= 0 ? 0 : -deltaPos.y;
            int yMax = i + relSize.y;

            for(; i < yMax; i++){
                int j = deltaPos.x < 0 ? 0 : deltaPos.x;
                int xMax = j + relSize.x;
                for(; j < xMax; j++){
                    if(i < tileData.GetLength(0) && i >= 0 && j < tileData.GetLength(1)){
                        int value = -1;
                        if(gObj.ToString().IndexOf("Arrivée(Clone)") > -1) value = 2;
                        tileData[i,j] = value;
                    }
                }  
            }
        }
    return tileData;
    }

    //*==================={Draw Method}===================

    
    void OnGUI()
    {   /*
        if(!montrerAI) return;
        //Rect rectScreen = cam.pixelRect;

        for(int i = 0; i < aiView.GetLength(0); i++){
            EditorGUI.TextField(new Rect(0, i*tailleAffichageCellule, tailleAffichageCellule, tailleAffichageCellule), i.ToString());
            for(int j = 0; j < aiView.GetLength(1); j++){
                Color color = new Color(0, 0, 0, 0.5f);
                if(j==aiView.GetLength(1)/2 && i == aiView.GetLength(0) - 2) color = new Color(0, 0, 255, 1.0f);
                else if(aiView[i, j] == 2) color = new Color(255, 223, 0, 0.7f);
                else if(aiView[i, j] > 0) color = new Color(255, 255, 255, 0.5f);
                else if (aiView[i, j] < 0) color = new Color(255, 0, 0, 0.5f);
                
                EditorGUI.DrawRect(new Rect(j * tailleAffichageCellule, i * tailleAffichageCellule, tailleAffichageCellule, tailleAffichageCellule), color);
            }
        }
        for (int i = 0; i < aiView.GetLength(1); i++){
            EditorGUI.TextField(new Rect(i*tailleAffichageCellule, 0, tailleAffichageCellule, tailleAffichageCellule), i.ToString());
        }
        for (int i = 0; i < aiView.GetLength(0) + 1; i++){
            EditorGUI.DrawRect(new Rect(0, i * tailleAffichageCellule, aiView.GetLength(1) * tailleAffichageCellule, 1), Color.black);
        }
         for(int j = 0; j < aiView.GetLength(1) + 1; j++){
            EditorGUI.DrawRect(new Rect(j * tailleAffichageCellule, 0, 1, aiView.GetLength(0) * tailleAffichageCellule), Color.black);
        }*/
        
    }
    //*==================={GETTER}===================
    GameObject[] getEveryTileObject(Tilemap map){
        GameObject[] tilesObject = new GameObject[map.transform.childCount];

        for (int i = 0; i < map.transform.childCount; ++i)
            tilesObject[i] = map.transform.GetChild(i).gameObject;

        return tilesObject;
    }
    public bool getMontrerAI(){return montrerAI;}

    public Vector3 getPositionJoueur(){
        return joueur.transform.position;
    }

    public Vector3 getPositionArrive(){
        GameObject[] tileObject = getEveryTileObject(levelTiledMap);

        foreach (GameObject tile in tileObject){
            if (tile.ToString().IndexOf("Arrivée(Clone)") > -1 ) return tile.transform.position;
        }

        return default;
    }
    public bool getJoueurMort(){
        return !joueur.activeInHierarchy;
    }
    public void activerJoueur(){
        joueur.SetActive(true);
    }
    public void desactiverJoueur(){
        joueur.SetActive(false);
    }
    public void setPosJoueur(Vector3 pos){
        joueur.transform.position = pos;
    }
    public Vector2Int getTailleVue(){
        return new Vector2Int(resolutionLongueur, resolutionHauteur);
    }
    public GameObject getJoueur(){
        return joueur;
    }
    public int[,] getVue(){
        return aiView;
    }
    public bool getIAActivee(){return montrerAI;}
}



