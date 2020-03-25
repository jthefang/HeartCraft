using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    A character is a sprite the player can control
*/
public class Character : Sprite
{   
    protected override void Init() {
        base.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool IsCharacter() {
        return true;
    }
}
