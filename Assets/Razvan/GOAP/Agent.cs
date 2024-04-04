using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using StatesSet = System.Collections.Generic.HashSet<string>;

public class SubGoal
{
    // ~ public interface
    public SubGoal(string subGoal, bool persistency)
    {
        SubGoals = new StatesSet
        {
            subGoal
        };

        Persistency = persistency;
    }

    public StatesSet SubGoals;
    public bool Persistency;
}

public abstract class Agent : MonoBehaviour
{
    // ~ public interface
    public Agent()
    {
        Actions = new List<Action>();
        Goals = new Dictionary<SubGoal, int>();
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
    protected Action AddAction(Action Action)
    {
        Actions.Add(Action);
        return Action;
    }

    protected void AddGoal(SubGoal Goal, int Priority)
    {
        Goals.Add(Goal, Priority);
    }

    // ~ private interface
    private void LateUpdate()
    {
        if (CurrentAction != null)
        {
            switch (CurrentAction.Progress)
            {
                case EActionProgress.NotStarted:
                    if (CurrentAction.AssertAdditionalChecks())
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

            var SortedGoals = Goals.OrderByDescending(Goal => Goal.Value);
            foreach (var Goal in SortedGoals)
            {
                ActionQueue = Planner.Plan(Actions, Goal.Key.SubGoals);

                if (ActionQueue != null)
                {
                    CurrentGoal = Goal.Key;
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

    private Dictionary<SubGoal, int> Goals;
    private List<Action> Actions;
    private Queue<Action> ActionQueue;
    private Action CurrentAction;

    private SubGoal CurrentGoal;

    private GoalPlanner Planner;

    private bool invoked = false;
}
