using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StatesDictionary = System.Collections.Generic.Dictionary<EStates, int>;

public abstract class Action : MonoBehaviour
{
    // ~ public interface
    public string TargetTag;
    public float Cost = 1.0f;
    public float Duration = 0.0f;
    public WorldState[] EditorPreconditions;
    public WorldState[] EditorAfterEffects;

    public bool Executing = false;

    public Action()
    {
        Preconditions = new StatesDictionary();
        AfterEffects = new StatesDictionary();
    }

    public void Awake()
    {
        foreach (var precondition in EditorPreconditions)
        {
            Preconditions.Add(precondition.key, precondition.value);
        }

        foreach (var aftereffect in EditorAfterEffects)
        {
            AfterEffects.Add(aftereffect.key, aftereffect.value);
        }
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

    public abstract bool PrePerform();
    public abstract bool PostPerform();

    public bool CanFindTarget()
    {
        return TargetTag == "";
    }

    public StatesDictionary GetAfterEffects()
    {
        return AfterEffects;
    }

    // ~ private interface
    private StatesDictionary Preconditions;
    private StatesDictionary AfterEffects;
}
