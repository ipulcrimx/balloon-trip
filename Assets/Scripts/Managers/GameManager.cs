using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Enemy[] enemies;

    private bool _hasGameOver = false;
    private int _live;

    private void Awake()
    {
        _live = Constant.startingLive;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
