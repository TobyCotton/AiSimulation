using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;


/*
    A class which handles the Agent's 'money'.
    The value can be changed by paying or receiving a salary.
*/
public class MoneyComponent : MonoBehaviour
{
    // ~ public interface
    public void Start()
    {
        ReceiveSalary(0);
    }

    public bool CanAfford(int Sum)
    {
        return Money >= Sum;
    }

    public void Pay(int Sum)
    {
        Money -= Sum;

        MoneyText.SetText("Money: " + Money.ToString());
    }

    public void ReceiveSalary(int Sum)
    {
        Money += Sum;

        MoneyText.SetText("Money: " + Money.ToString());
    }

    // ~ private interface
    [SerializeField]
    private TextMeshProUGUI MoneyText;

    private int Money = 0;
}

/*
    An additional check for an Agent attempting to pay a price.
*/
public class PaymentCheck : AdditionalCheck
{
    // ~ public interface
    public PaymentCheck(MoneyComponent Money, int price)
    {
        InjectedMoney = Money;
        Price = price;
    }

    public override bool Assert()
    {
        return InjectedMoney.CanAfford(Price);
    }

    // ~ private interface
    private MoneyComponent InjectedMoney;
    private int Price;
}

/*
    An additional check for an Agent attempting to work.
    The Agent needs to be rested and well fed in order to pass.
*/
public class IsReadyToWorkCheck : AdditionalCheck
{
    // ~ public interface
    public IsReadyToWorkCheck(StaminaComponent Stamina, HungerComponent Hunger)
    {
        InjectedStamina = Stamina;
        InjectedHunger = Hunger;
    }

    public override bool Assert()
    {
        bool NotTired = !InjectedStamina.IsTired();
        bool NotHungry = !InjectedHunger.IsHungry();
        return NotTired && NotHungry;
    }

    // ~ private interface
    private StaminaComponent InjectedStamina;
    private HungerComponent InjectedHunger;
}

/*
    An additional effect for an Agent paying a certain price.
*/
public class PayEffect : AdditionalEffect
{
    // ~ public interface
    public PayEffect(MoneyComponent Money, int price)
    {
        InjectedMoney = Money;
        Price = price;
    }

    public override void LateUpdate()
    {
        if(InjectedMoney.CanAfford(Price))
        {
            InjectedMoney.Pay(Price);
            SetResult(EAdditionalEffectResult.Success);
        }
        else
        {
            SetResult(EAdditionalEffectResult.Fail);
        }
    }

    // ~ private interface
    private MoneyComponent InjectedMoney;
    private int Price;
}

/*
    An additional effect for an Agent receiving a salary.
*/
public class SalaryEffect : AdditionalEffect
{
    // ~ public interface
    public SalaryEffect(MoneyComponent Money, int salary)
    {
        InjectedMoney = Money;
        Salary = salary;
    }

    public override void LateUpdate()
    {
        InjectedMoney.ReceiveSalary(Salary);
        SetResult(EAdditionalEffectResult.Success);
    }

    // ~ private interface
    private MoneyComponent InjectedMoney;
    private int Salary;
}