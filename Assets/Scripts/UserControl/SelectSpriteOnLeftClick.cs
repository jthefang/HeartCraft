using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectSpriteOnLeftClick : MonoBehaviour
{
    #region Singleton
    public static SelectSpriteOnLeftClick Instance;
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

    bool shouldIgnoreLeftClick;
    int numClicksToIgnore;

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
        if (numClicksToIgnore <= 0) {
            shouldIgnoreLeftClick = false;
        }

        if (Input.GetMouseButtonDown(0)) {
            if (shouldIgnoreLeftClick) {
                numClicksToIgnore -= 1;
            } else {
                List<Sprite> spritesUnderMouse = GetSpritesOnTileUnderMouse();
                SelectASpriteFrom(spritesUnderMouse);
            }
        }
    }

    void OnSpriteDeselectedHandler(Sprite prevSprite) {
        SpriteInfoPanel.Instance.ResetDisplay();
        prevSprite.OnDeselected();
    }

    void OnNewSpriteSelectedHandler(Sprite prevSprite, Sprite newSprite) {
        SpriteInfoPanel.Instance.DisplaySpriteInfo(newSprite);
        if (prevSprite != null)
            prevSprite.OnDeselected();
        newSprite.OnSelected();
    }

    public void IgnoreLeftClickForNClicks(int n) {
        shouldIgnoreLeftClick = true;
        numClicksToIgnore = n;
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

    void SelectASpriteFrom(List<Sprite> sprites) {
        /**
            Right now we assume there can only be one sprite at each tile
                => this list should only have one sprite
                => we only "select" one sprite 
        */
        if (sprites == null || sprites.Count <= 0) {
            return;
        }

        SelectSprite(sprites[0]);
    }

    void SelectSprite(Sprite sprite) {
        CurrSelectedSprite = sprite;
    }

    void DeselectSprite() {
        CurrSelectedSprite = null;
    }

    List<Sprite> GetSpritesOnTileUnderMouse() {
        Vector3Int tilePositionUnderMouse = tilemapInfo.GetTilePositionUnderMouse();
        if (!tilemapInfo.ExistsTileAt(tilePositionUnderMouse)) {
            return null;
        }
        return tilemapInfo.GetSpritesAtTilePosition(tilePositionUnderMouse);
    }

}
