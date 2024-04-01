using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

using StatesDictionary = System.Collections.Generic.Dictionary<string, int>;

public class Node
{
    // ~ public interface
    public Node(Node parent, float cost, StatesDictionary state, Action action)
    {
        Parent = parent;
        Cost = cost;
        State = new StatesDictionary(state);
        Action = action;
    }

    public Node Parent;
    public float Cost;
    public StatesDictionary State;
    public Action Action;
}

public class GoalPlanner
{
    // ~ public interface
    public Queue<Action> Plan(List<Action> Actions, StatesDictionary Goal)
    {
        var AchievableActions = Actions.FindAll(Action => Action.IsAchievable());

        var Leaves = new List<Node>();
        var Start = new Node(null, 0, new StatesDictionary(), null);

        if(!BuildGraph(Start, Leaves, AchievableActions, Goal))
        {
            //Debug.Log("NO PLAN FOUND");
            return null;
        }

        var PlanList = new List<Action>();
        var LeavesMin = Leaves.Min(Leaf => Leaf.Cost);
        var CurrentNode = Leaves.FindAll(Leaf => Leaf.Cost <= LeavesMin).First();
        while(CurrentNode != null)
        {
            if(CurrentNode.Action != null)
            {
                PlanList.Add(CurrentNode.Action);
            }
            CurrentNode = CurrentNode.Parent;
        }
        PlanList.Reverse();

        //Debug.Log("PLAN FOUND");

        return new Queue<Action>(PlanList);
    }

    // ~ private interface
    private bool BuildGraph(Node Parent, List<Node> Leaves, List<Action> Actions, StatesDictionary Goal)
    {
        bool FoundPath = false;

        var AchievableActions = Actions.FindAll(Action => Action.IsAchievable(Parent.State));
        foreach(var Action in AchievableActions)
        {
            StatesDictionary CurrentState = new StatesDictionary(Parent.State);
            foreach (var Effect in Action.GetResults())
            {
                if (!CurrentState.ContainsKey(Effect.Key))
                {
                    CurrentState.Add(Effect.Key, Effect.Value);
                }
            }

            var node = new Node(Parent, Parent.Cost + Action.GetCost(), CurrentState, Action);

            if (CurrentState.Intersect(Goal).Count() == Goal.Count())
            {
                Leaves.Add(node);
                FoundPath = true;
            }
            else
            {
                List<Action> Subset = new List<Action>(Actions);
                Subset.Remove(Action);

                FoundPath = BuildGraph(node, Leaves, Subset, Goal);
            }
        }

        return FoundPath;
    }
}
