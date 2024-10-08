using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private GameObject Marker;
    [SerializeField] private GameObject NameText;
    [SerializeField] private BuildingBaseData buildingBaseData;

    public ObjectState objectState;

    public int nowHp;
    public int nowDamage;
    public int nowDefence;
    public void MarkedBuilding()
    {
        Marker.SetActive(true);
    }

    public void UnMarkedBuilding()
    {
        Marker.SetActive(false);
    }
    public void SetHp(int hp)
    {
        nowHp = hp;
    }
    public void SetDamage(int damage)
    {
        nowDamage = damage;

    }
    public void SetDefence(int defence)
    {
        nowDefence = defence;

    }
    public BuildingBaseData GetData()
    {
        return buildingBaseData;
    }
}

