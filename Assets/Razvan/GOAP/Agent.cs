using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using StatesSet = System.Collections.Generic.HashSet<string>;

/*
    A base class used for creating custom AI agents.
    Override Start() in order to set the actions and goals.
*/
public abstract class Agent : MonoBehaviour
{
    // ~ public interface
    public Agent()
    {
        Actions = new List<Action>();
        Goals = new Dictionary<Goal, int>();
    }

    public void Start()
    {    
        MovementComponent = GetComponent<AI_Movement>();
    }

    /*
        A function called by external navigation systems to notify
        the agent that it reached its goal.
    */
    public void NotifyReachedGoal()
    {
        // Guard against spam calls.
        if (!invoked)
        {
            Invoke("CompleteWait", CurrentAction.GetDuration());
            invoked = true;
        }
    }

    // ~ protected interface
    protected Action AddAction(Action Action)
    {
        Actions.Add(Action);
        return Action;
    }

    protected void AddGoal(Goal Goal, int Priority)
    {
        Goals.Add(Goal, Priority);
    }

    /*
        A class defining an Agent goal.
    */
    protected class Goal
    {
        // ~ public interface
        public Goal(StatesSet subGoals, bool persistency)
        {
            SubGoals = subGoals;
            Persistency = persistency;
        }

        public StatesSet SubGoals;
        public bool Persistency;
    }

    // ~ private interface

    /*
        The main function driving the Agent's behaviour.
        It builds the plan based on the highest priority goals
        and executes the related actions' logic.
    */
    private void LateUpdate()
    {
        // If the current action is valid, proceed to run the current step.
        if (CurrentAction != null)
        {
            switch (CurrentAction.Progress)
            {
                case EActionProgress.NotStarted:
                    // Assert the additional checks.
                    if (CurrentAction.AssertAdditionalChecks())
                    {
                        // Advance to next step.
                        CurrentAction.Progress = EActionProgress.ExecutingPrePerform;
                    }
                    else
                    {
                        // In case they fail, abort the action.
                        CurrentAction.Progress = EActionProgress.NotStarted;
                        CurrentAction = null;
                    }
                    return;
                case EActionProgress.ExecutingPrePerform:
                    // Run the pre perform effects until they all succeed.
                    if (CurrentAction.LateUpdatePrePerformResult())
                    {
                        // Reset their state for next iteration.
                        CurrentAction.ResetPrePerformEffects();

                        // Advance to next step.
                        CurrentAction.Progress = EActionProgress.ExecutingMovement;

                        // Initiate the movement.
                        MovementComponent.moveTo(CurrentAction.GetTargetTag());
                    }
                    return;
                case EActionProgress.ExecutingMovement:
                    // Wait for NotifyReachedGoal.
                    return;
                case EActionProgress.ExecutingPostPerform:
                    // Run the post perform effects until they all succeed.
                    if (CurrentAction.LateUpdatePostPerformResult())
                    {
                        // Reset their state for next iteration.
                        CurrentAction.ResetPostPerformEffects();

                        // Finish the current action.
                        CurrentAction.Progress = EActionProgress.Finished;
                        CurrentAction = null;
                    }
                    return;
            }
        }

        // If both the planner and action queue are invalid, attempt to start a new plan. 
        if (Planner == null || ActionQueue == null)
        {
            Planner = new GoalPlanner();

            // Sort the goals based on their priority. (high priority comes first)
            var SortedGoals = Goals.OrderByDescending(Goal => Goal.Value);
            // Find the highest priority goal which is also achievable.
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

        // If an empty action queue was found, remove its goal unless it is persistent.
        if (ActionQueue != null && ActionQueue.Count() == 0)
        {
            if (!CurrentGoal.Persistency)
            {
                Goals.Remove(CurrentGoal);
            }
            Planner = null;
        }

        // If the action queue still has elements, pull the new current action from it.
        if (ActionQueue != null && ActionQueue.Count() > 0)
        {
            CurrentAction = ActionQueue.Dequeue();
            CurrentAction.Progress = EActionProgress.NotStarted;
        }
    }

    /*
        The function invoked by NotifyReachedGoal.
    */
    private void CompleteWait()
    {
        CurrentAction.Progress = EActionProgress.ExecutingPostPerform;
        invoked = false;
    }

    private AI_Movement MovementComponent;

    private Dictionary<Goal, int> Goals;
    private List<Action> Actions;
    private Queue<Action> ActionQueue;
    private Action CurrentAction;

    private Goal CurrentGoal;

    private GoalPlanner Planner;

    private bool invoked = false;
}
