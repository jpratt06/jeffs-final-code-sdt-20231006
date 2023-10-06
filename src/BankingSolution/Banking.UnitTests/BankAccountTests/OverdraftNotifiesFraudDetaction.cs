using Banking.Domain;

namespace Banking.UnitTests.BankAccountTests;
public class OverdraftNotifiesFraudDetaction
{
    [Fact]
    public void NotifiedOnOverdraft()
    {
        // Arrange
        var mockedFedNotifier = Substitute.For<INotifyOfFraudDetection>();
        var account = new Account(Substitute.For<ICalculateBonuses>(), mockedFedNotifier);
        var openingBalance = account.GetBalance();
        var amountToWithDraw = openingBalance + 10m;

        // Act
        try
        {
            account.Withdraw(amountToWithDraw);
        }
        catch (OverdraftException)
        {

            // Don't Care
        }

        // Assert
        mockedFedNotifier.Received().NotifyOfOverdraft(amountToWithDraw);

    }

    [Fact]
    public void NotNotifiedIfNoOverdraft()
    {
        var mockedFedNotifier = Substitute.For<INotifyOfFraudDetection>();
        var account = new Account(Substitute.For<ICalculateBonuses>(), mockedFedNotifier);
        var openingBalance = account.GetBalance();
        var amountToWithDraw = openingBalance;


        account.Withdraw(amountToWithDraw);

        mockedFedNotifier.DidNotReceive().NotifyOfOverdraft(Arg.Any<decimal>());
    }


}
