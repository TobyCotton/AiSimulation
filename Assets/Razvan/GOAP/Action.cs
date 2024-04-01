using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StatesDictionary = System.Collections.Generic.Dictionary<string, int>;

public enum EActionProgress
{
    NotStarted,
    ExecutingPrePerform,
    ExecutingMovement,
    ExecutingPostPerform,
    Finished,
}

public sealed class Action
{
    // ~ public interface
    public EActionProgress Progress = EActionProgress.NotStarted;

    public Action()
    {
        AdditionalChecks = new List<AdditionalCheck>();
        AditionalPreEffects = new List<AdditionalEffect>();
        AditionalPostEffects = new List<AdditionalEffect>();
        Preconditions = new StatesDictionary();
        Results = new StatesDictionary();
    }

    public bool IsAchievable(StatesDictionary conditions = null)
    {
        if(!AssertAditionalChecks())
        { 
            return false;
        }

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

    public Action AddPrecondition(string Key, int Value)
    {
        Preconditions.Add(Key, Value);
        return this;
    }

    public Action AddResults(string Key, int Value)
    {
        Results.Add(Key, Value);
        return this;
    }

    public bool AssertAditionalChecks()
    {
        bool result = true;
        foreach(var Check in AdditionalChecks)
        {
            result = result && Check.Assert();
        }
        return result;
    }

    public bool LateUpdatePrePerformResult()
    {
        bool Result = true;
        foreach (var Effect in AditionalPreEffects)
        {
            if (Effect.GetResult() == EAdditionalEffectResult.Fail)
            {
                Effect.LateUpdate();
            }

            Result = Result && (Effect.GetResult() != EAdditionalEffectResult.Fail);
        }
        return Result;
    }

    public void ResetPrePerformEffects()
    {
        foreach (var Effect in AditionalPreEffects)
        {
            Effect.ResertResult();
        }
    }

    public bool LateUpdatePostPerformResult()
    {
        bool Result = true;
        foreach (var Effect in AditionalPostEffects)
        {
            if (Effect.GetResult() == EAdditionalEffectResult.Fail)
            {
                Effect.LateUpdate();
            }

            Result = Result && (Effect.GetResult() != EAdditionalEffectResult.Fail);
        }
        return Result;
    }

    public void ResetPostPerformEffects()
    {
        foreach (var Effect in AditionalPostEffects)
        {
            Effect.ResertResult();
        }
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

    private List<AdditionalCheck> AdditionalChecks;
    private List<AdditionalEffect> AditionalPreEffects;
    private List<AdditionalEffect> AditionalPostEffects;

    private StatesDictionary Preconditions;
    private StatesDictionary Results;
}
