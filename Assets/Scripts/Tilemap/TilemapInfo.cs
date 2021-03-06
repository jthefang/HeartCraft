﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class TilemapInfo : MonoBehaviour, ILoadableScript
{
    #region Singleton
    public static TilemapInfo Instance;
    void Awake() {
        Instance = this;
    }
    #endregion 

    #region ILoadableScript
    public event Action<ILoadableScript> OnScriptInitialized;
    bool _isInitialized = false;
    bool isInitialized {
        get {
            return this._isInitialized;
        }
        set {
            this._isInitialized = value;
            if (this._isInitialized) {
                OnScriptInitialized?.Invoke(this);
            }
        }   
    }
    public bool IsInitialized () {
        return isInitialized;
    }
    #endregion

    Tilemap tilemap;
    Tiles tiles;
    Grid grid;
    TileSpriteInteraction[,] tileSpriteInteractions;
    Dictionary<Sprite, TileSpriteInteraction> spriteToTileInteraction;
    [SerializeField]
    Vector2 mapDimensions;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetTilemap();
        grid = GetGrid();
        tiles = Tiles.Instance;

        InitializeTilemap();
        spriteToTileInteraction = new Dictionary<Sprite, TileSpriteInteraction>();
        isInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeTilemap() {
        Vector3Int bottomCorner = GetBottomCorner();
        Vector3Int topCorner = GetTopCorner();

        int numRows = topCorner.x - bottomCorner.x + 1;
        int numCols = topCorner.y - bottomCorner.y + 1;
        tileSpriteInteractions = new TileSpriteInteraction[numRows, numCols];
        for (int r = bottomCorner.x; r <= topCorner.x; r++) {
            for (int c = bottomCorner.y; c <= topCorner.y; c++) {
                Vector3Int tilePosition = new Vector3Int(r, c, 0);
                tilemap.SetTile(tilePosition, tiles.BaseTile);

                Vector2Int tileSpriteIdx = TileSpriteIdxFromTilePosition(tilePosition);
                tileSpriteInteractions[tileSpriteIdx.x, tileSpriteIdx.y] = new TileSpriteInteraction(tilePosition);
            }
        }
    }

    #region Basic info
    public Grid GetGrid() {
        return GetTilemap().layoutGrid;
    }

    public Tilemap GetTilemap() {
        return GetComponent<Tilemap>();
    }

    public float GetSpriteOffsetAboveTile() {
        return 3 * tiles.TileBlockHeightOffset;
    }

    public Vector3Int GetBottomCorner() {
        int maxX = (int) mapDimensions.x / 2;
        int maxY = (int) mapDimensions.y / 2;
        return new Vector3Int(-maxX, -maxY, 0);
    }

    public Vector3Int GetCenter(){
        return new Vector3Int(0, 0, 0);
    }

    public Vector3Int GetTopCorner() {
        int maxX = (int) mapDimensions.x / 2;
        int maxY = (int) mapDimensions.y / 2;
        bool isOdd = mapDimensions.x % 2 == 1;
        if (isOdd) {
            return new Vector3Int(maxX, maxY, 0);
        }
        return new Vector3Int(maxX-1, maxY-1, 0);
    }
    
    public bool ExistsTileAt(Vector3Int tilePosition) {
        return tilemap.GetTile(tilePosition) != null;
    }

    public bool ExistsBaseTileAt(Vector3Int tilePosition) {
        TileBase tileAtPosition = tilemap.GetTile(tilePosition);
        return tileAtPosition != null && tileAtPosition.Equals(tiles.BaseTile);
    }
    
    public Vector3Int GetTilePositionUnderMouse() {
        Vector3 mouseCoords = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        mouseCoords.y -= tiles.TileBlockHeightOffset;
        Vector3Int tilePositionUnderMouse = grid.LocalToCell(mouseCoords); 
        tilePositionUnderMouse.z = 0;
        return tilePositionUnderMouse;
    }
    #endregion

    void SpriteDoesNotExist(Sprite sprite) {
        Debug.LogError("This sprite does not exist on the tilemap.");
    }

    void SpriteAlreadyExists(Sprite sprite) {
        Debug.LogError("This sprite already exists.");
    }

    #region TileSpriteInteractions
    Vector2Int TileSpriteIdxFromTilePosition(Vector3Int tilePosition) {
        Vector3Int bottomCorner = GetBottomCorner();
        int x = tilePosition.x - bottomCorner.x;
        int y = tilePosition.y - bottomCorner.y;
        return new Vector2Int(x, y);
    }

    public void AddNewSpriteAtTilePosition(Sprite newSprite, Vector3Int tilePosition) {
        if (spriteToTileInteraction.ContainsKey(newSprite)) {
            SpriteAlreadyExists(newSprite);
            return;
        }

        TileSpriteInteraction newTile = GetTileSpriteInteractionAtTilePosition(tilePosition);
        spriteToTileInteraction[newSprite] = newTile;
        newTile.AddSprite(newSprite);
    }

    public void UpdateTilePositionOfSprite(Sprite sprite, Vector3Int tilePosition) {
        if (!spriteToTileInteraction.ContainsKey(sprite)) { 
            SpriteDoesNotExist(sprite);
            return;
        }

        TileSpriteInteraction prevTile = spriteToTileInteraction[sprite];
        TileSpriteInteraction newTile = GetTileSpriteInteractionAtTilePosition(tilePosition);
        if (prevTile.Equals(newTile)) {
            return;
        }
        prevTile.RemoveSprite(sprite);
        newTile.AddSprite(sprite);
        spriteToTileInteraction[sprite] = newTile;
    }

    TileSpriteInteraction GetTileSpriteInteractionAtTilePosition(Vector3Int tilePosition) {
        Vector2Int tileSpriteIdx = TileSpriteIdxFromTilePosition(tilePosition);
        return tileSpriteInteractions[tileSpriteIdx.x, tileSpriteIdx.y];
    }

    public Vector3Int GetTilePositionOfSprite(Sprite sprite) {
        if (!spriteToTileInteraction.ContainsKey(sprite)) {
            SpriteDoesNotExist(sprite);
            return new Vector3Int(999, 999, 999);
        } 

        return spriteToTileInteraction[sprite].GetTilePosition();
    }

    public List<Sprite> GetSpritesAtTilePosition(Vector3Int tilePosition) {
        TileSpriteInteraction tile = GetTileSpriteInteractionAtTilePosition(tilePosition);
        return tile.GetSprites();
    }
    #endregion
}
