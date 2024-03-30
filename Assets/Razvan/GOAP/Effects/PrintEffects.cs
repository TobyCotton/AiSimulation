using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPrintEffect : AdditionalEffect
{
    public UIPrintEffect(TextMeshProUGUI ActionText, string Text)
    {
        InjectedActionText = ActionText;
        DebugText = Text;
    }

    public override void LateUpdate()
    {
        InjectedActionText.SetText(DebugText);
        SetResult(EAdditionalEffectResult.Success);
    }

    private TextMeshProUGUI InjectedActionText;
    private string DebugText;
}

public class DebugPrintEffect : AdditionalEffect
{
    public DebugPrintEffect(string Text)
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

public class DebugAsyncPrintEffect : AdditionalEffect
{
    public DebugAsyncPrintEffect(string Text)
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