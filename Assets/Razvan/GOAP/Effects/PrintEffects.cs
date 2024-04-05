using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/*
    An effect which prints the text on the injected text mesh pro text box.
*/
public class UIPrintEffect : AdditionalEffect
{
    // ~ public interface
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

    // ~ private interface
    private TextMeshProUGUI InjectedActionText;
    private string DebugText;
}

/*
    An effect which prints the text in the log.
*/
public class DebugPrintEffect : AdditionalEffect
{
    // ~ public interface
    public DebugPrintEffect(string Text)
    {
        DebugText = Text;
    }

    public override void LateUpdate()
    {
        Debug.Log(DebugText);
        SetResult(EAdditionalEffectResult.Success);
    }

    // ~ private interface
    private string DebugText;
}

/*
    An effect which prints the text in the log after 200 iteration of the update loop.
*/
public class DebugAsyncPrintEffect : AdditionalEffect
{
    // ~ public interface
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

    // ~ private interface
    private string DebugText;
    private uint i = 0;
}