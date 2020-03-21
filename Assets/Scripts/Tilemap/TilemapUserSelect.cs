using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

enum Direction {
    SE,
    SW
}

class PrevHighlightedTile {
    public bool hasActiveHighlight;
    public Vector3Int position;

    public PrevHighlightedTile() {
        UpdateBoolAndPosition(false, new Vector3Int(0, 0, 0));
    }

    public PrevHighlightedTile(bool highlighted, Vector3Int position) {
        UpdateBoolAndPosition(highlighted, position);
    }

    public void UpdateBoolAndPosition(bool highlighted, Vector3Int position) {
        this.hasActiveHighlight = highlighted;
        this.position = position;
    }

    public bool MatchesPosition(Vector3Int otherPosition) {
        return this.position.Equals(otherPosition);
    }
}

public class TilemapUserSelect : MonoBehaviour
{
    Grid grid;
    Tilemap tilemap;
    TilemapInfo tilemapInfo;
    Tiles tiles;

    PrevHighlightedTile prevHighlightedTile;
    PrevHighlightedTile[] highlightedNeighbors;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        tilemapInfo = TilemapInfo.Instance;
        tiles = Tiles.Instance;
        grid = tilemap.layoutGrid;

        prevHighlightedTile = new PrevHighlightedTile();
        int numDirections = GetDirections().Length;
        highlightedNeighbors = new PrevHighlightedTile[numDirections];
        for (int i = 0; i < numDirections; i++) {
            highlightedNeighbors[i] = new PrevHighlightedTile();
        }
    }

    // Update is called once per frame
    void Update()
    {
        HighlightTileUnderMouse();
        if (Input.GetMouseButtonDown(0)) {
            DisplayInfoForTileUnderMouse();
        }
    }

    Direction[] GetDirections() {
        return (Direction[]) Enum.GetValues(typeof(Direction));;
    }

    bool ExistsTileAt(Vector3Int tilePosition) {
        return tilemap.GetTile(tilePosition) != null;
    }

    Vector3Int GetTilePositionUnderMouse() {
        Vector3 mouseCoords = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        mouseCoords.y -= tiles.TileBlockHeightOffset;
        Vector3Int tilePositionUnderMouse = grid.LocalToCell(mouseCoords); 
        tilePositionUnderMouse.z = 0;
        return tilePositionUnderMouse;
    }

    void DisplayInfoForTileUnderMouse() {
        Vector3Int tilePositionUnderMouse = GetTilePositionUnderMouse();
        List<Sprite> spritesAtTilePosition = tilemapInfo.GetSpritesAtTilePosition(tilePositionUnderMouse);
        foreach (Sprite s in spritesAtTilePosition) {
            Debug.Log(s);
        }
    }

    #region HighlightOnHover
    void HighlightTileUnderMouse() {
        Vector3Int hoverTilePos = GetTilePositionUnderMouse();

        if (prevHighlightedTile.hasActiveHighlight && !prevHighlightedTile.MatchesPosition(hoverTilePos)) {
            DehighlightPrevTile();
        }

        if (ExistsUnhighlightedTileAt(hoverTilePos)) {
            HighlightTile(hoverTilePos);
        }
    }

    void HighlightTile(Vector3Int tilePosition) {
        tilemap.SetTile(tilePosition, tiles.HoverTile);
        prevHighlightedTile.UpdateBoolAndPosition(true, tilePosition);
        
        HighlightNeighbors(tilePosition); //need this because of tile overlap
    }

    void HighlightNeighbors(Vector3Int position) {
        Direction[] directions = GetDirections();
        for (int i = 0; i < directions.Length; i++) {
            Direction dir = directions[i];
            Vector3Int neighborPos = GetNeighborPositionInDirection(position, dir);
            if (ExistsUnhighlightedTileAt(neighborPos)) {
                TileBase highlightTile = tiles.HoverSETile;
                if (dir == Direction.SW) {
                    highlightTile = tiles.HoverSWTile;
                }

                tilemap.SetTile(neighborPos, highlightTile);
                highlightedNeighbors[i].UpdateBoolAndPosition(true, neighborPos);
            }
        }
    }

    Vector3Int GetNeighborPositionInDirection(Vector3Int position, Direction direction) {
        Vector3Int neighborPos = new Vector3Int(position.x, position.y, position.z);
        switch (direction) {
            case Direction.SW:
                neighborPos.x -= 1;
                return neighborPos;
            case Direction.SE:
                neighborPos.y -= 1;
                return neighborPos;
            default:
                return neighborPos;
        }
    }

    bool ExistsUnhighlightedTileAt(Vector3Int tilePosition) {
        TileBase tileAtPosition = tilemap.GetTile(tilePosition);
        return tileAtPosition != null && tileAtPosition.Equals(tiles.BaseTile);
    }

    void DehighlightPrevTile() {
        tilemap.SetTile(prevHighlightedTile.position, tiles.BaseTile);
        prevHighlightedTile.hasActiveHighlight = false;

        DehighlightNeighbors();
    }

    void DehighlightNeighbors() {
        for (int i = 0; i < highlightedNeighbors.Length; i++) {
            PrevHighlightedTile neighbor = highlightedNeighbors[i];
            if (neighbor.hasActiveHighlight) {
                tilemap.SetTile(neighbor.position, tiles.BaseTile);
                neighbor.hasActiveHighlight = false;
            }
        }
    }
    #endregion

}
