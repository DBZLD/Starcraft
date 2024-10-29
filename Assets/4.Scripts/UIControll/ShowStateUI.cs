using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ShowStateUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nowMineral;
    [SerializeField] private TMP_Text nowBespeneGas;
    [SerializeField] private TMP_Text Supply;

    [SerializeField] private TMP_Text objectName;
    [SerializeField] private TMP_Text objectHp;
    [SerializeField] private TMP_Text objectDamage;
    [SerializeField] private TMP_Text objectDefence;
    [SerializeField] private TMP_Text materialRemain;

    //debug
    [SerializeField] private TMP_Text unitState;
    [SerializeField] private TMP_Text unitSpeed;

    private UnitController m_UnitController;
    private BuildingController m_BuildingController;
    private MaterialController m_MaterialController;
    private EnemyController m_EnemyController;
    private PlayerManager m_PlayerManager;
   
    private void Awake()
    {
        objectName.gameObject.SetActive(false);
        objectHp.gameObject.SetActive(false);
        objectDamage.gameObject.SetActive(false);
        objectDefence.gameObject.SetActive(false);

        materialRemain.gameObject.SetActive(false);
        nowMineral.gameObject.SetActive(false);
        nowBespeneGas.gameObject.SetActive(false);
        Supply.gameObject.SetActive(false);
        
        //debug
        unitState.gameObject.SetActive(false);
        unitSpeed.gameObject.SetActive(false);

        m_UnitController = GetComponent<UnitController>();
        m_BuildingController = GetComponent<BuildingController>();
        m_MaterialController = GetComponent<MaterialController>();
        m_EnemyController = GetComponent<EnemyController>();
        m_PlayerManager = GetComponent<PlayerManager>();
        
        StartCoroutine(ShowUnitState());
        StartCoroutine(ShowPlayerMaterial());
    }
    private IEnumerator ShowUnitState()
    {  
        while(true)
        {
            objectName.gameObject.SetActive(false);
            objectHp.gameObject.SetActive(false);
            objectDamage.gameObject.SetActive(false);
            objectDefence.gameObject.SetActive(false);

            materialRemain.gameObject.SetActive(false);
            //debug
            unitState.gameObject.SetActive(false);
            unitSpeed.gameObject.SetActive(false);

            if (m_UnitController.selectUnitList.Count == 1)
            {
                objectName.gameObject.SetActive(true);
                objectHp.gameObject.SetActive(true);
                objectDamage.gameObject.SetActive(true);
                objectDefence.gameObject.SetActive(true);
                //debug
                unitState.gameObject.SetActive(true);
                unitSpeed.gameObject.SetActive(true);

                objectName.text = m_UnitController.selectUnitList[0].GetData().unitName.ToString();
                objectHp.text = "Hp : " + m_UnitController.selectUnitList[0].nowHp.ToString() + "/" + m_UnitController.selectUnitList[0].GetData().maxHp.ToString();
                objectDamage.text = "Damage : " + m_UnitController.selectUnitList[0].nowDamage.ToString();
                objectDefence.text = "Defence : " + m_UnitController.selectUnitList[0].nowDefence.ToString();
                //debug
                unitState.text = "State : " + m_UnitController.selectUnitList[0].objectState.ToString();
                unitSpeed.text = "Speed : " + m_UnitController.selectUnitList[0].UnitSpeed.ToString();
            }
            else if(m_UnitController.selectUnitList.Count > 1)
            {
                int uiPriority = m_UnitController.UIPriority();

                objectName.gameObject.SetActive(true);
                objectHp.gameObject.SetActive(true);
                objectDamage.gameObject.SetActive(true);
                objectDefence.gameObject.SetActive(true);
                //debug
                unitState.gameObject.SetActive(true);

                objectName.text = m_UnitController.selectUnitList[uiPriority].GetData().unitName.ToString();
                objectHp.text = m_UnitController.selectUnitList[uiPriority].nowHp.ToString() + "/" + m_UnitController.selectUnitList[uiPriority].GetData().maxHp.ToString();
                objectDamage.text = "Damage : " + m_UnitController.selectUnitList[uiPriority].nowDamage.ToString();
                objectDefence.text = "Defence : " + m_UnitController.selectUnitList[uiPriority].nowDefence.ToString();
                //debug
                unitState.text = "State : " + m_UnitController.selectUnitList[uiPriority].objectState.ToString();
            }

            if(m_BuildingController.selectBuilding != null)
            {
                objectName.gameObject.SetActive(true);
                objectHp.gameObject.SetActive(true);
                objectDamage.gameObject.SetActive(true);
                objectDefence.gameObject.SetActive(true);

                objectName.text = m_BuildingController.selectBuilding.GetData().buildingName.ToString();
                objectHp.text = m_BuildingController.selectBuilding.nowHp.ToString() + "/" + m_BuildingController.selectBuilding.GetData().buildingMaxHp.ToString();
                objectName.text = "Damage : " + m_BuildingController.selectBuilding.nowDamage.ToString();
                objectName.text = "Defence : " + m_BuildingController.selectBuilding.nowDefence.ToString();
            }
            if(m_MaterialController.selectMaterial != null)
            {
                objectName.gameObject.SetActive(true);
                materialRemain.gameObject.SetActive(true);

                objectName.text = m_MaterialController.selectMaterial.materialType.ToString();
                materialRemain.text = m_MaterialController.selectMaterial.remainMaterial.ToString();
            }
            if(m_EnemyController.selectEnemy != null)
            {
                objectName.gameObject.SetActive(true);
                objectHp.gameObject.SetActive(true);
                objectDamage.gameObject.SetActive(true);
                objectDefence.gameObject.SetActive(true);

                objectDamage.text = "Damage : " + m_EnemyController.selectEnemy.nowDamage.ToString();
                objectDefence.text = "Defence : " + m_EnemyController.selectEnemy.nowDefence.ToString();
                objectName.text = "Enemy";
                objectHp.text = m_EnemyController.selectEnemy.nowHp.ToString() + "/" + m_EnemyController.selectEnemy.GetData().maxHp.ToString();
            }
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator ShowPlayerMaterial()
    {
        while(true)
        {
            nowMineral.gameObject.SetActive(true);
            nowBespeneGas.gameObject.SetActive(true);
            Supply.gameObject.SetActive(true);

            nowMineral.text = m_PlayerManager.GetMineral().ToString();
            nowBespeneGas.text = m_PlayerManager.GetBespeneGas().ToString();
            Supply.text = m_PlayerManager.GetNowSupply().ToString() + "/" + m_PlayerManager.GetMaxSupply().ToString();

            yield return new WaitForEndOfFrame();
        }
    }
}
