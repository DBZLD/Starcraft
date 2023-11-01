using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] Button[] button;
    private UnitController m_unitController;

    public int buttonObject;

    private void Awake()
    {
        m_unitController = GetComponent<UnitController>();

        for(int i = 0; i < button.Length; i++)
        {
            button[i].interactable = false;
        }
    }

    public void SetButtonOwner()
    {
        buttonObject = m_unitController.UIPriority();
    }
    
}
