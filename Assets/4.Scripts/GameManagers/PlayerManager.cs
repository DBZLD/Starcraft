using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    public int mineral;
    public int bespeneGas;
    public int nowSupply;

    public void GatheringMineral()
    {
        mineral += 8;
    }
    public void GatheringBespeneGas()
    {
        bespeneGas += 8;
    }
    public PlayerData GetData()
    {
        return playerData;
    }
}
