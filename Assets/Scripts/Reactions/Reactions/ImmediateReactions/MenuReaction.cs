using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuReaction : Reaction
{
    public  GameObject Main_menu;                    // The name of the scene to be loaded.
    public List<GameObject> secondaryMenu = new List<GameObject>();

    protected override void SpecificInit()
    {

    }

    protected override void ImmediateReaction()
    {

        // Start the scene loading process.
        Main_menu.SetActive(true);
        foreach (var go in secondaryMenu)
        {
            go.SetActive(false);
        }
    }
}
