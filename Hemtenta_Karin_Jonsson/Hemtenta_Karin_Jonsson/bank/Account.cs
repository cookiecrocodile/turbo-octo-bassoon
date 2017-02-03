using System;

namespace HemtentaTdd2017.bank
{
    public class Account : IAccount
    {
        public double Amount { get; private set; }
      
        public void Deposit(double amount)
        {
            if (double.IsNaN(amount) || double.IsInfinity(amount) || amount < 0 )
            {
                throw new IllegalAmountException();
            }

            var result = Amount + amount;

            if (result <= Amount && amount != 0) //overflow
            {
                throw new OperationNotPermittedException();
            }

            Amount = result;
            
        }

        public void TransferFunds(IAccount destination, double amount)
        {
            if (destination == null)
            {
                throw new ArgumentNullException();
            }

            if (double.IsNaN(amount) || double.IsInfinity(amount) || amount < 0)
            {
                throw new IllegalAmountException();
            }

            if (amount > Amount)
            {
                throw new InsufficientFundsException();
            }

            var resultingRecieverAmount = destination.Amount + amount;

            if (resultingRecieverAmount <= destination.Amount && amount != 0) //overflow
            {
                throw new OperationNotPermittedException();
            }

            Withdraw(amount);
            destination.Deposit(amount);

        }

        public void Withdraw(double amount)
        {
            if (double.IsNaN(amount) || double.IsInfinity(amount) || amount < 0)
            {
                throw new IllegalAmountException();
            }

            if (amount > Amount)
            {
                throw new InsufficientFundsException();
            }

            Amount -= amount;
        }
    }
}
