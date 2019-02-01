﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public PausePopUpManager pausePopUp;
    public PostGamePopUpManager postGamePopUp;

    // Start is called before the first frame update
    void Start()
    {
        // Lazy init...
        pausePopUp.Init();
        postGamePopUp.Init();
    }
}
