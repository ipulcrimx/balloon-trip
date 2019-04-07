using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooler : MonoBehaviour
{
    public Transform enemyParent;
    public GameObject enemyClone;
    public Transform[] poolPositions;

    private GameManager _gameManager;

    #region Instances
    private static EnemyPooler _instance = null;
    public static EnemyPooler instance
    {
        get
        {
            if (!_instance)
            {
                Debug.LogWarning("Enemy pooler hasn't initiated yet...");
            }

            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        _instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.instance;
    }

    public void InitEnemy(int totalEnemy, float min, float max)
    {
        if(totalEnemy < _gameManager.enemies.Count)
        {
            return;
        }

        while(_gameManager.enemies.Count <= totalEnemy)
        {
            int rnd = Random.Range(0, poolPositions.Length);
            GameObject temp = Instantiate(enemyClone, poolPositions[rnd]);
            Alien alien = temp.GetComponent<Alien>();

            temp.transform.SetParent(enemyParent);
            temp.transform.localScale = Vector3.one;

            alien.moveSpeed = Random.Range(min, max);
            alien.isFacingRight = Random.Range(0, 100) % 2 == 0;

            _gameManager.enemies.Add(alien);
        }
    }
}
