using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Node 
{
    public int x = 0;
    public int y = 0;
    public float posX = 0;
    public float weight = 1;
    public bool occupied = false;
    public Node up;
    public Node down;
    public Node left;
    public Node right;
}
