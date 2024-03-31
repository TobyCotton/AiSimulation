using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StaminaComponent : MonoBehaviour
{
    // ~ public interface
    void Start()
    {
        Rest();
    }

    public bool IsTired()
    {
        return Stamina <= 0.0f;
    }

    public void DepleteStamina(float Percentange)
    {
        Percentange = Mathf.Clamp(Percentange / 100.0f, 0.0f, 1.0f);
        Stamina -= Percentange * FullStamina;
        Stamina = Mathf.Clamp(Stamina, 0.0f, FullStamina);

        StaminaText.SetText("Stamina: " + Stamina.ToString());
    }

    public void Rest()
    {
        Stamina = FullStamina;

        StaminaText.SetText("Stamina: " + Stamina.ToString());
    }

    // ~ private interface
    [SerializeField]
    private TextMeshProUGUI StaminaText;

    private float FullStamina = 100.0f;
    private float Stamina;
}

public class IsTiredCheck : AdditionalCheck
{
    // ~ public interface
    public IsTiredCheck(StaminaComponent Stamina)
    {
        InjectedStamina = Stamina;
    }

    public override bool Assert()
    {
        return InjectedStamina.IsTired();
    }

    // ~ private interface
    private StaminaComponent InjectedStamina;
}

public class RestEffect : AdditionalEffect
{
    // ~ public interface
    public RestEffect(StaminaComponent Stamina)
    {
        InjectedStamina = Stamina;
    }

    public override void LateUpdate()
    {
        InjectedStamina.Rest();
        SetResult(EAdditionalEffectResult.Success);
    }

    // ~ private interface
    private StaminaComponent InjectedStamina;
}

public class WorkEffect : AdditionalEffect
{
    // ~ public interface
    public WorkEffect(StaminaComponent Stamina, float percent)
    {
        InjectedStamina = Stamina;
        Percent = percent;
    }

    public override void LateUpdate()
    {
        InjectedStamina.DepleteStamina(Percent);
        SetResult(EAdditionalEffectResult.Success);
    }

    // ~ private interface
    private StaminaComponent InjectedStamina;
    private float Percent;
}