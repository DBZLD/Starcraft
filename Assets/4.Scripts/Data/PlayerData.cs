using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScirptableObjects/PlayerData", order = 1)]

public class PlayerData : ScriptableObject
{
    public int mineral;
    public int bespeneGas;
    public int maxSupply;
    public int nowSupply;


}
