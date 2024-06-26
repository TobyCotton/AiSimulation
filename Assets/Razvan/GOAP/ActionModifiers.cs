using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    A base class used for creating custom additional checks.
*/
public abstract class AdditionalCheck
{
    // ~ public interface
    public abstract bool Assert();
};

public enum EAdditionalEffectResult
{
    Success,
    Fail,
};

/*
    A base class used for creating custom additional effects.
*/
public abstract class AdditionalEffect
{
    // ~ public interface
    public abstract void LateUpdate();
    public EAdditionalEffectResult GetResult()
    {
        return CurrentResult;
    }

    public void ResertResult()
    {
        CurrentResult = EAdditionalEffectResult.Fail;
    }

    // ~ protected interface
    protected void SetResult(EAdditionalEffectResult Result)
    {
        CurrentResult = Result;
    }

    // ~ private interface
    private EAdditionalEffectResult CurrentResult = EAdditionalEffectResult.Fail;
}