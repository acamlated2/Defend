using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIHelperScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> groundBlocks = new List<GameObject>();
    public List<GameObject> roadBlocks = new List<GameObject>();

    private SliderScript _towerSpreadSlider;
    private SliderScript _typeSensitivitySlider;
    private SliderScript _upgradeSensitivitySlider;
    private SliderScript _platformSensitivitySlider;

    private GameObject _archerTowerButton;
    private GameObject _siegeTowerButton;
    private GameObject _magicTowerButton;
    private GameObject _platformTowerButton;

    private GameObject _upgradeUI;

    private GameObject _lastSuggestedBlock;

    private void Awake()
    {
        _towerSpreadSlider =
            GameObject.FindGameObjectWithTag("Tower Spread Slider").GetComponent<SliderScript>();
        _typeSensitivitySlider = GameObject.FindGameObjectWithTag("Type Sensitivity Slider")
                                           .GetComponent<SliderScript>();
        _upgradeSensitivitySlider = GameObject.FindGameObjectWithTag("Upgrade Sensitivity Slider")
                                              .GetComponent<SliderScript>();
        _platformSensitivitySlider = GameObject.FindGameObjectWithTag("Platform Sensitivity Slider")
                                            .GetComponent<SliderScript>();

        _archerTowerButton = GameObject.FindGameObjectWithTag("Archer Tower Button");
        _siegeTowerButton = GameObject.FindGameObjectWithTag("Siege Tower Button");
        _magicTowerButton = GameObject.FindGameObjectWithTag("Magic Tower Button");
        _platformTowerButton = GameObject.FindGameObjectWithTag("Platform Tower Button");

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
        if (SuggestPlatform(suggestedType))
        {
            Debug.Log("New Platform Suggested");
            _upgradeUI.GetComponent<UpgradeUIScript>().CloseUpgrade();
            GetComponent<PlayerInputController>().HideUpgradeIndicatorCube();
            _archerTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
            _siegeTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
            _magicTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
            _platformTowerButton.GetComponent<TowerButtonScript>().Suggest();
            
            GetComponent<TowerSelectorScript>().ChangeSelection(TowerManagerScript.TowerType.Platform);
            return;
        }
        
        if (SuggestUpgrade(suggestedType, out GameObject suggestedTower))
        {
            Debug.Log("upgrade suggested");
            _archerTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
            _siegeTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
            _magicTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
            _platformTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
            _lastSuggestedBlock.GetComponent<GroundBlockScript>().UnSuggest();
            
            _upgradeUI.GetComponent<UpgradeUIScript>().OpenUpgrade(suggestedTower);
            _upgradeUI.GetComponent<UpgradeUIScript>().UnSuggestUpgrades();
            _upgradeUI.GetComponent<UpgradeUIScript>().SuggestByType(suggestedType);
            
            GetComponent<PlayerInputController>().IndicateObject(suggestedTower);
            return;
        }
        Debug.Log("new tower suggested");
        _upgradeUI.GetComponent<UpgradeUIScript>().CloseUpgrade();
        GetSuggestedTowerPlacement(GetComponent<TowerManagerScript>().GetPrefabByType(suggestedType));
        GetComponent<PlayerInputController>().HideUpgradeIndicatorCube();
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

            float coverageSum = GetTowerCoverageSum(groundBlock, tower, 0);
            
            if (coverageSum >= highestCoverageSum)
            {
                highestCoverageSum = coverageSum;
                blockWithHighestSum = groundBlock;
            }
        }

        if (_lastSuggestedBlock)
        {
            _lastSuggestedBlock.GetComponent<GroundBlockScript>().UnSuggest();
        }
        blockWithHighestSum.GetComponent<GroundBlockScript>().Suggest();
        _lastSuggestedBlock = blockWithHighestSum;
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
        float shieldDangerLevel = (shieldTotal - siegeDamageTotal) * _typeSensitivitySlider.sliderValue;
        float armorDangerLevel = (armorTotal - magicDamageTotal) * _typeSensitivitySlider.sliderValue;

        Debug.Log("health " + healthDangerLevel + " shield " + shieldDangerLevel 
                  + " armor " + armorDangerLevel);

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
                _platformTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
                GetComponent<TowerSelectorScript>().ChangeSelection(TowerManagerScript.TowerType.Archer);
                break;
            
            case TowerManagerScript.TowerType.Siege:
                _archerTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
                _siegeTowerButton.GetComponent<TowerButtonScript>().Suggest();
                _magicTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
                _platformTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
                GetComponent<TowerSelectorScript>().ChangeSelection(TowerManagerScript.TowerType.Siege);
                break;
            
            case TowerManagerScript.TowerType.Magic:
                _archerTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
                _siegeTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
                _magicTowerButton.GetComponent<TowerButtonScript>().Suggest();
                _platformTowerButton.GetComponent<TowerButtonScript>().UnSuggest();
                GetComponent<TowerSelectorScript>().ChangeSelection(TowerManagerScript.TowerType.Magic);
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
        
        if (bestProjectedPrice < newTowerPrice * _upgradeSensitivitySlider.sliderValue)
        {
            return true;
        }
        return false;
    }

    private bool SuggestPlatform(TowerManagerScript.TowerType suggestedType)
    {
        EconomyManager economyManager = GetComponent<EconomyManager>();
        
        float priceSum = economyManager.GetTowerPriceByType(suggestedType) +
                         economyManager.GetTowerPriceByType(TowerManagerScript.TowerType.Platform);

        GameObject highestGroundBlock = new GameObject();
        int highestHeight = int.MinValue;

        foreach (var groundBlock in groundBlocks)
        {
            if (groundBlock.GetComponent<GroundBlockScript>().hasTower)
            {
                continue;
            }
            
            int height = groundBlock.GetComponent<GroundBlockScript>().height;

            if (height > highestHeight)
            {
                highestHeight = height;
                highestGroundBlock = groundBlock;
            }
        }

        GameObject tempTower = GetComponent<TowerManagerScript>().GetPrefabByType(suggestedType);
        float towerNormalCoverage = GetTowerCoverageSum(highestGroundBlock,  tempTower, 0);
        float towerWithPlatformCoverage = GetTowerCoverageSum(highestGroundBlock, tempTower, 1);

        float ratio = towerWithPlatformCoverage / towerNormalCoverage;

        float projectedPrice = economyManager.GetTowerPriceByType(suggestedType) * ratio;
        
        if (_lastSuggestedBlock)
        {
            _lastSuggestedBlock.GetComponent<GroundBlockScript>().UnSuggest();
        }
        highestGroundBlock.GetComponent<GroundBlockScript>().Suggest();
        _lastSuggestedBlock = highestGroundBlock;
        
        if (projectedPrice * _platformSensitivitySlider.sliderValue > priceSum)
        {
            return true;
        }
        return false;
    }

    private float GetTowerCoverageSum(GameObject groundBlock, GameObject tower, int heightIncrease)
    {
        float towerRange = tower.GetComponent<BaseTowerScript>()
                                .CalculateRange(groundBlock.GetComponent<GroundBlockScript>().height +
                                                heightIncrease);
            
        Vector3 towerLocation = groundBlock.transform.position + new Vector3(0, 2, 0);

        float coverageSum = 0;
        
        foreach (var roadblock in roadBlocks)
        {
            float distance = Vector3.Distance(towerLocation, roadblock.transform.position);
                
            if (distance <= towerRange)
            {
                float roadBlockCoverageValue = roadblock.GetComponent<RoadBlockScript>().coverageValue;

                coverageSum -= (roadBlockCoverageValue / towerRange) * _towerSpreadSlider.sliderValue;
                coverageSum += towerRange - distance / towerRange;
            }
        }

        return coverageSum;
    }
}
