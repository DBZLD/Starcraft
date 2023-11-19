using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowStateUI : MonoBehaviour
{
    [SerializeField] private TMP_Text objectName;
    [SerializeField] private TMP_Text objectHp;
    [SerializeField] private TMP_Text unitState;
    [SerializeField] private TMP_Text materialRemain;

    private UnitController m_UnitController;
    private BuildingController m_BuildingController;
    private MaterialController m_MaterialController;

    private void Awake()
    {
        objectName.gameObject.SetActive(false);
        objectHp.gameObject.SetActive(false);
        unitState.gameObject.SetActive(false);

        m_UnitController = GetComponent<UnitController>();
        m_BuildingController = GetComponent<BuildingController>();
        m_MaterialController = GetComponent<MaterialController>();

        StartCoroutine(ShowUnitState(m_UnitController.SelectUnitList, m_BuildingController.SelectingBuilding, m_MaterialController.SelectingMaterial));
    }
    private IEnumerator ShowUnitState(List<UnitManager> unitList, BuildingManager building, MaterialManager material)
    {  
        while(true)
        {
            objectName.gameObject.SetActive(false);
            objectHp.gameObject.SetActive(false);
            unitState.gameObject.SetActive(false);
            materialRemain.gameObject.SetActive(false);

            if (unitList.Count == 1)
            {
                objectName.gameObject.SetActive(true);
                objectHp.gameObject.SetActive(true);
                unitState.gameObject.SetActive(true);

                objectName.text = unitList[0].GetData().unitName.ToString();
                objectHp.text = unitList[0].nowHp.ToString() + "/" + unitList[0].GetData().maxHp.ToString();
                unitState.text = unitList[0].unitState.ToString();
            }
            else if(unitList.Count > 1)
            {
                int uiPriority = m_UnitController.UIPriority();

                objectName.gameObject.SetActive(true);
                objectHp.gameObject.SetActive(true);
                unitState.gameObject.SetActive(true);

                objectName.text = unitList[uiPriority].GetData().unitName.ToString();
                objectHp.text = unitList[uiPriority].nowHp.ToString() + "/" + unitList[uiPriority].GetData().maxHp.ToString();
                unitState.text = unitList[uiPriority].unitState.ToString();
            }

            if(building != null)
            {
                objectName.gameObject.SetActive(true);
                objectHp.gameObject.SetActive(true);

                objectName.text = building.GetData().buildingName.ToString();
                objectHp.text = building.nowHp.ToString() + "/" + building.GetData().buildingMaxHp.ToString();
            }
            if(material != null)
            {
                objectName.gameObject.SetActive(true);
                materialRemain.gameObject.SetActive(true);

                objectName.text = material.materialType.ToString();
                materialRemain.text = material.remainMaterial.ToString();
            }
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}
