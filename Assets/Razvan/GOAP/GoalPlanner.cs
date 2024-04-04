using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

using StatesSet = System.Collections.Generic.HashSet<string>;

public class GoalPlanner
{
    // ~ public interface

    /*
        A function which returns the cheapest queue of actions
        leading to the Goal.
    */
    public Queue<Action> Plan(List<Action> Actions, StatesSet Goal)
    {
        // Filter all the achievable actions.
        var AchievableActions = Actions.FindAll(Action => Action.IsAchievable());

        var Leaves = new List<Node>();
        var Start = new Node(null, 0, new StatesSet(), null);

        // Attempt to build a tree.
        if(!BuildTree(Start, Leaves, AchievableActions, Goal))
        {
            // If no tree was built, then it means the Goal is NOT achievable.
            return null;
        }

        // Find the cheapest leaf and its respective node.
        var PlanList = new List<Action>();
        var LeavesMin = Leaves.Min(Leaf => Leaf.Cost);
        var CurrentNode = Leaves.FindAll(Leaf => Leaf.Cost <= LeavesMin).First();

        // Traverse the tree from the cheapest leaf up to the root
        // in order to build the action queue.
        while(CurrentNode != null)
        {
            if(CurrentNode.Action != null)
            {
                PlanList.Add(CurrentNode.Action);
            }
            CurrentNode = CurrentNode.Parent;
        }
        PlanList.Reverse();

        return new Queue<Action>(PlanList);
    }

    // ~ private interface

    /*
        A recursive function which builds a tree of actions
        leading to the Goal.
    */
    private bool BuildTree(Node Parent, List<Node> Leaves, List<Action> Actions, StatesSet Goal)
    {
        // Assume the path has not been found yet.
        bool FoundPath = false;

        // Filter all the achievable actions.
        var AchievableActions = Actions.FindAll(Action => Action.IsAchievable(Parent.State));
        foreach(var Action in AchievableActions)
        {
            // Store the overall states from the current action and the parent node.
            StatesSet CurrentState = new StatesSet(Parent.State);
            foreach (var Effect in Action.GetResults())
            {
                CurrentState.Add(Effect);
            }

            // Create a node based on the overall states and cost.
            var node = new Node(Parent, Parent.Cost + Action.GetCost(), CurrentState, Action);

            // Check if the overall states match the goal.
            if (CurrentState.Intersect(Goal).Count() == Goal.Count())
            {
                // If so, add the node as a leaf.
                Leaves.Add(node);
                FoundPath = true;
            }
            else
            {
                // Otherwise, continue building the tree.
                List<Action> Subset = new List<Action>(Actions);
                Subset.Remove(Action);

                FoundPath = BuildTree(node, Leaves, Subset, Goal);
            }
        }

        return FoundPath;
    }

    /*
        The Node class used to build the weighted plan.
    */
    private class Node
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
}
