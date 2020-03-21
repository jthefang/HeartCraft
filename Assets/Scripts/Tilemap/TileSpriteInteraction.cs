using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpriteInteraction
{
    Vector3Int tilePosition;
    List<Sprite> sprites;

    public TileSpriteInteraction(Vector3Int tilePosition) {
        this.tilePosition = tilePosition;
        this.sprites = new List<Sprite>();
    }

    public TileSpriteInteraction(Vector3Int tilePosition, List<Sprite> sprites) {
        this.tilePosition = tilePosition;
        this.sprites = sprites;
    }

    public void RemoveSprite(Sprite sprite) {
        sprites.Remove(sprite);
    }

    public void AddSprite(Sprite sprite) {
        sprites.Add(sprite);
    }

    public Vector3Int GetTilePosition() {
        return tilePosition;
    }

    public List<Sprite> GetSprites() {
        return sprites;
    }

}
