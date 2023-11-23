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
    [SerializeField] private TMP_Text Mineral;
    [SerializeField] private TMP_Text BespeneGas;
    [SerializeField] private TMP_Text Supply;

    private UnitController m_UnitController;
    private BuildingController m_BuildingController;
    private MaterialController m_MaterialController;
    private PlayerManager m_PlayerManager;

    private void Awake()
    {
        objectName.gameObject.SetActive(false);
        objectHp.gameObject.SetActive(false);
        unitState.gameObject.SetActive(false);
        materialRemain.gameObject.SetActive(false);

        m_UnitController = GetComponent<UnitController>();
        m_BuildingController = GetComponent<BuildingController>();
        m_MaterialController = GetComponent<MaterialController>();
        m_PlayerManager = GetComponent<PlayerManager>();
        
        StartCoroutine(ShowUnitState());
    }
    public void SetMineral()
    {
        Mineral.text = m_PlayerManager.mineral.ToString();
    }
    public void SetBespeneGas()
    {
        BespeneGas.text = m_PlayerManager.bespeneGas.ToString();
    }
    public void SetSupply()
    {
        Supply.text = m_PlayerManager.nowSupply.ToString() + "/" + m_PlayerManager.GetData().maxSupply.ToString();
    }
    private IEnumerator ShowUnitState()
    {  
        while(true)
        {
            objectName.gameObject.SetActive(false);
            objectHp.gameObject.SetActive(false);
            unitState.gameObject.SetActive(false);
            materialRemain.gameObject.SetActive(false);

            if (m_UnitController.SelectUnitList.Count == 1)
            {
                objectName.gameObject.SetActive(true);
                objectHp.gameObject.SetActive(true);
                unitState.gameObject.SetActive(true);

                objectName.text = m_UnitController.SelectUnitList[0].GetData().unitName.ToString();
                objectHp.text = m_UnitController.SelectUnitList[0].nowHp.ToString() + "/" + m_UnitController.SelectUnitList[0].GetData().maxHp.ToString();
                unitState.text = m_UnitController.SelectUnitList[0].unitState.ToString();
            }
            else if(m_UnitController.SelectUnitList.Count > 1)
            {
                int uiPriority = m_UnitController.UIPriority();

                objectName.gameObject.SetActive(true);
                objectHp.gameObject.SetActive(true);
                unitState.gameObject.SetActive(true);

                objectName.text = m_UnitController.SelectUnitList[uiPriority].GetData().unitName.ToString();
                objectHp.text = m_UnitController.SelectUnitList[uiPriority].nowHp.ToString() + "/" + m_UnitController.SelectUnitList[uiPriority].GetData().maxHp.ToString();
                unitState.text = m_UnitController.SelectUnitList[uiPriority].unitState.ToString();
            }

            if(m_BuildingController.SelectingBuilding != null)
            {
                objectName.gameObject.SetActive(true);
                objectHp.gameObject.SetActive(true);

                objectName.text = m_BuildingController.SelectingBuilding.GetData().buildingName.ToString();
                objectHp.text = m_BuildingController.SelectingBuilding.nowHp.ToString() + "/" + m_BuildingController.SelectingBuilding.GetData().buildingMaxHp.ToString();
            }
            if(m_MaterialController.SelectingMaterial != null)
            {
                objectName.gameObject.SetActive(true);
                materialRemain.gameObject.SetActive(true);

                objectName.text = m_MaterialController.SelectingMaterial.materialType.ToString();
                materialRemain.text = m_MaterialController.SelectingMaterial.remainMaterial.ToString();
            }
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}
