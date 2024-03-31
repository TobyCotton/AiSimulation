using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using StatesDictionary = System.Collections.Generic.Dictionary<EStates, int>;

public class SubGoal
{
    // ~ public interface
    public StatesDictionary SubGoals;
    public bool Persistency = true;

    public SubGoal(EStates key, int value, bool persistency)
    {
        SubGoals = new StatesDictionary();
        SubGoals.Add(key, value);

        Persistency = persistency;
    }
}

public abstract class Agent : MonoBehaviour
{
    // ~ public interface
    public Agent()
    {
        Actions = new List<Action>();
    }

    public void Start()
    {    
        MovementComponent = GetComponent<AI_Movement>();
    }

    public void NotifyReachedGoal()
    {
        if (!invoked)
        {
            Invoke("CompleteAction", CurrentAction.GetDuration());
            invoked = true;
        }
    }

    // ~ protected interface
    protected Action AddAction(Action action)
    {
        Actions.Add(action);
        return action;
    }

    protected void AddGoal(SubGoal Key, int Value)
    {
        Goals.Add(Key, Value);
    }

    // ~ private interface
    private void LateUpdate()
    {
        if (CurrentAction != null)
        {
            switch (CurrentAction.Progress)
            {
                case EActionProgress.NotStarted:
                    if (CurrentAction.AssertAditionalChecks())
                    {
                        CurrentAction.Progress = EActionProgress.ExecutingPrePerform;
                    }
                    else
                    {
                        CurrentAction.Progress = EActionProgress.NotStarted;
                        CurrentAction = null;
                    }
                    return;
                case EActionProgress.ExecutingPrePerform:
                    if (CurrentAction.LateUpdatePrePerformResult())
                    {
                        CurrentAction.ResetPrePerformEffects();
                        CurrentAction.Progress = EActionProgress.ExecutingMovement;
                        MovementComponent.moveTo(CurrentAction.GetTargetTag());
                    }
                    return;
                case EActionProgress.ExecutingMovement:
                    // Wait for NotifyReachedGoal.
                    return;
                case EActionProgress.ExecutingPostPerform:
                    if (CurrentAction.LateUpdatePostPerformResult())
                    {
                        CurrentAction.ResetPostPerformEffects();
                        CurrentAction.Progress = EActionProgress.Finished;
                        CurrentAction = null;
                    }
                    return;
            }
        }

        if (Planner == null || ActionQueue == null)
        {
            Planner = new GoalPlanner();

            var SortedGoals = from goal in Goals orderby goal.Value descending select goal;

            foreach (var goal in SortedGoals)
            {
                ActionQueue = Planner.Plan(Actions, goal.Key.SubGoals, null);

                if (ActionQueue != null)
                {
                    CurrentGoal = goal.Key;
                    break;
                }
            }
        }

        if (ActionQueue != null && ActionQueue.Count() == 0)
        {
            if (!CurrentGoal.Persistency)
            {
                Goals.Remove(CurrentGoal);
            }
            Planner = null;
        }

        if (ActionQueue != null && ActionQueue.Count() > 0)
        {
            CurrentAction = ActionQueue.Dequeue();
            CurrentAction.Progress = EActionProgress.NotStarted;
        }
    }

    private void CompleteAction()
    {
        CurrentAction.Progress = EActionProgress.ExecutingPostPerform;
        invoked = false;
    }

    private AI_Movement MovementComponent;

    private Dictionary<SubGoal, int> Goals = new Dictionary<SubGoal, int>();
    private List<Action> Actions;
    private Queue<Action> ActionQueue;
    private Action CurrentAction;

    private SubGoal CurrentGoal;

    private GoalPlanner Planner;

    private bool invoked = false;
}
