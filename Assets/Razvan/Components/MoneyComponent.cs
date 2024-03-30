using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MoneyComponent : MonoBehaviour
{
    // ~ public interface
    public void Start()
    {
        Money = 0;
    }

    public bool CanAfford(int Sum)
    {
        return Money >= Sum;
    }

    public void Pay(int Sum)
    {
        Money -= Sum;
    }

    public void ReceiveSalary(int Sum)
    {
        Money += Sum;
    }

    // ~ private interface
    private int Money;
}

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