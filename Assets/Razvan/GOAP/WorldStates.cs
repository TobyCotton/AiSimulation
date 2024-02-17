using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StatesDictionary = System.Collections.Generic.Dictionary<EStates, int>;

public enum EStates
{
    Objective1,
    Objective2,
    Objective3,
}

[System.Serializable]
public class WorldState
{
    public EStates key;
    public int value;
}

public class WorldStates
{
    // ~ public interface
    public WorldStates()
    {
        States = new StatesDictionary();
    }

    public bool HasState(EStates key)
    {
        return States.ContainsKey(key);
    }

    public void AddState(EStates key, int value)
    {
        States.Add(key, value);
    }

    public void SetState(EStates key, int value)
    {
        if(HasState(key))
        {
            States[key] = value;
        }
        else
        {
            States.Add(key, value);
        }
    }

    public void ModifyState(EStates key, int value)
    {
        if (HasState(key))
        {
            States[key] += value;
        }
        else
        {
            States.Add(key, value);
        }
    }

    public void RemoveState(EStates key, int value)
    {
        if(HasState(key))
        {
            States.Remove(key);
        }
    }

    public StatesDictionary GetStates()
    {
        return States;
    }

    // ~ private interface
    private StatesDictionary States;
}
