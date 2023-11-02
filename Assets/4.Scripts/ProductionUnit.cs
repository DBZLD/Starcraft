using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionUnit : MonoBehaviour
{
    [SerializeField]
    private GameObject unitPrefab;
    [SerializeField]
    private LayerMask layerSpawn;

    private Vector3 pos;
    private Vector3 SpawnerScale;
    private Vector3 UnitScale;
    private Vector3 spawnPos;

    private new Collider[] collider;

    private void Awake()
    {
        pos = gameObject.transform.position;
        SpawnerScale = gameObject.transform.lossyScale;
        UnitScale = unitPrefab.transform.lossyScale;

        for(int i = 0; i < 10; i++)
        {       
            GameObject.FindWithTag("GameController").GetComponent<UnitController>().AddUnitList(SpawnUnit());
        }
    }

    public UnitManager SpawnUnit()
    {
        if (!IsSetSpawnPos()) { Debug.Log("cant spawn"); }

        GameObject clone = Instantiate(unitPrefab, spawnPos, Quaternion.identity);
        UnitManager unit = clone.GetComponent<UnitManager>();

        return unit;
    }

    private bool IsSetSpawnPos()
    {
        Vector3 returnPos;
        Vector3 maxSize;
        Vector3 minSize;

        int re;

        maxSize.x = pos.x + SpawnerScale.x / 2 + UnitScale.x / 2 + 0.01f;
        minSize.x = pos.x - SpawnerScale.x / 2 - UnitScale.x / 2 - 0.01f;
        maxSize.z = pos.z + SpawnerScale.z / 2 + UnitScale.z / 2 + 0.01f;
        minSize.z = pos.z - SpawnerScale.z / 2 - UnitScale.z / 2 - 0.01f;

        returnPos = new Vector3(maxSize.x, UnitScale.y / 2, maxSize.z);
        re = 0;
        while (returnPos.z > minSize.z)
        {
            collider = Physics.OverlapBox(returnPos, UnitScale * 0.5f, Quaternion.identity, layerSpawn);
            if (collider.Length == 0)
            {
                spawnPos = returnPos;
                return true;
            }
            returnPos.z -= UnitScale.z - 0.01f;
            re++;
            if(re >= 100) { break; }
        }

        returnPos = new Vector3(maxSize.x, UnitScale.y / 2, minSize.z);
        re = 0;
        while (returnPos.x > minSize.x)
        {
            collider = Physics.OverlapBox(returnPos, UnitScale * 0.5f, Quaternion.identity, layerSpawn);
            if (collider.Length == 0)
            {
                spawnPos = returnPos;
                return true;
            }
            returnPos.x -= UnitScale.x - 0.01f;
            re++;
            if (re >= 100) { break; }
        }

        returnPos = new Vector3(minSize.x, UnitScale.y / 2, minSize.z);
        re = 0;
        while (returnPos.z < maxSize.z)
        {
            collider = Physics.OverlapBox(returnPos, UnitScale * 0.5f, Quaternion.identity, layerSpawn);
            if (collider.Length == 0)
            {
                spawnPos = returnPos;
                return true;
            }
            returnPos.z += UnitScale.z + 0.01f;
            re++;
            if (re >= 100) { break; }
        }

        returnPos = new Vector3(minSize.x, UnitScale.y / 2, maxSize.z);
        re = 0;
        while (returnPos.x < maxSize.x)
        {
            collider = Physics.OverlapBox(returnPos, UnitScale * 0.5f, Quaternion.identity, layerSpawn);
            if (collider.Length == 0)
            {
                spawnPos = returnPos;
                return true;
            }
            returnPos.x += UnitScale.x + 0.01f;
            re++;
            if (re >= 100) { break; }
        }
        return false;
    }
}
