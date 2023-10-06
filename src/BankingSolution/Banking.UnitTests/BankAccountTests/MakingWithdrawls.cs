using Banking.Domain;

namespace Banking.UnitTests.BankAccountTests;
public class MakingWithdrawls
{
    [Fact]
    public void MakingAWithdrawalDecreasesBalance()
    {
        var account = new Account(Substitute.For<ICalculateBonuses>(), Substitute.For<INotifyOfFraudDetection>());
        var openingBalance = account.GetBalance();
        var amountToWithdraw = 100M;

        // When
        account.Withdraw(amountToWithdraw);

        // Then
        Assert.Equal(openingBalance - amountToWithdraw, account.GetBalance());
    }

    [Fact]
    public void CanTakeFullBalance()
    {
        var account = new Account(Substitute.For<ICalculateBonuses>(), Substitute.For<INotifyOfFraudDetection>());

        account.Withdraw(account.GetBalance());

        Assert.Equal(0, account.GetBalance());
    }

    [Fact]
    public void OverdraftNotAllowed()
    {
        var account = new Account(Substitute.For<ICalculateBonuses>(), Substitute.For<INotifyOfFraudDetection>());
        var openingBalance = account.GetBalance();
        var amountToWithdraw = account.GetBalance() + .01M;


        Assert.Throws<OverdraftException>(() => account.Withdraw(amountToWithdraw));


        // Then
        Assert.Equal(openingBalance, account.GetBalance());

    }
}
