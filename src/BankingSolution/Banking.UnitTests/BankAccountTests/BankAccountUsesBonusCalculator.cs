using Banking.Domain;

namespace Banking.UnitTests.BankAccountTests;
public class BankAccountUsesBonusCalculator
{
    [Fact]
    public void BonusCalculatorUsedInDeposit()
    {
        // Given
        var stubbedBonusCalculator = Substitute.For<ICalculateBonuses>(); // Stub
        var account = new Account(stubbedBonusCalculator, Substitute.For<INotifyOfFraudDetection>());
        var openingBalance = account.GetBalance();
        var amountToDeposit = 100M;
        stubbedBonusCalculator.GetBonusForDepositOn(openingBalance, amountToDeposit).Returns(42.23M);


        // When
        account.Deposit(amountToDeposit); // SUT - does my deposit method use the bonus calculator correctly.

        Assert.Equal(amountToDeposit + openingBalance + 42.23M, account.GetBalance());
    }
}
