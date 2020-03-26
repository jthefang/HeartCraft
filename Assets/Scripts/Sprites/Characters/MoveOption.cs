using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOption : MonoBehaviour, ICharacterControlOption
{
    [SerializeField]
    float speedInTilesPerSecond;

    bool _isActive;
    public bool IsActive {
        get;
    }

    Character character;
    SpriteMovement spriteMovement;

    // Start is called before the first frame update
    void Start()
    {
        character = this.GetComponent<Character>();
        spriteMovement = this.GetComponent<SpriteMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (character.IsSelected()) {
            if (OptionTurnsActive()) {
                SelectSpriteOnLeftClick.Instance.IgnoreLeftClickForNClicks(1);
                _isActive = true;
            }
            bool rightClick = Input.GetMouseButtonDown(1);
            bool shouldMove = (_isActive && Input.GetMouseButtonDown(0)) || rightClick;
            if (shouldMove) {
                MoveCharacterToTilePositionUnderMouse();
                _isActive = false;
            }
        }
    }

    public void DisplayUI() {
        
    }

    public bool OptionTurnsActive() {
        return Input.GetKeyDown(KeyCode.M);
    }

    void MoveCharacterToTilePositionUnderMouse() {
        spriteMovement.MoveToTilePositionUnderMouse();
    }

}
