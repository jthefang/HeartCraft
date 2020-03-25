using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveOnClickOption : MonoBehaviour
{
    SelectSpriteOnClick selectSprite;
    TilemapInfo tilemapInfo;

    bool isCharacterSelected;
    Character selectedCharacter;

    // Start is called before the first frame update
    void Start()
    {
        selectSprite = SelectSpriteOnClick.Instance;
        selectSprite.OnNewSpriteSelected += OnNewSpriteSelected;
        selectSprite.OnSpriteDeselected += OnSpriteDeselected;

        tilemapInfo = TilemapInfo.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCharacterSelected && Input.GetMouseButtonDown(0)) {
            MoveSelectedCharacterToTilePositionUnderMouse();
        }
    }

    void MoveSelectedCharacterToTilePositionUnderMouse() {
        Vector3Int tilePositionUnderMouse = tilemapInfo.GetTilePositionUnderMouse();
        if (!tilemapInfo.ExistsTileAt(tilePositionUnderMouse)) {
            return;
        }

        bool hasSpritesAtTilePosition = tilemapInfo.GetSpritesAtTilePosition(tilePositionUnderMouse).Count > 0;
        if (hasSpritesAtTilePosition) {
            return;
        }
        selectedCharacter.GetComponent<SpriteMovement>().MoveToTilePosition(tilePositionUnderMouse);
    }

    void OnNewSpriteSelected(Sprite prevSprite, Sprite newSprite) {
        if (newSprite.IsCharacter()) {
            isCharacterSelected = true;
            selectedCharacter = newSprite.GetComponent<Character>();
        } else {
            DeselectCharacter();
        }
    }

    void OnSpriteDeselected(Sprite sprite) {
        DeselectCharacter();
    }

    void DeselectCharacter() {
        isCharacterSelected = false;
    }

}
