using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
    [SerializeField]
    public MaterialManager selectMaterial;
    [SerializeField]
    public List<MaterialManager> AllMaterialList;

    public void ClickSelectMaterial(MaterialManager NewMaterial)
    {
        UnselectMaterial();

        SelectMaterial(NewMaterial);
    }
    private void SelectMaterial(MaterialManager NewMaterial)
    {
        NewMaterial.MarkedMaterial();

        selectMaterial = NewMaterial;
    }
    public void UnselectMaterial()
    {
        selectMaterial.UnMarkedMaterial();

        selectMaterial = null;
    }

    public void AddUnitList(MaterialManager NewMaterial)
    {
        AllMaterialList.Add(NewMaterial);
    }
}
