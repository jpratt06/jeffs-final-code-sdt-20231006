namespace Banking.Domain;

public interface ICalculateBonuses
{
    decimal GetBonusForDepositOn(decimal balance, decimal amountToDeposit);
}