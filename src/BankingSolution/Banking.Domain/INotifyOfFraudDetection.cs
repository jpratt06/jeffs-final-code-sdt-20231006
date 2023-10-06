namespace Banking.Domain;

public interface INotifyOfFraudDetection
{
    void NotifyOfOverdraft(decimal amountToWithdraw);
}