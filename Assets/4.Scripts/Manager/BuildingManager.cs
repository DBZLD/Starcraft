using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [Header ("System")]
    [SerializeField] private GameObject Marker;
    [SerializeField] private GameObject NameText;
    [SerializeField] private BuildingBaseData buildingBaseData;

    [Header ("State")]
    public ObjectState objectState;
    public bool CanAttack;

    [Header ("Status")]
    public int nowHp;
    public int nowDamage;
    public int nowDefence;
    private void Awake()
    {
        Marker.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        Marker.transform.localPosition = new Vector3(0, -transform.lossyScale.y / 2 + 0.01f, 0);
        Marker.SetActive(false);

        NameText.transform.localPosition = new Vector3(0, transform.lossyScale.y / 2, 0);
        NameText.transform.rotation = Quaternion.Euler(90, 0, 0);

        SetHp(nowHp);
        SetDamage(nowDamage);
        SetDefence(nowDefence);

        CanAttack = true;
    }
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

