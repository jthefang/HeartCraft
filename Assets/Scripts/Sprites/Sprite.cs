using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sprite : MonoBehaviour, IDependentScript
{
    protected TilemapInfo tilemapInfo;
    protected SpriteMovement spriteMovement;

    bool hasSpawnPosition;
    bool isSelected;
    Vector3Int spawnTilePosition;

    // Start is called before the first frame update
    void Start()
    {
        List<ILoadableScript> dependencies = new List<ILoadableScript>();
        dependencies.Add(this.GetComponent<SpriteMovement>());
        ScriptDependencyManager.Instance.UpdateDependencyDicts(this, dependencies);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Initialization
    public void OnAllDependenciesLoaded() {
        Init();
        OnSpriteInitialized();
    }

    void OnSpriteInitialized() {
        if (hasSpawnPosition)
            SpawnSpriteAtTilePosition(spawnTilePosition);
    }

    public void OnInitializedSpawnAtTilePosition(Vector3Int tilePosition) {
        hasSpawnPosition = true;
        spawnTilePosition = tilePosition;
    }

    protected virtual void Init() {
        tilemapInfo = TilemapInfo.Instance;
        spriteMovement = this.GetComponent<SpriteMovement>();
    }

    public void SpawnSpriteAtTilePosition(Vector3Int tilePosition) {
        tilemapInfo.AddNewSpriteAtTilePosition(this, tilePosition);
        spriteMovement.MoveToTilePosition(tilePosition);
    }
    #endregion

    public virtual bool IsCharacter() {
        return false;
    }

    public bool IsSelected() {
        return isSelected;
    }

    public virtual void OnSelected() {
        isSelected = true;
    }

    public virtual void OnDeselected() {
        isSelected = false;
    }

}
