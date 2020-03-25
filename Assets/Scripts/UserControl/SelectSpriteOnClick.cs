using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectSpriteOnClick : MonoBehaviour
{
    #region Singleton
    public static SelectSpriteOnClick Instance;
    void Awake() {
        Instance = this;
    }
    #endregion

    TilemapInfo tilemapInfo;

    Sprite _currSelectedSprite;
    Sprite CurrSelectedSprite {
        get {
            return this._currSelectedSprite;
        }
        set {
            Sprite prevSprite = this._currSelectedSprite;
            this._currSelectedSprite = value;
            if (this._currSelectedSprite == null) {
                if (prevSprite != null)
                    OnSpriteDeselected?.Invoke(prevSprite);
                return;
            }

            if (prevSprite == null || !prevSprite.Equals(this._currSelectedSprite)) {
                OnNewSpriteSelected?.Invoke(prevSprite, this._currSelectedSprite);
            }
        }
    }
    public event Action<Sprite, Sprite> OnNewSpriteSelected;
    public event Action<Sprite> OnSpriteDeselected;

    // Start is called before the first frame update
    void Start()
    {
        tilemapInfo = TilemapInfo.Instance;
        OnNewSpriteSelected += OnNewSpriteSelectedHandler;
        OnSpriteDeselected += OnSpriteDeselectedHandler;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            List<Sprite> spritesUnderMouse = GetSpritesOnTileUnderMouse();
            SelectSprites(spritesUnderMouse);
        }
    }

    void OnSpriteDeselectedHandler(Sprite prevSprite) {
        SpriteInfoPanel.Instance.ResetDisplay();
    }

    void OnNewSpriteSelectedHandler(Sprite prevSprite, Sprite newSprite) {
        SpriteInfoPanel.Instance.DisplaySpriteInfo(newSprite);
    }

    public Sprite GetCurrSelectedSprite() {
        return CurrSelectedSprite;
    }

    public bool CurrSelectedSpriteIsCharacter() {
        if (CurrSelectedSprite == null) {
            return false;
        }
        return CurrSelectedSprite.IsCharacter();
    }

    public Character GetCurrSelectedCharacter() {
        return CurrSelectedSprite.GetComponent<Character>();
    }

    void SelectSprites(List<Sprite> sprites) {
        /**
            Right now we assume there can only be one sprite at each tile
                => this list should only have one sprite
                => we only "select" one sprite 
        */
        if (sprites == null || sprites.Count <= 0) {
            CurrSelectedSprite = null;
            return;
        }

        CurrSelectedSprite = sprites[0];
    }

    List<Sprite> GetSpritesOnTileUnderMouse() {
        Vector3Int tilePositionUnderMouse = tilemapInfo.GetTilePositionUnderMouse();
        if (!tilemapInfo.ExistsTileAt(tilePositionUnderMouse)) {
            return null;
        }
        return tilemapInfo.GetSpritesAtTilePosition(tilePositionUnderMouse);
    }

}
