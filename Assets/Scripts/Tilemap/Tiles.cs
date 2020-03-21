using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tiles : MonoBehaviour
{
    #region Singleton
    public static Tiles Instance;
    void Awake() {
        Instance = this;
    }   
    #endregion

    [SerializeField]
    float _tileBlockHeightOffset;
    public float TileBlockHeightOffset {
        get {
            return _tileBlockHeightOffset;
        }
    }

    #region Tile Types
    [SerializeField]
    Tile _hoverTile;
    public Tile HoverTile {
        get {
            return _hoverTile;
        }
    }
    [SerializeField]
    Tile _hoverSWTile;
    public Tile HoverSWTile {
        get {
            return _hoverSWTile;
        }
    }
    [SerializeField]
    Tile _hoverSETile;
    public Tile HoverSETile {
        get {
            return _hoverSETile;
        }
    }
    [SerializeField]
    Tile _baseTile;
    public Tile BaseTile {
        get {
            return _baseTile;
        }
    }
    #endregion
}
