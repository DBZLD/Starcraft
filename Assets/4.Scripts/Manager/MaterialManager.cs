using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    [SerializeField] private GameObject Marker;
    [SerializeField] private GameObject NameText;

    public int remainMaterial;
    public MaterialType materialType;
    public GameObject GatheringObject;
    public bool isRefinery;
    private void Start()
    {
        isRefinery = false;
    }

    public void MarkedMaterial()
    {
        Marker.SetActive(true);
    }

    public void UnMarkedMaterial()
    {
        Marker.SetActive(false);
    }

    public int GatheredMaterial()
    {
        if (remainMaterial < 8)
        {
            int re = remainMaterial;
            remainMaterial -= remainMaterial;
            GatheringObject = null;
            return re;
        }
        else
        {
            remainMaterial -= 8;
            GatheringObject = null;
            return 8;
        }
    }
}
