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
        List<Action> AchievableActions = Actions.FindAll(action => action.IsAchievable());

        List<Node> Leaves = new List<Node>();
        Node Start = new Node(null, 0, new StatesDictionary(), null);

        bool Success = BuildGraph(Start, Leaves, AchievableActions, Goal);

        if(!Success)
        {
            //Debug.Log("NO PLAN FOUND");
            return null;
        }

        Node Cheapest = Leaves[0];
        foreach(var leaf in Leaves)
        {
            if(leaf.Cost < Cheapest.Cost)
            {
                Cheapest = leaf;
            }
        }

        List<Action> Result = new List<Action>();
        Node N = Cheapest;
        while(N != null)
        {
            if(N.Action != null)
            {
                Result.Insert(0, N.Action);
            }
            N = N.Parent;
        }

        Queue<Action> Queue = new Queue<Action>();
        foreach(var action in Result)
        {
            Queue.Enqueue(action);
        }

        //Debug.Log("PLAN FOUND");

        return Queue;
    }

    // ~ private interface
    private bool BuildGraph(Node Parent, List<Node> Leaves, List<Action> Actions, StatesDictionary Goal)
    {
        bool FoundPath = false;

        foreach(var action in Actions)
        {
            if(action.IsAchievable(Parent.State))
            {
                StatesDictionary CurrentState = new StatesDictionary(Parent.State);
                foreach(var effect in action.GetResults())
                {
                    if (!CurrentState.ContainsKey(effect.Key))
                    {
                        CurrentState.Add(effect.Key, effect.Value);
                    }
                }

                Node node = new Node(Parent, Parent.Cost + action.GetCost(), CurrentState, action);

                if(CurrentState.Intersect(Goal).Count() == Goal.Count())
                {
                    Leaves.Add(node);
                    FoundPath = true;
                }
                else
                {
                    List<Action> Subset = new List<Action>(Actions);
                    Subset.Remove(action);

                    FoundPath = BuildGraph(node, Leaves, Subset, Goal);
                }
            }
        }

        return FoundPath;
    }
}
