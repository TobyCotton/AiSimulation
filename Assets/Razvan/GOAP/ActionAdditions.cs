using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AdditionalCheck
{
    // ~ public interface
    public abstract bool Assert();
};

public enum EAdditionalEffectResult
{
    Success,
    Abandoned,
    Fail,
};

public abstract class AdditionalEffect
{
    // ~ public interface
    public abstract void Perform();
    public EAdditionalEffectResult GetResult()
    {
        return CurrentResult;
    }

    // ~ protected interface
    protected void SetResult(EAdditionalEffectResult Result)
    {
        CurrentResult = Result;
    }

    // ~ private interface
    private EAdditionalEffectResult CurrentResult = EAdditionalEffectResult.Fail;
}