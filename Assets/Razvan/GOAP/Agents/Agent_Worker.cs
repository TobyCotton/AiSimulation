using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Agent_Worker : Agent
{
    // ~ public interface
    new public void Start()
    {
        base.Start();

        Hunger = GetComponent<HungerComponent>();
        Stamina = GetComponent<StaminaComponent>();
        Money = GetComponent<MoneyComponent>();

        AddAction(new Action()).
            SetTargetTag("Social").
            SetCost(100.0f).
            SetDuration(2.0f).
            AddAdditionalPreEffect(new UIPrintEffect(ActionText, "Spending time at Social.")).
            AddResults("Idle");

        AddAction(new Action()).
            SetTargetTag("House").
            SetCost(3.0f).
            SetDuration(5.0f).
            AddAdditionalCheck(new IsTiredCheck(Stamina)).
            AddAdditionalPreEffect(new UIPrintEffect(ActionText, "Resting at House.")).
            AddResults("Idle").
            AddAdditionalPostEffect(new RestEffect(Stamina));

        AddAction(new Action()).
            SetTargetTag("Hotel").
            SetCost(1.0f).
            SetDuration(2.5f).
            AddAdditionalCheck(new IsTiredCheck(Stamina)).
            AddAdditionalCheck(new PaymentCheck(Money, 25)).
            AddAdditionalPreEffect(new UIPrintEffect(ActionText, "Resting at Hotel.")).
            AddAdditionalPreEffect(new PayEffect(Money, 25)).
            AddResults("Idle").
            AddAdditionalPostEffect(new RestEffect(Stamina));

        AddAction(new Action()).
            SetTargetTag("Cafeteria").
            SetCost(3.0f).
            SetDuration(5.0f).
            AddAdditionalCheck(new IsHungryCheck(Hunger)).
            AddAdditionalPreEffect(new UIPrintEffect(ActionText, "Eating at Cafeteria")).
            AddResults("Idle").
            AddAdditionalPostEffect(new EatEffect(Hunger));

        AddAction(new Action()).
            SetTargetTag("Restaurant").
            SetCost(1.0f).
            SetDuration(2.5f).
            AddAdditionalCheck(new IsHungryCheck(Hunger)).
            AddAdditionalCheck(new PaymentCheck(Money, 10)).
            AddAdditionalPreEffect(new UIPrintEffect(ActionText, "Eating at Restaurant")).
            AddAdditionalPreEffect(new PayEffect(Money, 10)).
            AddResults("Idle").
            AddAdditionalPostEffect(new EatEffect(Hunger));

        AddAction(new Action()).
            SetTargetTag("Workplace").
            SetCost(1.0f).
            SetDuration(7.0f).
            AddPrecondition("Idle").
            AddAdditionalCheck(new IsReadyToWorkCheck(Stamina, Hunger)).
            AddAdditionalPreEffect(new UIPrintEffect(ActionText, "Working")).
            AddResults("Working").
            AddAdditionalPostEffect(new WorkEffect(Stamina, 25)).
            AddAdditionalPostEffect(new SalaryEffect(Money, 5));

        AddGoal(new SubGoal("Working", true), 2);
        AddGoal(new SubGoal("Idle", true), 1);
    }

    // ~ private interface
    private HungerComponent Hunger;
    private StaminaComponent Stamina;
    private MoneyComponent Money;

    [SerializeField]
    private TextMeshProUGUI ActionText;
}
