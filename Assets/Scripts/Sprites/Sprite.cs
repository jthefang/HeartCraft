using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite : MonoBehaviour
{
    TilemapInfo tilemapInfo;
    SpriteMovement spriteMovement;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void Init() {
        tilemapInfo = TilemapInfo.Instance;
        spriteMovement = this.GetComponent<SpriteMovement>();
    }

    public void SpawnSpriteAtTopCorner() {
        SpawnSpriteAtTilePosition(tilemapInfo.GetTopCorner());
    }

    public void SpawnSpriteAtTilePosition(Vector3Int tilePosition) {
        tilemapInfo.AddNewSpriteAtTilePosition(this, tilePosition);
        spriteMovement.MoveToTilePosition(tilePosition);
    }
}
