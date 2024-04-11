using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIHelperScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> groundBlocks = new List<GameObject>();
    public List<GameObject> roadBlocks = new List<GameObject>();

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

        foreach (var block in blocks)
        {
            switch (block.GetComponent<BlockScript>().type)
            {
                case BlockScript.BlockType.Ground:
                    groundBlocks.Add(block.gameObject);
                    break;
                
                case BlockScript.BlockType.Road:
                    roadBlocks.Add(block.gameObject);
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

    private void GetSuggestion()
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
        
        foreach (var groundBlock in groundBlocks)
        {
            if (groundBlock.GetComponent<GroundBlockScript>().hasTower)
            {
                continue;
            }

            float towerRange = tower.GetComponent<BaseTowerScript>()
                                    .CalculateRange(groundBlock.GetComponent<GroundBlockScript>().height);
            
            Vector3 towerLocation = groundBlock.transform.position + new Vector3(0, 2, 0);

            float coverageSum = 0;
            
            foreach (var roadblock in roadBlocks)
            {
                float distance = Vector3.Distance(towerLocation, roadblock.transform.position);
                
                if (distance <= towerRange)
                {
                    float roadBlockCoverageValue = roadblock.GetComponent<RoadBlockScript>().coverageValue;

                    coverageSum -= (roadBlockCoverageValue / towerRange) *
                                   _towerSpreadSlider.GetComponent<SliderScript>().sliderValue;
                    coverageSum += towerRange - distance / towerRange;
                }
            }
            
            if (coverageSum >= highestCoverageSum)
            {
                highestCoverageSum = coverageSum;
                blockWithHighestSum = groundBlock;
            }
        }
        
        blockWithHighestSum.GetComponent<Renderer>().material.color = Color.blue;
    }

    public void AssignCoverageValues(GameObject tower)
    {
        float towerRange = tower.GetComponent<BaseTowerScript>().range;

        foreach (var roadBlock in roadBlocks)
        {
            float distance = Vector3.Distance(tower.transform.position, roadBlock.transform.position);

            if (distance <= towerRange)
            {
                roadBlock.GetComponent<RoadBlockScript>().coverageValue += towerRange - distance / towerRange;
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
        
        foreach (var enemy in enemiesInScene)
        {
            healthTotal += enemy.GetComponent<BaseEnemyScript>().health;
            shieldTotal += enemy.GetComponent<BaseEnemyScript>().shield;
            armorTotal += enemy.GetComponent<BaseEnemyScript>().armor;
        }

        float normalDamageTotal = 0;
        float siegeDamageTotal = 0;
        float magicDamageTotal = 0;

        foreach (var tower in towersInScene)
        {
            normalDamageTotal += tower.GetComponent<BaseTowerScript>().normalDamage;
            siegeDamageTotal += tower.GetComponent<BaseTowerScript>().siegeDamage;
            magicDamageTotal += tower.GetComponent<BaseTowerScript>().magicDamage;
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
        
        foreach (var tower in towerManagerScript.Towers)
        {
            float upgradeDamageIncrement =
                tower.GetComponent<BaseTowerScript>().upgradeDamageIncrement;

            float projectedDamage = 0;
            float projectedPrice  = 0;

            suggestedTower = tower;

            while (projectedDamage < newTypeSpecificDamage)
            {
                projectedDamage += upgradeDamageIncrement;
                projectedPrice +=
                    tower.GetComponent<BaseTowerScript>()
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
