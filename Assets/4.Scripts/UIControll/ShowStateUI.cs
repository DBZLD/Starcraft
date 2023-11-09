using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowStateUI : MonoBehaviour
{
    [SerializeField] TMP_Text unitName;
    [SerializeField] TMP_Text unitHp;
    [SerializeField] TMP_Text unitState;

    private void Awake()
    {
        unitName.gameObject.SetActive(false);
        unitHp.gameObject.SetActive(false);
        unitState.gameObject.SetActive(false);
    }

    public void ShowUnitStateUI(List<UnitManager> unitList)
    {
        Debug.Log(unitList.Count);
        if (unitList.Count == 1)
        {
            unitName.gameObject.SetActive(true);
            unitHp.gameObject.SetActive(true);
            unitState.gameObject.SetActive(true);

            StartCoroutine(UpdateStateUI(unitList));
        }
        else
        {
            unitName.gameObject.SetActive(false);
            unitHp.gameObject.SetActive(false);
            unitState.gameObject.SetActive(false);
        }
        
    }
    IEnumerator UpdateStateUI(List<UnitManager> unitList)
    {
        while (unitList.Count == 1)
        {
            unitName.text = unitList[0].GetComponent<UnitBaseData>().unitName.ToString();
            unitHp.text = unitList[0].GetComponent<UnitBaseData>().maxHp.ToString();
            unitState.text = unitList[0].unitState.ToString();

            yield return new WaitForSecondsRealtime(0.5f);
        }
        yield break;
    }
}
