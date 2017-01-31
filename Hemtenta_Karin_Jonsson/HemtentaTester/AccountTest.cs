using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using HemtentaTdd2017.bank;
using HemtentaTdd2017;

namespace HemtentaTester
{
    [TestFixture]
    public class AccountTest
    {
        Account account;

        [SetUp]
        public void Setup()
        {
            account = new Account();
        }

        [TestCase(0.1)]
        [TestCase(double.MaxValue)]
        public void Deposit_ValidAmountSuccess(double amount)
        {
            double originalAmount = account.Amount;
            account.Deposit(amount);
            Assert.That(account.Amount, Is.EqualTo(originalAmount + amount));

            //Om det inte funkar, kolla på .Within(0.001)
            //för att det är decimaltal
            //Kolla att amount ökat med det insatta
        }

        [TestCase(-0.01)]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        public void Deposit_InvalidAmountThrowsException(double amount)
        {
            Assert.That(() => account.Deposit(amount), Throws.TypeOf<IllegalAmountException>());

            //negative numbers, 0(?), NaN, Infinity(s)
            //IllegalAmountException
        }

        [Test]
        public void Deposit_ValidInputTooLargeForAccountThrowsException()
        {
            account.Deposit(1);

            Assert.That(() => account.Deposit(double.MaxValue), Throws.TypeOf<OperationNotPermittedException>());

        }

        [TestCase(0.1, 0.1)]
        [TestCase(5, 500)]
        [TestCase(double.MaxValue, double.MaxValue)]
        [TestCase(25000.55, double.MaxValue)]
        public void Withdraw_ValidAmountSuccess(double withdraw, double originallyInAccount)
        {
            account.Deposit(originallyInAccount);
            double amount = account.Amount;

            account.Withdraw(withdraw);

            Assert.That(account.Amount, Is.EqualTo(originallyInAccount - withdraw));

        }

        [TestCase(-0.01)]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        public void Withdraw_InvalidAmountThrowsException(double amount)
        {
            Assert.That(() => account.Withdraw(amount), Throws.TypeOf<IllegalAmountException>());
        }


        [Test]
        public void Withdraw_ValidInputLargerThanAmountThrowsException()
        {

            Assert.That(() => account.Withdraw(0.1), Throws.TypeOf<InsufficientFundsException>());

            //Använd .Within om den bråkar
            account.Deposit(5000);
            Assert.That(() => account.Withdraw(5000.1), Throws.TypeOf<InsufficientFundsException>());
        }

        [TestCase(-0.01)]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        public void TransferFunds_InvalidAmountThrowsException(double amount)
        {
            IAccount reciever = new Account();
            Assert.That(() => account.TransferFunds(reciever, amount), Throws.TypeOf<IllegalAmountException>());
        }

        [Test]
        public void TransferFunds_InsufficientFundsThrowsException()
        {
            IAccount reciever = new Account();
            Assert.That(() => account.TransferFunds(reciever, 0.1), Throws.TypeOf<InsufficientFundsException>());
        }

        [Test]
        public void TransferFunds_SumTooLargeForRecieverThrowsException()
        {
            IAccount reciever = new Account();
            reciever.Deposit(double.MaxValue);
            account.Deposit(double.MaxValue);


            Assert.That(() => account.TransferFunds(reciever, 1), Throws.TypeOf<OperationNotPermittedException>());

        }

        [Test]
        public void TransferFunds_ReceivingAccountNullThrowsException()
        {
            Assert.That(() => account.TransferFunds(null, 500), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void TransferFunds_ValidValuesTransferSuccessfully()
        {
            IAccount reciever = new Account();

            double transferAmount = 5000;

            reciever.Deposit(100.5);
            account.Deposit(6000);

            double recieverOriginalAmount = reciever.Amount;
            double senderOriginalAmount = account.Amount;

            account.TransferFunds(reciever, transferAmount);

            Assert.That((senderOriginalAmount - transferAmount), Is.EqualTo(account.Amount));
            Assert.That((recieverOriginalAmount + transferAmount), Is.EqualTo(reciever.Amount));

        }

        // Överför ett belopp från ett konto till ett annat
        //void TransferFunds(IAccount destination, double amount);

        //Kolla att amount är giltigt värde, annars exceptions
        //Kolla att mottagande konto kan ta emot så mycket
        //I så fall, överför och kolla så att Amount sjunkit och mottagande kontos Amount ökat
        //Kolla att så mycket pengar får plats på mottagande konto, annars exception

        /*
         EXCEPTIONS

        // Kastas när beloppet på kontot inte tillåter
    // ett uttag eller en överföring
    InsufficientFundsException

    // Kastas för ogiltiga siffror
   IllegalAmountException

    // Kastas om en operation på kontot inte tillåts av någon
    // anledning som inte de andra exceptions täcker in
    OperationNotPermittedException
         
         */
    }
}
