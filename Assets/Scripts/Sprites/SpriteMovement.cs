using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpriteMovement : MonoBehaviour
{
    TilemapInfo tilemapInfo;
    Grid grid;
    Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        this.tilemapInfo = TilemapInfo.Instance;
        this.grid = tilemapInfo.GetGrid();
        this.sprite = this.GetComponent<Sprite>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToTopCorner() {
        MoveToTilePosition(tilemapInfo.GetTopCorner());
    }

    public void MoveToTilePosition(Vector3Int tilePosition) {
        Vector3 newPosition = grid.CellToWorld(tilePosition);
        newPosition.y += tilemapInfo.GetSpriteOffsetAboveTile();
        this.transform.position = newPosition;
        this.tilemapInfo.UpdateTilePositionOfSprite(this.sprite, tilePosition);
    }
}
