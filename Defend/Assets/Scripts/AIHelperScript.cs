using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHelperScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> groundBlocks = new List<GameObject>();
    public List<GameObject> roadBlocks = new List<GameObject>();

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
        GameObject tower = GetComponent<TowerManagerScript>().towerPrefabs[0];
        GetSuggestedTowerPlacement(tower);
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
                    
                    coverageSum -= roadBlockCoverageValue / towerRange;
                    //coverageSum += 100 - roadBlockCoverageValue / float.PositiveInfinity;
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
        blockWithHighestSum.transform.name = "oiuansodifu";
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
}
