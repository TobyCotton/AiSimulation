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
            AddResults(EStates.Objective1, 1).
            AddAdditionalPreEffect(new PrintEffect("Objective1 PreEffect")).
            AddAdditionalPreEffect(new AsyncPrintEffect("Objective1 AsyncPreEffect")).
            AddAdditionalPostEffect(new PrintEffect("Objective1 PostEffect"));

        AddAction(new Action()).
            SetTargetTag("Objective2").
            SetCost(1.0f).
            SetDuration(5.0f).
            AddPrecondition(EStates.Objective1, 1).
            AddResults(EStates.Objective2, 1).
            AddAdditionalPreEffect(new PrintEffect("Objective2 PreEffect")).
            AddAdditionalPostEffect(new PrintEffect("Objective2 PostEffect"));

        AddAction(new Action()).
            SetTargetTag("Objective3").
            SetCost(1.0f).
            SetDuration(15.0f).
            AddPrecondition(EStates.Objective2, 1).
            AddResults(EStates.Objective3, 1).
            AddAdditionalPreEffect(new PrintEffect("Objective3 PreEffect")).
            AddAdditionalPostEffect(new PrintEffect("Objective3 PostEffect"));

        AddGoal(new SubGoal(EStates.Objective3, 1, true), 1);
    }
}
