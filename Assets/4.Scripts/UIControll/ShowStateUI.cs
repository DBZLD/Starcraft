using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowStateUI : MonoBehaviour
{
    [SerializeField] private TMP_Text unitName;
    [SerializeField] private TMP_Text unitHp;
    [SerializeField] private TMP_Text unitState;

    private UnitController m_UnitController;

    private void Awake()
    {
        unitName.gameObject.SetActive(false);
        unitHp.gameObject.SetActive(false);
        unitState.gameObject.SetActive(false);

        m_UnitController = GetComponent<UnitController>();

        StartCoroutine(ShowUnitState(m_UnitController.SelectUnitList));
    }
    private IEnumerator ShowUnitState(List<UnitManager> unitList)
    {
        Debug.Log("count" + unitList.Count);
        
        while(true)
        {
            if (unitList.Count == 1)
            {
                GameObject selectedUnit = unitList[0].gameObject;
                unitName.gameObject.SetActive(true);
                unitHp.gameObject.SetActive(true);
                unitState.gameObject.SetActive(true);

                unitName.text = unitList[0].GetData().unitName.ToString();
                unitHp.text = unitList[0].GetData().nowHp.ToString() + "/" + unitList[0].GetData().maxHp.ToString();
                unitState.text = unitList[0].unitState.ToString();
            }
            else
            {
                unitName.gameObject.SetActive(false);
                unitHp.gameObject.SetActive(false);
                unitState.gameObject.SetActive(false);
            }
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}
