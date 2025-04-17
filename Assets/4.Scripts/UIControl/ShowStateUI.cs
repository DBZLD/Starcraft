using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ShowStateUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nowMineral;
    [SerializeField] private TMP_Text nowVespeneGas;
    [SerializeField] private TMP_Text Supply;

    [SerializeField] private TMP_Text objectName;
    [SerializeField] private TMP_Text objectHp;
    [SerializeField] private TMP_Text objectDamage;
    [SerializeField] private TMP_Text objectDefence;
    [SerializeField] private TMP_Text objectEnergy;
    [SerializeField] private TMP_Text materialRemain;
    [SerializeField] private TMP_Text buttonInfomation;
    //debug

    private UnitController m_UnitController;
    private BuildingController m_BuildingController;
    private MaterialController m_MaterialController;
    private EnemyController m_EnemyController;
    private PlayerManager m_PlayerManager;
    private ButtonController m_ButtonController;

    
    private void Awake()
    {
        objectName.gameObject.SetActive(false);
        objectHp.gameObject.SetActive(false);
        objectDamage.gameObject.SetActive(false);
        objectDefence.gameObject.SetActive(false);
        objectEnergy.gameObject.SetActive(false);
        buttonInfomation.gameObject.SetActive(false);

        materialRemain.gameObject.SetActive(false);
        nowMineral.gameObject.SetActive(false);
        nowVespeneGas.gameObject.SetActive(false);
        Supply.gameObject.SetActive(false);

        m_UnitController = GetComponent<UnitController>();
        m_BuildingController = GetComponent<BuildingController>();
        m_MaterialController = GetComponent<MaterialController>();
        m_EnemyController = GetComponent<EnemyController>();
        m_PlayerManager = GetComponent<PlayerManager>();
        m_ButtonController = FindObjectOfType<ButtonController>();
        
        StartCoroutine(ShowUnitState());
        StartCoroutine(ShowPlayerMaterial());
        StartCoroutine(ShowButton());
    }
    private IEnumerator ShowButton()
    {
        while(true)
        {
            if(m_UnitController.selectUnitList.Count == 1)
            {
                m_ButtonController.SetButtonImage(m_UnitController.selectUnitList[0].GetComponent<UnitManager>().nowButtonNumList);
            }
            else if(m_UnitController.selectUnitList.Count > 1)
            {
                m_ButtonController.SetButtonImage(ButtonPageNum.UnitsNormal);
            }
            else if(m_BuildingController.selectBuilding != null)
            {
                m_ButtonController.SetButtonImage(m_BuildingController.selectBuilding.GetComponent<BuildingManager>().nowButtonNumList);
            }
            else
            {
                m_ButtonController.SetButtonImage(ButtonPageNum.None);
            }
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator ShowUnitState()
    {  
        while(true)
        {
            objectName.gameObject.SetActive(false);
            objectHp.gameObject.SetActive(false);
            objectDamage.gameObject.SetActive(false);
            objectDefence.gameObject.SetActive(false);
            objectEnergy.gameObject.SetActive(false);

            materialRemain.gameObject.SetActive(false);


            if (m_UnitController.selectUnitList.Count == 1)
            {
                objectName.gameObject.SetActive(true);
                objectHp.gameObject.SetActive(true);
                objectDamage.gameObject.SetActive(true);
                objectDefence.gameObject.SetActive(true);
                if (m_UnitController.selectUnitList[0].GetData().isMagic == true) { objectEnergy.gameObject.SetActive(true); }

                objectName.text = m_UnitController.selectUnitList[0].GetData().unitName.ToString();
                objectHp.text = "체력 : " + m_UnitController.selectUnitList[0].nowHp.ToString() + "/" + m_UnitController.selectUnitList[0].GetData().maxHp.ToString();
                if (m_UnitController.selectUnitList[0].GetData().isAttack == true) objectDamage.text = "공격력 : " + m_UnitController.selectUnitList[0].nowDamage.ToString();
                objectDefence.text = "방어력 : " + m_UnitController.selectUnitList[0].nowDefence.ToString();
                if (m_UnitController.selectUnitList[0].GetData().isMagic == true) objectEnergy.text = "에너지 : " + m_UnitController.selectUnitList[0].nowEnergy.ToString() + "/" + m_UnitController.selectUnitList[0].GetData().maxEnergy.ToString();
            }
            else if(m_UnitController.selectUnitList.Count > 1)
            {
                objectName.gameObject.SetActive(true);
                objectHp.gameObject.SetActive(true);
                objectDamage.gameObject.SetActive(true);
                objectDefence.gameObject.SetActive(true);
            }
            else if (m_BuildingController.selectBuilding != null)
            {
                objectName.gameObject.SetActive(true);
                objectHp.gameObject.SetActive(true);
                if(m_BuildingController.selectBuilding.GetData().isAttack == true)objectDamage.gameObject.SetActive(true);
                objectDefence.gameObject.SetActive(true);

                objectName.text = m_BuildingController.selectBuilding.GetData().buildingName.ToString();
                objectHp.text = m_BuildingController.selectBuilding.nowHp.ToString() + "/" + m_BuildingController.selectBuilding.GetData().buildingMaxHp.ToString();
                objectDamage.text = "공격력 : " + m_BuildingController.selectBuilding.nowDamage.ToString();
                objectDefence.text = "방어력 : " + m_BuildingController.selectBuilding.nowDefence.ToString();
            }
            else if (m_MaterialController.selectMaterial != null)
            {
                objectName.gameObject.SetActive(true);
                materialRemain.gameObject.SetActive(true);

                objectName.text = m_MaterialController.selectMaterial.materialType.ToString();
                materialRemain.text = "남은 양 : " + m_MaterialController.selectMaterial.remainMaterial.ToString();
            }
            else if(m_EnemyController.selectEnemy != null)
            {
                objectName.gameObject.SetActive(true);
                objectHp.gameObject.SetActive(true);

                objectName.text = "적";
                objectHp.text = "체력 : " + m_EnemyController.selectEnemy.nowHp.ToString() + "/" + m_EnemyController.selectEnemy.GetData().maxHp.ToString();
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public void OnButtonInfo(string sInfo)
    {
        buttonInfomation.gameObject.SetActive(true);
        buttonInfomation.text = sInfo;
    }
    public void OffButtonInfo()
    {
        buttonInfomation.gameObject.SetActive(false);
        buttonInfomation.text = null;
    }
    private IEnumerator ShowPlayerMaterial()
    {
        while(true)
        {
            nowMineral.gameObject.SetActive(true);
            nowVespeneGas.gameObject.SetActive(true);
            Supply.gameObject.SetActive(true);

            nowMineral.text = "M  " + m_PlayerManager.GetMineral().ToString();
            nowVespeneGas.text = "V  " + m_PlayerManager.GetVespeneGas().ToString();
            Supply.text = "S  " + m_PlayerManager.GetNowSupply().ToString() + "/" + m_PlayerManager.GetMaxSupply().ToString();

            yield return new WaitForEndOfFrame();
        }
    }
}
