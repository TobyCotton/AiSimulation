using System.Collections;
using System.Collections.Generic;
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
    }

    public void Rest()
    {
        Stamina = FullStamina;
    }

    // ~ private interface
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