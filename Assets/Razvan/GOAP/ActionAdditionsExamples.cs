using System.Collections;
using System.Collections.Generic;
using TMPro;
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