using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AIScripts : MonoBehaviour{

    //*==================={PUBLIC}===================
    public GameObject joueur;
    public int resolutionHauteur;
    public int resolutionLongueur;
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
                m[i, j] = valeur;
            }
        }
        return m;
    }

    int[,] copyTilesDataToView(){
        int[,] copiedMatrix = new int[resolutionHauteur, resolutionLongueur];

        float longueurJoueur = joueur.GetComponent<BoxCollider2D>().bounds.size.x;
        float hauteurJoueur = joueur.GetComponent<BoxCollider2D>().bounds.size.y;

        Vector3Int relativeTileMapPos = levelTiledMap.layoutGrid.WorldToCell(levelTiledMap.transform.position);
        relativeTileMapPos.x = relativeTileMapPos.x - tilesData.GetLength(1)/2;
        relativeTileMapPos.y = relativeTileMapPos.y - tilesData.GetLength(0)/2;

        Vector3Int relativePlayerPos = levelTiledMap.layoutGrid.WorldToCell(
                                                                            new Vector3(
                                                                                        joueur.transform.position.x,
                                                                                        joueur.transform.position.y + hauteurJoueur/2
                                                                            ));
        relativeAIView = new Vector3Int  (
                                                    relativePlayerPos.x - resolutionLongueur/2,
                                                    relativePlayerPos.y + 1
                                                    );

        Vector3Int deltaPos = new Vector3Int(
                                                relativeTileMapPos.x - relativeAIView.x,
                                                relativeTileMapPos.y - relativeAIView.y
                                            );

        int i = deltaPos.y < 0 ? 0 : deltaPos.y;

        for(; i < resolutionHauteur; i++){
            int j = deltaPos.x < 0 ? 0 : deltaPos.x;
            for(; j < resolutionLongueur; j++){
                int valueToCopy = 0;
                if( i - deltaPos.y < tilesData.GetLength(0) && j - deltaPos.x < tilesData.GetLength(1) &&
                    i - deltaPos.y >= 0 && j - deltaPos.x >= 0
                ) {
                    valueToCopy = tilesData[i - deltaPos.y, j - deltaPos.x];
                }
                if(copiedMatrix.GetLength(0) - i < copiedMatrix.GetLength(0) && copiedMatrix.GetLength(0) - i >= 0){
                    copiedMatrix[copiedMatrix.GetLength(0) - i, j] = valueToCopy;
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
                (int)Math.Floor(gObj.GetComponent<BoxCollider2D>().bounds.size.x / cellSize.x),
                (int)Math.Floor(gObj.GetComponent<BoxCollider2D>().bounds.size.y / cellSize.y)
            );

            relPos.x -= relSize.x/2;
            relPos.y -= relSize.y/2;


            Vector3Int deltaPos = new Vector3Int(
                                                relPos.x - relativeAIView.x,
                                                relPos.y - relativeAIView.y
                                            );

            int i = deltaPos.y < 0 ? 0 : deltaPos.y;

            for(; i < relSize.y + deltaPos.y; i++){
                int j = deltaPos.x < 0 ? 0 : deltaPos.x;
                for(; j < relSize.x + deltaPos.x; j++){
                    if(tileData.GetLength(0) - i < tileData.GetLength(0) && tileData.GetLength(0) - i >= 0 && j < tileData.GetLength(1)){
                        tileData[tileData.GetLength(0) - i,j] = -1;
                    }
                }  
            }
        }
    return tileData;
    }

    //*==================={Draw Method}===================

    
    void OnGUI()
    {
        if(!montrerAI) return;
        //Rect rectScreen = cam.pixelRect;
        float squareSize = 20;

        for(int i = 0; i < aiView.GetLength(0); i++){
            EditorGUI.TextField(new Rect(0, i*squareSize, squareSize, squareSize), i.ToString());
            for(int j = 0; j < aiView.GetLength(1); j++){
                Color color = new Color(0, 0, 0, 0.5f);
                if(j==aiView.GetLength(1)/2 && i == aiView.GetLength(0) - 2) color = new Color(0, 0, 255, 1.0f);
                else if(aiView[i, j] > 0) color = new Color(255, 255, 255, 0.5f);
                else if (aiView[i, j] < 0) color = new Color(255, 0, 0, 0.5f);
                
                EditorGUI.DrawRect(new Rect(j * squareSize, i * squareSize, squareSize, squareSize), color);
            }
        }
        for (int i = 0; i < aiView.GetLength(1); i++){
            EditorGUI.TextField(new Rect(i*squareSize, 0, squareSize, squareSize), i.ToString());
        }
        for (int i = 0; i < aiView.GetLength(0) + 1; i++){
            EditorGUI.DrawRect(new Rect(0, i * squareSize, aiView.GetLength(1) * squareSize, 1), Color.black);
        }
         for(int j = 0; j < aiView.GetLength(1) + 1; j++){
            EditorGUI.DrawRect(new Rect(j * squareSize, 0, 1, aiView.GetLength(0) * squareSize), Color.black);
        }
        
    }
    //*==================={GETTER}===================
    GameObject[] getEveryTileObject(Tilemap map){
        GameObject[] tilesObject = new GameObject[map.transform.childCount];

        for (int i = 0; i < map.transform.childCount; ++i)
            tilesObject[i] = map.transform.GetChild(i).gameObject;

        return tilesObject;
    }
}


