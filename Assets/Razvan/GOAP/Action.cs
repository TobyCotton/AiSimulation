using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StatesSet = System.Collections.Generic.HashSet<string>;

/*
    An enum defining the steps of processing an action.
*/
public enum EActionProgress
{
    NotStarted,
    ExecutingPrePerform,
    ExecutingMovement,
    ExecutingPostPerform,
    Finished,
}

/*
    A class used for building custom actions for the AI agents.
    Each action must have:
        - a target tag
        - a cost
        - a duration
    Each action could have:
        - pre conditions
        - additional checks
        - additional pre effects
        - results
        - additional post effects
*/
public sealed class Action
{
    // ~ public interface
    public EActionProgress Progress = EActionProgress.NotStarted;

    public Action()
    {
        AdditionalChecks = new List<AdditionalCheck>();
        AditionalPreEffects = new List<AdditionalEffect>();
        AditionalPostEffects = new List<AdditionalEffect>();
        Preconditions = new StatesSet();
        Results = new StatesSet();
    }

    /*
        A function for checking if the action is achievable.
    */
    public bool IsAchievable(StatesSet Conditions = null)
    {
        // First assert the additional checks.
        if(!AssertAdditionalChecks())
        { 
            return false;
        }

        // If no states are provided, ignore the preconditions.
        if (Conditions == null)
        {
            return true;
        }

        // If states are provided, check if the preconditions are met.
        foreach (var Precondition in Preconditions)
        {
            if (!Conditions.Contains(Precondition))
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

    public Action AddAdditionalCheck(AdditionalCheck Check)
    {
        AdditionalChecks.Add(Check);
        return this;
    }

    public Action AddAdditionalPreEffect(AdditionalEffect Effect)
    {
        AditionalPreEffects.Add(Effect);
        return this;
    }

    public Action AddAdditionalPostEffect(AdditionalEffect Effect)
    {
        AditionalPostEffects.Add(Effect);
        return this;
    }

    public Action AddPrecondition(string Key)
    {
        Preconditions.Add(Key);
        return this;
    }

    public Action AddResults(string Key)
    {
        Results.Add(Key);
        return this;
    }

    /*
        A function returning the aggregate result of
        the additional checks.
    */
    public bool AssertAdditionalChecks()
    {
        bool Result = true;
        foreach(var Check in AdditionalChecks)
        {
            Result = Result && Check.Assert();
        }
        return Result;
    }

    /*
        A function running and returning the aggregate result of
        the additional pre effects.
    */
    public bool LateUpdatePrePerformResult()
    {
        bool Result = true;
        foreach(var Effect in AditionalPreEffects)
        {
            // Only run the failed effects.
            if (Effect.GetResult() == EAdditionalEffectResult.Fail)
            {
                Effect.LateUpdate();
            }

            Result = Result && (Effect.GetResult() != EAdditionalEffectResult.Fail);
        }
        return Result;
    }

    /*
        A function for resetting the results of the pre effects.
    */
    public void ResetPrePerformEffects()
    {
        foreach(var Effect in AditionalPreEffects)
        {
            Effect.ResertResult();
        }
    }

    /*
        A function running and returning the aggregate result of
        the additional post effects.
    */
    public bool LateUpdatePostPerformResult()
    {
        bool Result = true;
        foreach(var Effect in AditionalPostEffects)
        {
            // Only run the failed effects.
            if (Effect.GetResult() == EAdditionalEffectResult.Fail)
            {
                Effect.LateUpdate();
            }

            Result = Result && (Effect.GetResult() != EAdditionalEffectResult.Fail);
        }
        return Result;
    }

    /*
        A function for resetting the results of the post effects.
    */
    public void ResetPostPerformEffects()
    {
        foreach (var Effect in AditionalPostEffects)
        {
            Effect.ResertResult();
        }
    }

    public StatesSet GetResults()
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

    private List<AdditionalCheck> AdditionalChecks;
    private List<AdditionalEffect> AditionalPreEffects;
    private List<AdditionalEffect> AditionalPostEffects;

    private StatesSet Preconditions;
    private StatesSet Results;
}
