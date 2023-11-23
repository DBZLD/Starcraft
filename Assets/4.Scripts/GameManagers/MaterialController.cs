using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
    [SerializeField]
    public MaterialManager SelectingMaterial;
    [SerializeField]
    public List<MaterialManager> AllMaterialList;

    public void ClickSelectMaterial(MaterialManager NewMaterial)
    {
        SelectMaterial(NewMaterial);
    }
    private void SelectMaterial(MaterialManager NewMaterial)
    {
        NewMaterial.MarkedMaterial();

        SelectingMaterial = NewMaterial;
    }
    public void UnselectMaterial()
    {
        SelectingMaterial.UnMarkedMaterial();

        SelectingMaterial = null;
    }

    public void AddUnitList(MaterialManager NewMaterial)
    {
        AllMaterialList.Add(NewMaterial);
    }
}
