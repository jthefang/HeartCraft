using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class SpriteMovement : MonoBehaviour, ILoadableScript
{
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

    TilemapInfo tilemapInfo;
    Grid grid;
    Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        this.tilemapInfo = TilemapInfo.Instance;
        this.grid = tilemapInfo.GetGrid();
        this.sprite = this.GetComponent<Sprite>();
        isInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToTopCorner() {
        MoveToTilePosition(tilemapInfo.GetTopCorner());
    }

    public void MoveToTilePositionUnderMouse() {
        Vector3Int tilePositionUnderMouse = tilemapInfo.GetTilePositionUnderMouse();
        if (!tilemapInfo.ExistsTileAt(tilePositionUnderMouse)) {
            return;
        }

        bool hasSpritesAtTilePosition = tilemapInfo.GetSpritesAtTilePosition(tilePositionUnderMouse).Count > 0;
        if (hasSpritesAtTilePosition) {
            return;
        }
        MoveToTilePosition(tilePositionUnderMouse);
    }

    public void MoveToTilePosition(Vector3Int tilePosition) {
        Vector3 newPosition = grid.CellToWorld(tilePosition);
        newPosition.y += tilemapInfo.GetSpriteOffsetAboveTile();
        this.transform.position = newPosition;
        this.tilemapInfo.UpdateTilePositionOfSprite(this.sprite, tilePosition);
    }
}
