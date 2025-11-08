using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using UnityEngine;
using Zenject;

public class Testing : MonoBehaviour
{
    [Inject]
    private BattleTurnsManager _battleTurnsManager;
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _battleTurnsManager.NextTurn();
        }
    }
}
