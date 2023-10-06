namespace Banking.Domain;

public class Account
{
    private readonly ICalculateBonuses _bonusCalculator;
    private readonly INotifyOfFraudDetection _fraudDetection;

    public Account(ICalculateBonuses bonusCalculator, INotifyOfFraudDetection fraudDetection)
    {
        _bonusCalculator = bonusCalculator;
        _fraudDetection = fraudDetection;
    }

    private decimal _balance = 5000M;
    public void Deposit(decimal amountToDeposit)
    {
        decimal bonus = _bonusCalculator.GetBonusForDepositOn(_balance, amountToDeposit);

        _balance += amountToDeposit + bonus;
    }

    public decimal GetBalance()
    {
        return _balance;

    }

    public void Withdraw(decimal amountToWithdraw)
    {
        if (amountToWithdraw <= _balance)
        {
            _balance -= amountToWithdraw;
        }
        else
        {
            // The fraud detection people are notified.
            _fraudDetection.NotifyOfOverdraft(amountToWithdraw);
            throw new OverdraftException();
        }


    }
}

public class OverdraftException : ArgumentOutOfRangeException { }