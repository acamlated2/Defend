using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public enum BlockType
    {
        Ground, 
        Road, 
        Null
    }

    protected virtual void Awake()
    {
        
    }

    public BlockType type = BlockType.Ground;
}
