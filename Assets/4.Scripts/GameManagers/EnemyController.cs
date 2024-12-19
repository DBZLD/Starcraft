using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    public EnemyManager selectEnemy;
    [SerializeField]
    public List<EnemyManager> allEnemyList;

    public void ClickSelectEnemy(EnemyManager NewEnemy)
    {
        SelectEnemy(NewEnemy);
    }
    private void SelectEnemy(EnemyManager NewEnemy)
    {
        NewEnemy.MarkedEnemy();

        selectEnemy = NewEnemy;
    }
    public void UnselectEnemy()
    {
        selectEnemy.UnMarkedEmemy();

        selectEnemy = null;
    }
    public void AddUnitList(EnemyManager NewEnemy)
    {
        allEnemyList.Add(NewEnemy);
    }

}
