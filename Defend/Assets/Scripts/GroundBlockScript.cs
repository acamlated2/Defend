using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GroundBlockScript : BlockScript
{
    public bool hasTower;
    public int height;

    [SerializeField] private GameObject stoolBlockPrefab;

    public bool suggested;

    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material suggestedMaterial;

    protected override void Awake()
    {
        base.Awake();
        type = BlockType.Ground;
        RandomizeHeight();
    }

    private void RandomizeHeight()
    {
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        float zPos = transform.position.z;
        int randInt = Random.Range(0, 21);

        switch (randInt)
        {
            case 0: case 1: case 2: case 3: case 4: case 11: case 12: case 13: case 14: case 15: case 16: case 17: case 18: case 19: case 20:
                transform.position = new Vector3(xPos, yPos + 0, zPos);
                height = 0;
                break;
            
            case 5: case 6: case 7:
                transform.position = new Vector3(xPos, yPos + 0.25f, zPos);
                CreateStool(new Vector3(xPos, transform.position.y - 1.125f, zPos));
                height = 1;
                break;
            
            case 8: case 9:
                transform.position = new Vector3(xPos, yPos + 0.5f, zPos);
                CreateStool(new Vector3(xPos, transform.position.y - 1.125f, zPos));
                CreateStool(new Vector3(xPos, transform.position.y - 1.375f, zPos));
                height = 2;
                break;
            
            case 10:
                transform.position = new Vector3(xPos, yPos + 0.75f, zPos);
                CreateStool(new Vector3(xPos, transform.position.y - 1.125f, zPos));
                CreateStool(new Vector3(xPos, transform.position.y - 1.375f, zPos));
                CreateStool(new Vector3(xPos, transform.position.y - 1.625f, zPos));
                height = 3;
                break;
        }
    }

    private void CreateStool(Vector3 pos)
    {
        GameObject newStool = Instantiate(stoolBlockPrefab, pos, Quaternion.identity);
        newStool.transform.SetParent(transform);
    }

    public void AddStool()
    {
        var position = transform.position;
        position = new Vector3(position.x, position.y + 0.25f, position.z);
        transform.position = position;

        height += 1;
        
        CreateStool(new Vector3(position.x, position.y - 1.125f - 0.25f * (height - 1), position.z));
    }

    public void Suggest()
    {
        suggested = true;
        GetComponent<Renderer>().material = suggestedMaterial;
    }

    public void UnSuggest()
    {
        suggested = false;
        GetComponent<Renderer>().material = normalMaterial;
    }
}
