using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_Worker : Agent
{
    new void Start()
    {
        base.Start();
        SubGoal SubGoal1 = new SubGoal(EStates.Objective3, 1, true);
        Goals.Add(SubGoal1, 1);
    }
}
