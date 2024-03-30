using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Agent_Worker : Agent
{
    new void Start()
    {
        base.Start();

        Hunger = GetComponent<HungerComponent>();
        Stamina = GetComponent<StaminaComponent>();
        Money = GetComponent<MoneyComponent>();

        AddAction(new Action()).
            SetTargetTag("Social").
            SetCost(100.0f).
            SetDuration(1.0f).
            AddAdditionalPreEffect(new PrintEffect("Spending time at Social.")).
            AddResults(EStates.Idle, 1);

        AddAction(new Action()).
            SetTargetTag("House").
            SetCost(3.0f).
            SetDuration(7.0f).
            AddAdditionalCheck(new IsTiredCheck(Stamina)).
            AddAdditionalPreEffect(new PrintEffect("Resting at House.")).
            AddResults(EStates.Idle, 1).
            AddAdditionalPostEffect(new RestEffect(Stamina));

        AddAction(new Action()).
            SetTargetTag("Hotel").
            SetCost(1.0f).
            SetDuration(2.0f).
            AddAdditionalCheck(new IsTiredCheck(Stamina)).
            AddAdditionalCheck(new PaymentCheck(Money, 25)).
            AddAdditionalPreEffect(new PrintEffect("Resting at Hotel.")).
            AddAdditionalPreEffect(new PayEffect(Money, 25)).
            AddResults(EStates.Idle, 1).
            AddAdditionalPostEffect(new RestEffect(Stamina));

        AddAction(new Action()).
            SetTargetTag("Cafeteria").
            SetCost(3.0f).
            SetDuration(7.0f).
            AddAdditionalCheck(new IsHungryCheck(Hunger)).
            AddAdditionalPreEffect(new PrintEffect("Eating at Cafeteria")).
            AddResults(EStates.Idle, 1).
            AddAdditionalPostEffect(new EatEffect(Hunger));

        AddAction(new Action()).
            SetTargetTag("Restaurant").
            SetCost(1.0f).
            SetDuration(5.0f).
            AddAdditionalCheck(new IsHungryCheck(Hunger)).
            AddAdditionalCheck(new PaymentCheck(Money, 10)).
            AddAdditionalPreEffect(new PrintEffect("Eating at Restaurant")).
            AddAdditionalPreEffect(new PayEffect(Money, 10)).
            AddResults(EStates.Idle, 1).
            AddAdditionalPostEffect(new EatEffect(Hunger));

        AddAction(new Action()).
            SetTargetTag("Workplace").
            SetCost(1.0f).
            SetDuration(15.0f).
            AddPrecondition(EStates.Idle, 1).
            AddAdditionalCheck(new IsReadyToWorkCheck(Stamina, Hunger)).
            AddAdditionalPreEffect(new PrintEffect("Working")).
            AddResults(EStates.Working, 1).
            AddAdditionalPostEffect(new SalaryEffect(Money, 100));

        AddGoal(new SubGoal(EStates.Working, 1, true), 1);
    }

    // ~ private interface
    private HungerComponent Hunger;
    private StaminaComponent Stamina;
    private MoneyComponent Money;
}
