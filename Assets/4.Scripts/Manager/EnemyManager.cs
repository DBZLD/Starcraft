using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject Marker;
    [SerializeField] private GameObject NameText;

    public ObjectState objectState;

    public int nowHp;
    public int nowDamage;
    public int nowDefence;

    public struct EnemyBaseData
    {
        public AirGround airGround;    //적 이동형식
        public AttackType attackType;  //적 공격형식
        public AttackAirGround attackAirGround; //적 공격범위
        public ObjectSize objectSize;      //적 크기
        public ObjectType objectType;      //적 타입

        public int uiPriority;      //적 UI 우선순위
        public int maxHp;           //최대 체력
        public int baseDefense;     //기본 방어력
        public int baseDamage;      //기본 공격력
        public int upgradeDefense;  //업그레이드 당 방어력
        public int upgradeDamage;   //업그레이드 당 공격력
        public float attackSpeed;   //공격 속도
        public float attackRange;   //공격 사거리
        public float moveSpeed;     //이동 속도

        public bool isAttack;       //공격 가능 여부
    }
    public EnemyBaseData enemyBaseData = new EnemyBaseData();
    private void Start()
    {
        
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
