using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BuildingManager : MonoBehaviour
{
    [Header ("System")]
    [SerializeField] private GameObject Marker;
    [SerializeField] private GameObject NameText;
    [SerializeField] private BuildingBaseData buildingBaseData;
    private UnitController m_unitController;
    private PlayerManager m_PlayerManager;

    [Header ("State")]
    public ObjectState objectState;
    public bool CanAttack;
    public Coroutine productList;
    public ButtonPageNum nowButtonNumList;

    [Header ("Status")]
    public float nowHp;
    public float nowDamage;
    public float nowDefence;
    private Vector3 spawnPos;

    private void Awake()
    {
        m_unitController = FindObjectOfType<UnitController>();
        m_PlayerManager = FindObjectOfType<PlayerManager>();

        Marker.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        Marker.transform.localPosition = new Vector3(0, -transform.lossyScale.y / 2 + 0.01f, 0);
        Marker.SetActive(false);

        NameText.transform.localPosition = new Vector3(0, transform.lossyScale.y / 2, 0);
        NameText.transform.rotation = Quaternion.Euler(90, 0, 0);

        productList = null;
        SetHp();
        SetDamage();
        SetDefence();

        nowButtonNumList = buildingBaseData.mainButtonPage;

        CanAttack = true;
    }
    public IEnumerator ProductionUnit(GameObject unitPrefab, int time, int needSupply, int needMineral, int needVespene)
    {
        int timer = 0;
        while(true)
        {
            if(timer >= time)
            {
                if(m_PlayerManager.GetMineral() >= needMineral && m_PlayerManager.GetVespeneGas() >= needVespene && m_PlayerManager.GetMaxSupply() - m_PlayerManager.GetNowSupply() >= needSupply)
                {
                    if (!IsSetSpawnPos(unitPrefab)) { Debug.Log("cant spawn"); yield break; }
                    GameObject clone = Instantiate(unitPrefab, spawnPos, Quaternion.identity);
                    UnitManager unit = clone.GetComponent<UnitManager>();
                    m_unitController.AddUnitList(unit);
                    m_PlayerManager.DecreaseMaterial(MaterialType.Mineral, unit.GetData().costMineral);
                    m_PlayerManager.DecreaseMaterial(MaterialType.VespeneGas, unit.GetData().costVespeneGas);
                    m_PlayerManager.IncreaseNowSupply(unit.GetData().costSupply);
                    yield break;
                }
                else
                {
                    Debug.Log("not enought material");
                }
            }
            Debug.Log(timer);
            timer += 1;
            yield return new WaitForSeconds(1f);
        }

    }
    private bool IsSetSpawnPos(GameObject unit)
    {
        if(unit.GetComponent<UnitManager>().GetData().airGround == AirGround.Air) { spawnPos.x = transform.position.x; spawnPos.y = 5.5f; spawnPos.z = transform.position.z; return true; }
        else { spawnPos.y = 0.5f; }
        Vector3 returnPos;
        Vector3 maxSize;
        Vector3 minSize;

        Vector3 pos = transform.position;
        Vector3 SpawnerScale = transform.lossyScale;
        Vector3 UnitScale = unit.transform.lossyScale;
        int re = 0;
        Collider[] collider;

        maxSize.x = pos.x + SpawnerScale.x / 2 + UnitScale.x / 2 + 0.01f;
        minSize.x = pos.x - SpawnerScale.x / 2 - UnitScale.x / 2 - 0.01f;
        maxSize.z = pos.z + SpawnerScale.z / 2 + UnitScale.z / 2 + 0.01f;
        minSize.z = pos.z - SpawnerScale.z / 2 - UnitScale.z / 2 - 0.01f;

        //1
        returnPos = new Vector3(maxSize.x, 0.5f, maxSize.z);
        while (returnPos.z > minSize.z)
        {
            collider = Physics.OverlapBox(returnPos, UnitScale * 0.5f, Quaternion.identity);
            if (collider.Length <= 0)
            {
                spawnPos = returnPos;
                return true;
            }
            returnPos.z -= (UnitScale.z + 0.01f);
            re++;
            if (re >= 100) { break; }
        }
        //2
        returnPos = new Vector3(maxSize.x, 0.5f, minSize.z);
        while (returnPos.x > minSize.x)
        {
            collider = Physics.OverlapBox(returnPos, UnitScale * 0.5f, Quaternion.identity);
            if (collider.Length <= 0)
            {
                spawnPos = returnPos;
                return true;
            }
            returnPos.x -= (UnitScale.x + 0.01f);
            re++;
            if (re >= 100) { break; }
        }
        //3
        returnPos = new Vector3(minSize.x, 0.5f, minSize.z);
        while (returnPos.z < minSize.z)
        {
            collider = Physics.OverlapBox(returnPos, UnitScale * 0.5f, Quaternion.identity);
            if (collider.Length <= 0)
            {
                spawnPos = returnPos;
                return true;
            }
            returnPos.z += (UnitScale.z + 0.01f);
            re++;
            if (re >= 100) { break; }
        }
        returnPos = new Vector3(minSize.x, 0.5f, maxSize.z);
        while (returnPos.x < minSize.x)
        {
            collider = Physics.OverlapBox(returnPos, UnitScale * 0.5f, Quaternion.identity);
            if (collider.Length <= 0)
            {
                spawnPos = returnPos;
                return true;
            }
            returnPos.x += (UnitScale.x + 0.01f);
            re++;
            if (re >= 100) { break; }
        }
        return false;
    }

        public void MarkedBuilding()
    {
        Marker.SetActive(true);
    }

    public void UnMarkedBuilding()
    {
        Marker.SetActive(false);
    }
    public void SetHp()
    {
        nowHp = buildingBaseData.buildingMaxHp;
    }
    public void SetDamage()
    {
        nowDamage = buildingBaseData.buildingBaseDamage;

    }
    public void SetDefence()
    {
        nowDefence = buildingBaseData.buildingBaseDefense;

    }
    public BuildingBaseData GetData()
    {
        return buildingBaseData;
    }
}

