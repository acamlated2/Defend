using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelectorScript : MonoBehaviour
{
    private TowerManagerScript.TowerType selectedTowerType = TowerManagerScript.TowerType.Archer;

    public void ChangeSelection(TowerManagerScript.TowerType type)
    {
        selectedTowerType = type;
    }

    public TowerManagerScript.TowerType GetSelectedType()
    {
        return selectedTowerType;
    }
}
