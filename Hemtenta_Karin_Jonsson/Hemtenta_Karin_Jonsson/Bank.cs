using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017
{
    // Representerar ett konto. Implementera den här!
    // Obs: i vanliga fall ska datatypen decimal användas
    // i stället för double när man hanterar pengar.
    public interface IAccount
    {
        // behöver inte testas
        double Amount { get; }

        // Sätter in ett belopp på kontot
        void Deposit(double amount);

        // Gör ett uttag från kontot
        void Withdraw(double amount);

        // Överför ett belopp från ett konto till ett annat
        void TransferFunds(IAccount destination, double amount);
    }

    // Kastas när beloppet på kontot inte tillåter
    // ett uttag eller en överföring
    public class InsufficientFundsException : Exception { }

    // Kastas för ogiltiga siffror
    public class IllegalAmountException : Exception { }

    // Kastas om en operation på kontot inte tillåts av någon
    // anledning som inte de andra exceptions täcker in
    public class OperationNotPermittedException : Exception { }
}
