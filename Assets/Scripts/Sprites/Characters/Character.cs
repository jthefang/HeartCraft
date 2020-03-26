using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    A character is a sprite the player can control
*/
public class Character : Sprite
{   
    HashSet<ICharacterControlOption> controlOptions;
    Dictionary<string, ICharacterControlOption> componentNameToControlOption;

    protected override void Init() {
        base.Init();

        InitializeControlOptions();
    }

    void InitializeControlOptions() {
        controlOptions = new HashSet<ICharacterControlOption>();
        componentNameToControlOption = new Dictionary<string, ICharacterControlOption>();
        
        Component[] currControlOptionComponents = GetComponents(typeof(ICharacterControlOption));
        foreach (Component controlComponent in currControlOptionComponents) {
            ICharacterControlOption controlOption = (ICharacterControlOption) controlComponent;
            string controlOptionName = controlOption.GetType().ToString();
            controlOptions.Add(controlOption);
            componentNameToControlOption.Add(controlOptionName, controlOption);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool IsCharacter() {
        return true;
    }

    public HashSet<ICharacterControlOption> GetControlOptions() {
        return controlOptions;
    }

    public bool HasControlOptionName(string optionName) {
        return componentNameToControlOption.ContainsKey(optionName);
    }

    public override void OnSelected() {
        base.OnSelected();
        DisplayControlOptions();
    }

    public virtual void DisplayControlOptions() {
        foreach (ICharacterControlOption option in controlOptions) {
            option.DisplayUI();
        }
    }

}
