using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using UnityEngine;
using Zenject;

public class Testing : MonoBehaviour
{
    public float TurnLength;
    
    private float _currentTurnLength;
    
    [Inject]
    private BattleTurnsManager _battleTurnsManager;
    
    
    
    void Update()
    {
        // _currentTurnLength += Time.deltaTime;
        //
        // if (_currentTurnLength >= TurnLength)
        // {
        //     _battleTurnsManager.NextTurn();
        //     _currentTurnLength = 0;
        // }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _battleTurnsManager.NextTurn();
        }
    }
}
