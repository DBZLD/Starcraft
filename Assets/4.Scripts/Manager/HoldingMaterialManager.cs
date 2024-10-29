using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class HoldingMaterialManager : MonoBehaviour
{
    [SerializeField] private GameObject MineralChunk;
    [SerializeField] private GameObject BespeneGasTank;

    [SerializeField] UnityEvent<MaterialType, int> onPutMaterial;

    public MaterialType holdingMaterialType;
    public int holdingMaterialValue;

    public void GatherMaterial(MaterialType materialType, int materialValue)
    {
        holdingMaterialType = materialType;
        holdingMaterialValue = materialValue;
    }
    public void PutMaterial()
    {
        onPutMaterial.Invoke(holdingMaterialType, holdingMaterialValue);

        holdingMaterialType = MaterialType.None;
        holdingMaterialValue = 0;
    }
}
