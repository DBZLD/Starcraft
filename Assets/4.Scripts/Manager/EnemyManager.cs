using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header ("System")]
    [SerializeField] private GameObject Marker;
    [SerializeField] private GameObject NameText;

    [Header("State")]
    public ObjectState objectState;
    public bool CanAttack;

    [Header ("Status")]
    public int nowHp;
    public int nowDamage;
    public int nowDefence;

    [Serializable]
    public struct EnemyBaseData
    {   
        public AirGround airGround;                 //적 이동형식
        public AttackType attackType;               //적 공격형식
        public AttackAirGround attackAirGround;     //적 공격범위
        public ObjectSize objectSize;               //적 크기
        public ObjectType objectType;               //적 타입

        public int uiPriority;                      //적 UI 우선순위
        public int maxHp;                           //최대 체력
        public int baseDefense;                     //기본 방어력
        public int baseDamage;                      //기본 공격력
        public int upgradeDefense;                  //업그레이드 당 방어력
        public int upgradeDamage;                   //업그레이드 당 공격력
        public float attackSpeed;                   //공격 속도
        public float attackRange;                   //공격 사거리

         public bool isAttack;                      //공격 가능 여부
    }
     public EnemyBaseData enemyBaseData = new EnemyBaseData();
    private void Awake()
    {
        Marker.transform.localScale = new Vector3(1.3f, 1.3f, 1);
        Marker.transform.localPosition = new Vector3(0, -transform.lossyScale.y / 2 + 0.01f, 0);
        Marker.SetActive(false);

        NameText.transform.localPosition = new Vector3(0, transform.lossyScale.y / 2, 0);
        NameText.transform.rotation = Quaternion.Euler(90, 0, 0);

        SetHp(enemyBaseData.maxHp);
        SetDamage(enemyBaseData.baseDamage);
        SetDefence(enemyBaseData.baseDefense);

        CanAttack = true;
    }
    public void TakeDamage(int nowDamage, AttackType attackType)
    {
        nowHp -= Mathf.RoundToInt((nowDamage - nowDefence) * AttackTypeUnitSize(attackType, enemyBaseData.objectSize));
    } // (기본공격력 + 업그레이드 공격력*업그레이드 횟수 ) - ( 쉴드 잔량 + 쉴드 총 방어력 ) - 총 방어력 } * 공격/방어 방식에 따른 비율 
    public float AttackTypeUnitSize(AttackType attackType, ObjectSize unitSize)
    {

        if (unitSize == ObjectSize.Small)
        {
            if (attackType == AttackType.Normal) { return 1f; }
            else if (attackType == AttackType.Explosive) { return 0.5f; }
            else if (attackType == AttackType.Concussive) { return 1f; }
            else if (attackType == AttackType.Spell) { return 1f; }
        }
        else if (unitSize == ObjectSize.Medium)
        {
            if (attackType == AttackType.Normal) { return 1f; }
            else if (attackType == AttackType.Explosive) { return 0.75f; }
            else if (attackType == AttackType.Concussive) { return 0.5f; }
            else if (attackType == AttackType.Spell) { return 1f; }
        }
        else if (unitSize == ObjectSize.Large)
        {
            if (attackType == AttackType.Normal) { return 1f; }
            else if (attackType == AttackType.Explosive) { return 1f; }
            else if (attackType == AttackType.Concussive) { return 0.25f; }
            else if (attackType == AttackType.Spell) { return 1f; }
        }
        return 0f;
    }
    public void MarkedEnemy()
    {
        Marker.SetActive(true);
    }

    public void UnMarkedEmemy()
    {
        Marker.SetActive(false);
    }

    public void SetHp(int hp)
    {
        nowHp = hp;
    }
    public void SetDamage(int damage)
    {
        nowDamage = damage;

    }
    public void SetDefence(int defence)
    {
        nowDefence = defence;

    }
    public EnemyBaseData GetData() { return enemyBaseData; }
}
