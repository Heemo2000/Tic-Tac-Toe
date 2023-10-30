using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLookHandler : MonoBehaviour
{
    [SerializeField]private MeshRenderer[] gridParts;

    public void SetColor(Color color)
    {
        foreach(MeshRenderer part in gridParts)
        {
            part.material.color = color;
        }
    }
}
