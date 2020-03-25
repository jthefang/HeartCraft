using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerLocation {
    NW, 
    NE, 
    CENTER, 
    SW,
    SE
}

public class TowerManager : MonoBehaviour
{
    [SerializeField]
    GameObject towerPrefab;
    GameManager gameManager;
    TilemapInfo tilemapInfo;

    Dictionary<TowerLocation, Vector3Int> towerPositions;
    Dictionary<TowerLocation, TowerSprite> towers;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnGameStart += OnGameStart;
        tilemapInfo = TilemapInfo.Instance;
        InitializeTowerPositions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGameStart(GameManager gm) {
        SpawnTowers();
    }

    void InitializeTowerPositions() {
        Vector3Int center = tilemapInfo.GetCenter();
        Vector3Int bottomCorner = tilemapInfo.GetBottomCorner();
        Vector3Int topCorner = tilemapInfo.GetTopCorner();
        towerPositions = new Dictionary<TowerLocation, Vector3Int> {
            {TowerLocation.NW, new Vector3Int(center.x, topCorner.y-1, 0)}, //NW
            {TowerLocation.NE, new Vector3Int(topCorner.x-1, center.y, 0)}, //NE
            {TowerLocation.CENTER, center},
            {TowerLocation.SW, new Vector3Int(bottomCorner.x+1, center.y, 0)}, //SW
            {TowerLocation.SE, new Vector3Int(center.x, bottomCorner.y+1, 0)} //SE
        };
    }

    void SpawnTowers() {
        towers = new Dictionary<TowerLocation, TowerSprite>();
        foreach(KeyValuePair<TowerLocation, Vector3Int> towerPos in towerPositions) {
            TowerLocation towerLoc = towerPos.Key;
            Vector3Int position = towerPos.Value;

            GameObject tower = Instantiate(towerPrefab, position, Quaternion.identity);
            tower.transform.SetParent(this.transform);
            TowerSprite towerSprite = tower.GetComponent<TowerSprite>();
            towerSprite.OnInitializedSpawnAtTilePosition(position);
            towers[towerLoc] = towerSprite; 
        }
    }

}
