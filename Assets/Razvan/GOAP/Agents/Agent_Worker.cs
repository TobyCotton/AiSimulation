using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Agent_Worker : Agent
{
    new void Start()
    {
        base.Start();

        AddAction(new Action()).
            SetTargetTag("Objective1").
            SetCost(1.0f).
            SetDuration(5.0f).
            AddResults(EStates.Objective1, 1);

        AddAction(new Action()).
            SetTargetTag("Objective2").
            SetCost(1.0f).
            SetDuration(5.0f).
            AddPrecondition(EStates.Objective1, 1).
            AddResults(EStates.Objective2, 1);

        AddAction(new Action()).
            SetTargetTag("Objective3").
            SetCost(1.0f).
            SetDuration(15.0f).
            AddPrecondition(EStates.Objective2, 1).
            AddResults(EStates.Objective3, 1);

        AddGoal(new SubGoal(EStates.Objective3, 1, true), 1);
    }
}
