using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StatesDictionary = System.Collections.Generic.Dictionary<EStates, int>;

public sealed class Action
{
    // ~ public interface
    public bool Executing = false;

    public Action()
    {
        PrePerformEffects = new List<Effects>();
        PostPerformEffects = new List<Effects>();
        Preconditions = new StatesDictionary();
        Results = new StatesDictionary();
    }

    public bool IsAchievable(StatesDictionary conditions = null)
    {
        if (conditions == null)
        {
            return true;
        }

        foreach (var precondition in Preconditions)
        {
            if (!conditions.ContainsKey(precondition.Key))
            {
                return false;
            }
        }

        return true;
    }

    public Action SetCost(float cost)
    {
        Cost = cost;
        return this;
    }

    public Action SetTargetTag(string targetTag)
    {
        TargetTag = targetTag;
        return this;
    }

    public Action SetDuration(float duration)
    {
        Duration = duration;
        return this;
    }

    public Action AddPrePerformEffect(Effects Effect)
    {
        PrePerformEffects.Add(Effect);
        return this;
    }

    public Action AddPostPerformEffect(Effects Effect)
    {
        PostPerformEffects.Add(Effect);
        return this;
    }

    public Action AddPrecondition(EStates Key, int Value)
    {
        Preconditions.Add(Key, Value);
        return this;
    }

    public Action AddResults(EStates Key, int Value)
    {
        Results.Add(Key, Value);
        return this;
    }

    public bool PrePerform()
    {
        bool result = true;
        foreach(var Effect in PrePerformEffects)
        {
            result = result && Effect.Perform();
        }
        return result;
    }

    public bool PostPerform()
    {
        bool result = true;
        foreach (var Effect in PostPerformEffects)
        {
            result = result && Effect.Perform();
        }
        return result;
    }

    public bool CanFindTarget()
    {
        return TargetTag == "";
    }

    public StatesDictionary GetResults()
    {
        return Results;
    }

    public string GetTargetTag()
    {
        return TargetTag;
    }

    public float GetCost()
    {
        return Cost;
    }

    public float GetDuration()
    {
        return Duration;
    }

    // ~ private interface
    private string TargetTag;
    private float Cost = 1.0f;
    private float Duration = 0.0f;

    private List<Effects> PrePerformEffects;
    private List<Effects> PostPerformEffects;

    private StatesDictionary Preconditions;
    private StatesDictionary Results;
}
