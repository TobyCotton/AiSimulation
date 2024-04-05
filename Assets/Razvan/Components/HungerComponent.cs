using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
    A class which handles the Agent's 'hunger'.
    The hunger value decreases based on time and hunger rate.
*/
public class HungerComponent : MonoBehaviour
{
    // ~ public interface
    public void Start()
    {
        Eat();
    }

    public void Update()
    {
        Nourishment = Mathf.Max(Nourishment - Time.deltaTime * HungerRate, 0.0f);

        HungerText.SetText("Hunger: " + ((int)Nourishment).ToString());
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
    [SerializeField]
    private TextMeshProUGUI HungerText;

    [SerializeField, Range(1.0f, 10.0f)]
    private float HungerRate = 5.0f;

    private const float FullNourishment = 100.0f;
    private float Nourishment;
}

/*
    An additional check for an Agent's hunger state.
*/
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

/*
    An additional effect which replenishes an Agent's hunger value.
*/
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