using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteInfoPanel : MonoBehaviour
{
    #region Singleton
    public static SpriteInfoPanel Instance;
    void Awake() {
        Instance = this;
    }
    #endregion

    [SerializeField]
    Text spriteName;
    [SerializeField]
    Image spriteImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplaySpriteInfo(Sprite sprite) {
        spriteName.text = sprite.gameObject.name;
        spriteImage.sprite = sprite.GetComponent<SpriteRenderer>().sprite;
    }

    public void ResetDisplay() {
        spriteName.text = null;
        spriteImage.sprite = null;
    }

}
