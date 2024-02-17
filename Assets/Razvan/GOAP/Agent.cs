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
    public void Start()
    {
        Actions = GetComponents<Action>().ToList<Action>();
        MovementComponent = GetComponent<AI_Movement>();
    }

    public void NotifyReachedGoal()
    {
        if (!invoked)
        {
            Invoke("CompleteAction", CurrentAction.Duration);
            invoked = true;
        }
    }

    // ~ protected interface
    protected Dictionary<SubGoal, int> Goals = new Dictionary<SubGoal, int>();

    // ~ private interface
    private void LateUpdate()
    {
        if (CurrentAction != null && CurrentAction.Executing)
        {
            return;
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
            if (CurrentAction.PrePerform())
            {
                CurrentAction.Executing = true;

                MovementComponent.moveTo(CurrentAction.TargetTag);
            }
            else
            {
                ActionQueue = null;
            }
        }
    }

    private void CompleteAction()
    {
        CurrentAction.Executing = false;
        CurrentAction.PostPerform();
        invoked = false;
    }

    private AI_Movement MovementComponent;

    private List<Action> Actions;
    private Queue<Action> ActionQueue;
    private Action CurrentAction;

    private SubGoal CurrentGoal;

    private GoalPlanner Planner;

    private bool invoked = false;
}
