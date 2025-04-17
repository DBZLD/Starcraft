using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatusManager : MonoBehaviour
{
    public struct SCVStatus
    {
        public int maxHp;               //최대 체력
        public int nowDamage;           //현재 공격력
        public int nowDefence;          //현재 방어력
        public float moveSpeed;         //이동 속도
        public int maxEnergy;           //최대 에너지
    }
    void Start()
    {
        
    }
}
