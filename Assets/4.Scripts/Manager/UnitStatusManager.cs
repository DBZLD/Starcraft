using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatusManager : MonoBehaviour
{
    public struct SCVStatus
    {
        public int maxHp;               //�ִ� ü��
        public int nowDamage;           //���� ���ݷ�
        public int nowDefence;          //���� ����
        public float moveSpeed;         //�̵� �ӵ�
        public int maxEnergy;           //�ִ� ������
    }
    void Start()
    {
        
    }
}
