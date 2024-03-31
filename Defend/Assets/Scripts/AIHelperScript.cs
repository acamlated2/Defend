using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIHelperScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> groundBlocks = new List<GameObject>();
    public List<GameObject> roadBlocks = new List<GameObject>();
    
    [SerializeField] private float normalStrength;
    [SerializeField] private float siegeStrength;
    [SerializeField] private float magicStrength;

    private GameObject _towerSpreadSlider;
    private GameObject _typeSensitivitySlider;
    private GameObject _upgradeSensitivitySlider;

    private GameObject _archerTowerButton;
    private GameObject _siegeTowerButton;
    private GameObject _magicTowerButton;

    private GameObject _upgradeUI;

    private void Awake()
    {
        _towerSpreadSlider = GameObject.FindGameObjectWithTag("Tower Spread Slider");
        _typeSensitivitySlider = GameObject.FindGameObjectWithTag("Type Sensitivity Slider");
        _upgradeSensitivitySlider = GameObject.FindGameObjectWithTag("Upgrade Sensitivity Slider");

        _archerTowerButton = GameObject.FindGameObjectWithTag("Archer Tower Button");
        _siegeTowerButton = GameObject.FindGameObjectWithTag("Siege Tower Button");
        _magicTowerButton = GameObject.FindGameObjectWithTag("Magic Tower Button");

        _upgradeUI = GameObject.FindGameObjectWithTag("Upgrade UI");
    }

    private void Start()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");

        for (int i = 0; i < blocks.Length; i++)
        {
            switch (blocks[i].GetComponent<BlockScript>().type)
            {
                case BlockScript.BlockType.Ground:
                    groundBlocks.Add(blocks[i].gameObject);
                    break;
                
                case BlockScript.BlockType.Road:
                    roadBlocks.Add(blocks[i].gameObject);
                    break;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            GetSuggestion();
        }
    }

    public void GetSuggestion()
    {
        TowerManagerScript.TowerType suggestedType = GetSuggestedTowerType();
        if (SuggestUpgrade(suggestedType, out GameObject suggestedTower))
        {
            Debug.Log("upgrade suggested");
            _archerTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
            _siegeTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
            _magicTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
            
            _upgradeUI.GetComponent<UpgradeUIScript>().OpenUpgrade(suggestedTower);
            _upgradeUI.GetComponent<UpgradeUIScript>().UnSuggestUpgrades();
            _upgradeUI.GetComponent<UpgradeUIScript>().SuggestByType(suggestedType);
            
            GetComponent<PlayerInputController>().IndicateObject(suggestedTower);
        }
        else
        {
            Debug.Log("new tower suggested");
            _upgradeUI.GetComponent<UpgradeUIScript>().CloseUpgrade();
            GetSuggestedTowerPlacement(GetComponent<TowerManagerScript>().GetPrefabByType(suggestedType));
            GetComponent<PlayerInputController>().HideUpgradeIndicatorCube();
        }
    }

    private void GetSuggestedTowerPlacement(GameObject tower)
    {
        float highestCoverageSum = float.NegativeInfinity;
        GameObject blockWithHighestSum = new GameObject();
        
        for (int i = 0; i < groundBlocks.Count; i++)
        {
            if (groundBlocks[i].GetComponent<GroundBlockScript>().hasTower)
            {
                continue;
            }

            float towerRange = tower.GetComponent<BaseTowerScript>()
                .CalculateRange(groundBlocks[i].GetComponent<GroundBlockScript>().height);
            
            Vector3 towerLocation = groundBlocks[i].transform.position + new Vector3(0, 2, 0);

            float coverageSum = 0;
            
            for (int j = 0; j < roadBlocks.Count; j++)
            {
                float distance = Vector3.Distance(towerLocation, roadBlocks[j].transform.position);
                
                if (distance <= towerRange)
                {
                    float roadBlockCoverageValue = roadBlocks[j].GetComponent<RoadBlockScript>().coverageValue;

                    coverageSum -= (roadBlockCoverageValue / towerRange) *
                                   _towerSpreadSlider.GetComponent<SliderScript>().sliderValue;
                    coverageSum += towerRange - distance / towerRange;
                }
            }
            
            if (coverageSum >= highestCoverageSum)
            {
                highestCoverageSum = coverageSum;
                blockWithHighestSum = groundBlocks[i];
            }
        }
        
        blockWithHighestSum.GetComponent<Renderer>().material.color = Color.blue;
    }

    public void AssignCoverageValues(GameObject tower)
    {
        float towerRange = tower.GetComponent<BaseTowerScript>().range;
        
        for (int i = 0; i < roadBlocks.Count; i++)
        {
            float distance = Vector3.Distance(tower.transform.position, roadBlocks[i].transform.position);

            if (distance <= towerRange)
            {
                roadBlocks[i].GetComponent<RoadBlockScript>().coverageValue += towerRange - distance / towerRange;
            }
        }
    }

    private TowerManagerScript.TowerType GetSuggestedTowerType()
    {
        GameObject[] enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] towersInScene = GetComponent<TowerManagerScript>().Towers.ToArray();

        float healthTotal = 0;
        float shieldTotal = 0;
        float armorTotal = 0;
        
        for (int i = 0; i < enemiesInScene.Length; i++)
        {
            healthTotal += enemiesInScene[i].GetComponent<BaseEnemyScript>().health;
            shieldTotal += enemiesInScene[i].GetComponent<BaseEnemyScript>().shield;
            armorTotal += enemiesInScene[i].GetComponent<BaseEnemyScript>().armor;
        }

        float normalDamageTotal = 0;
        float siegeDamageTotal = 0;
        float magicDamageTotal = 0;

        for (int i = 0; i < towersInScene.Length; i++)
        {
            normalDamageTotal += towersInScene[i].GetComponent<BaseTowerScript>().normalDamage;
            siegeDamageTotal += towersInScene[i].GetComponent<BaseTowerScript>().siegeDamage;
            magicDamageTotal += towersInScene[i].GetComponent<BaseTowerScript>().magicDamage;
        }

        float healthDangerLevel = healthTotal / 3 - normalDamageTotal;
        float shieldDangerLevel = (shieldTotal - siegeDamageTotal) *
                                  _typeSensitivitySlider.GetComponent<SliderScript>().sliderValue;
        float armorDangerLevel = (armorTotal - magicDamageTotal) *
                                 _typeSensitivitySlider.GetComponent<SliderScript>().sliderValue;
        
        Debug.Log("health " + healthDangerLevel);
        Debug.Log("shield " + shieldDangerLevel);
        Debug.Log("armor " + armorDangerLevel);

        float highestDangerValue = healthDangerLevel;
        TowerManagerScript.TowerType suggestedTowerType = TowerManagerScript.TowerType.Archer;

        if (shieldDangerLevel > highestDangerValue)
        {
            highestDangerValue = shieldDangerLevel;
            suggestedTowerType = TowerManagerScript.TowerType.Siege;
        }

        if (armorDangerLevel > highestDangerValue)
        {
            highestDangerValue = armorDangerLevel;
            suggestedTowerType = TowerManagerScript.TowerType.Magic;
        }
        
        switch (suggestedTowerType)
        {
            case TowerManagerScript.TowerType.Archer:
                _archerTowerButton.GetComponent<TowerButtonScript>().Suggest();
                _siegeTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
                _magicTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
                break;
            
            case TowerManagerScript.TowerType.Siege:
                _archerTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
                _siegeTowerButton.GetComponent<TowerButtonScript>().Suggest();
                _magicTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
                break;
            
            case TowerManagerScript.TowerType.Magic:
                _archerTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
                _siegeTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
                _magicTowerButton.GetComponent<TowerButtonScript>().Suggest();
                break;
        }

        return suggestedTowerType;
    }

    private bool SuggestUpgrade(TowerManagerScript.TowerType type, out GameObject suggestedTower)
    {
        TowerManagerScript towerManagerScript = GetComponent<TowerManagerScript>();
        
        float newTypeSpecificDamage = towerManagerScript.GetPrefabByType(type)
            .GetComponent<BaseTowerScript>().GetTypeSpecificDamageByType(type);
        float newTowerPrice = GetComponent<EconomyManager>().GetTowerPriceByType(type);

        float bestProjectedPrice = float.PositiveInfinity;
        GameObject bestSuggestedTower = new GameObject();
        
        for (int i = 0; i < towerManagerScript.Towers.Count; i++)
        {
            float upgradeDamageIncrement =
                towerManagerScript.Towers[i].GetComponent<BaseTowerScript>().upgradeDamageIncrement;

            float projectedDamage = 0;
            float projectedPrice = 0;

            suggestedTower = towerManagerScript.Towers[i];

            while (projectedDamage < newTypeSpecificDamage)
            {
                projectedDamage += upgradeDamageIncrement;
                projectedPrice +=
                    towerManagerScript.Towers[i].GetComponent<BaseTowerScript>()
                        .GetTypeSpecificUpgradePriceByType(type);
            }
            
            if (projectedPrice <= bestProjectedPrice)
            {
                bestProjectedPrice = projectedPrice;
                bestSuggestedTower = suggestedTower;
            }
        }
        
        suggestedTower = bestSuggestedTower;
        
        if (bestProjectedPrice < newTowerPrice * _upgradeSensitivitySlider.GetComponent<SliderScript>().sliderValue)
        {
            return true;
        }
        return false;
    }
}
