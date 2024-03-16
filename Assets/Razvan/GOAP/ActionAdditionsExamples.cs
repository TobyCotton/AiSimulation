using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintEffect : AdditionalEffect
{
    public PrintEffect(string Text)
    {
        DebugText = Text;
    }

    public override void LateUpdate()
    {
        Debug.Log(DebugText);
        SetResult(EAdditionalEffectResult.Success);
    }

    private string DebugText;
}

public class AsyncPrintEffect : AdditionalEffect
{
    public AsyncPrintEffect(string Text)
    {
        DebugText = Text;
    }

    public override void LateUpdate()
    {
        if (i > 200)
        {
            Debug.Log(DebugText);
            SetResult(EAdditionalEffectResult.Success);
        }
        i++;
    }

    private string DebugText;
    private uint i = 0;
}
