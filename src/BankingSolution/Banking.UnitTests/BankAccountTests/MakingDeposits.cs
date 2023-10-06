using Banking.Domain;

namespace Banking.UnitTests.BankAccountTests;
public class MakingDeposits
{
    [Fact]
    public void MakingADepositIncreasesTheBalance()
    {
        // Given
        var account = new Account(Substitute.For<ICalculateBonuses>(), Substitute.For<INotifyOfFraudDetection>());
        var openingBalance = account.GetBalance();
        var amountToDeposit = 100M;

        // When
        account.Deposit(amountToDeposit);

        // Then
        Assert.Equal(openingBalance + amountToDeposit, account.GetBalance());
    }

}
