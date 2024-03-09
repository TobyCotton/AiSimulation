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
            AddAfterEffects(EStates.Objective1, 1);

        AddAction(new Action()).
            SetTargetTag("Objective2").
            SetCost(1.0f).
            SetDuration(5.0f).
            AddPrecondition(EStates.Objective1, 1).
            AddAfterEffects(EStates.Objective2, 1);

        AddAction(new Action()).
            SetTargetTag("Objective3").
            SetCost(1.0f).
            SetDuration(15.0f).
            AddPrecondition(EStates.Objective2, 1).
            AddAfterEffects(EStates.Objective3, 1);

        SubGoal SubGoal1 = new SubGoal(EStates.Objective3, 1, true);
        Goals.Add(SubGoal1, 1);
    }
}
