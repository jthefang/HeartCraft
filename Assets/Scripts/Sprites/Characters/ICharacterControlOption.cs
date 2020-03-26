using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterControlOption {
    void DisplayUI();
    bool IsActive {
        get;
    }
    bool OptionTurnsActive(); //how the option turns active

    /**
    bool _isActive;
    public bool IsActive {
        get;
    }

    e.g.
    public class MoveOption {
        ...
        public bool OptionTurnsActive() {
            bool rightClick = Input.GetMouseButtonDown(1);
            return Input.GetKeyDown(KeyCode.M) || rightclick;
        }
    }
    */
}
