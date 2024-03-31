using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerComponent : MonoBehaviour
{
    // ~ public interface
    public void Start()
    {
        Eat();
    }

    public void Update()
    {
        Nourishment = Mathf.Max(Nourishment - Time.deltaTime * 5.0f, 0.0f);
    }

    public bool IsHungry()
    {
        return Nourishment <= 0.0f;
    }

    public void Eat()
    {
        Nourishment = FullNourishment;
    }

    // ~ private interface
    private const float FullNourishment = 100.0f;
    private float Nourishment;
}

public class IsHungryCheck : AdditionalCheck
{
    // ~ public interface
    public IsHungryCheck(HungerComponent Hunger)
    {
        InjectedHunger = Hunger;
    }

    public override bool Assert()
    {
        return InjectedHunger.IsHungry();
    }

    // ~ private interface
    private HungerComponent InjectedHunger;
};

public class EatEffect : AdditionalEffect
{
    // ~ public interface
    public EatEffect(HungerComponent Hunger)
    {
        InjectedHunger = Hunger;
    }
    
    public override void LateUpdate()
    {
        InjectedHunger.Eat();
        SetResult(EAdditionalEffectResult.Success);
    }

    // ~ private interface
    private HungerComponent InjectedHunger;
}