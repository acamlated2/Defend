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

    private GameObject _archerTowerButton;
    private GameObject _siegeTowerButton;
    private GameObject _magicTowerButton;

    private void Awake()
    {
        _towerSpreadSlider = GameObject.FindGameObjectWithTag("Tower Spread Slider");
        _typeSensitivitySlider = GameObject.FindGameObjectWithTag("Type Sensitivity Slider");

        _archerTowerButton = GameObject.FindGameObjectWithTag("Archer Tower Button");
        _siegeTowerButton = GameObject.FindGameObjectWithTag("Siege Tower Button");
        _magicTowerButton = GameObject.FindGameObjectWithTag("Magic Tower Button");
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
            SuggestPlacement();
        }
    }

    public void SuggestPlacement()
    {
        GetSuggestedTowerPlacement(GetSuggestedTower());
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

        switch (tower.GetComponent<BaseTowerScript>().type)
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

    private GameObject GetSuggestedTower()
    {
        GameObject[] enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] towersInScene = GetComponent<TowerManagerScript>().Towers.ToArray();
        GameObject[] towerPrefabs =  GetComponent<TowerManagerScript>().towerPrefabs;

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
        GameObject suggestedTower = towerPrefabs[0];

        if (shieldDangerLevel > highestDangerValue)
        {
            highestDangerValue = shieldDangerLevel;
            suggestedTower = towerPrefabs[1];
        }

        if (armorDangerLevel > highestDangerValue)
        {
            highestDangerValue = armorDangerLevel;
            suggestedTower = towerPrefabs[2];
        }

        return suggestedTower;
    }
}
