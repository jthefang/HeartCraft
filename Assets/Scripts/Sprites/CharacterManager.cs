using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    GameObject characterPrefab;

    GameManager gameManager;
    TilemapInfo tilemapInfo;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnGameStart += OnGameStart;

        tilemapInfo = TilemapInfo.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGameStart(GameManager gm) {
        SpawnCharacters();
    }

    void SpawnCharacters() {
        Vector3Int position = tilemapInfo.GetTopCorner();
        GameObject characterObj = Instantiate(characterPrefab, position, Quaternion.identity);
        characterObj.transform.SetParent(this.transform);
        Character character = characterObj.GetComponent<Character>();
        character.OnInitializedSpawnAtTilePosition(position);
    }

}
