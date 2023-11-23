using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    [SerializeField] private GameObject Marker;
    [SerializeField] private GameObject NameText;

    public int remainMaterial;
    public MaterialType materialType;
    public bool isGathering;

    public void MarkedMaterial()
    {
        Marker.SetActive(true);
    }

    public void UnMarkedMaterial()
    {
        Marker.SetActive(false);
    }

}
