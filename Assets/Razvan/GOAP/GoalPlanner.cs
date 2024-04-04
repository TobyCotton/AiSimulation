using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

using StatesSet = System.Collections.Generic.HashSet<string>;

public class Node
{
    // ~ public interface
    public Node(Node parent, float cost, StatesSet state, Action action)
    {
        Parent = parent;
        Cost = cost;
        State = new StatesSet(state);
        Action = action;
    }

    public Node Parent;
    public float Cost;
    public StatesSet State;
    public Action Action;
}

public class GoalPlanner
{
    // ~ public interface
    public Queue<Action> Plan(List<Action> Actions, StatesSet Goal)
    {
        var AchievableActions = Actions.FindAll(Action => Action.IsAchievable());

        var Leaves = new List<Node>();
        var Start = new Node(null, 0, new StatesSet(), null);

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
    private bool BuildGraph(Node Parent, List<Node> Leaves, List<Action> Actions, StatesSet Goal)
    {
        bool FoundPath = false;

        var AchievableActions = Actions.FindAll(Action => Action.IsAchievable(Parent.State));
        foreach(var Action in AchievableActions)
        {
            StatesSet CurrentState = new StatesSet(Parent.State);
            foreach (var Effect in Action.GetResults())
            {
                if (!CurrentState.Contains(Effect))
                {
                    CurrentState.Add(Effect);
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
