using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017
{
    // En del av uppgiften är att fundera över vad
    // det är som inte står i specen. När du stöter
    // på något som är osäkert, skriv ner som en
    // kommentar hur du tänkt.


    // Testa och implementera
    public interface IWebshop
    {
        IBasket Basket { get; }

        void Checkout(IBilling billing);
    }

    // Testa och implementera
    public interface IBasket
    {
        void AddProduct(Product p, int amount);
        void RemoveProduct(Product p, int amount);
        decimal TotalCost { get; }
    }

    // Mocka
    public interface IBilling
    {
        decimal Balance { get; set; }
        void Pay(decimal amount);
    }

    public class Product
    {
        public decimal Price { get; set; }

        public string Name { get; set; }
    }

}
