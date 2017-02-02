using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using HemtentaTdd2017.webshop;
using HemtentaTdd2017;

namespace HemtentaTester
{
    [TestFixture]
    public class BasketTest
    {
        Basket b;

        [SetUp]
        public void Setup()
        {
            b = new Basket();
        }

        [TestCase(1)]
        [TestCase(5000)]

        public void AddProduct_Success(int amount)
        {
            decimal expected;

            Product p = new Product() { Name = "Name", Price = 5.9m };

            expected = b.TotalCost + p.Price * amount;
            b.AddProduct(p, amount);
            
            Assert.That(expected, Is.EqualTo(b.TotalCost));

            p.Price = 0;
            expected = b.TotalCost + p.Price * amount;
            b.AddProduct(p, amount);
            
            Assert.That(expected, Is.EqualTo(b.TotalCost));

        }

        [Test]
        public void AddProduct_ProductNullThrowsException()
        {
            Product p = null;
            Assert.That(() => b.AddProduct(p, 1), Throws.TypeOf<ArgumentNullException>());

            
        }

        [Test]
        public void AddProduct_ProductNameNullOrEmptyThrowsException()
        {
            Product p = new Product();
            Assert.That(() => b.AddProduct(p, 1), Throws.TypeOf<ArgumentException>());
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void AddProduct_InvalidAmountThrowsException(int amount)
        {
            Product p = new Product();
            Assert.That(() => b.AddProduct(p, amount), Throws.TypeOf<ArgumentException>());
        }

        [TestCase(563, 1)]
        [TestCase(17, 6)]
        public void RemoveProduct_SuccessRemovesCorrectAmount(int noPInDb, int noToRemove)
        {
            //eftersom en produkt har Price som enda property har jag förutsatt att man
            //kan identifiera den via priset, alltså att det inte finns flera produkter
            //med samma pris

            Product p = new Product() { Name = "p", Price = 135.5m };
            b.AddProduct(p, noPInDb);

            Product p2 = new Product() { Name = "p2", Price = 564.7m };
            b.AddProduct(p2, 1);

            decimal expected = b.TotalCost - (noToRemove * p.Price);

            b.RemoveProduct(p, noToRemove);

            Assert.That(expected, Is.EqualTo(b.TotalCost));

        }

        [TestCase(-1)]
        public void RemoveProduct_InvalidAmountThrowsException(int amount)
        {
            Product p = new Product();
            Assert.That(() => b.RemoveProduct(p, amount), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void RemoveProduct_ProductNotInBasketThrowsException()
        {
            //eftersom en produkt har Price som enda property har jag förutsatt att man
            //kan identifiera den via priset, alltså att det inte finns flera produkter
            //med samma pris

            Product inBasket = new Product() { Name ="Test 1", Price = 100.5m};
            Product pRemove = new Product() { Name = "Removable", Price = 93.9m };

            b.AddProduct(inBasket, 2);

            Assert.That(() => b.RemoveProduct(pRemove, 1), Throws.TypeOf<NullReferenceException>());

            
        }

        [Test]
        public void RemoveProduct_AmountHigherThanInBasketRemovesAll() 
        {
            //Tänker att om man försöker ta bort fler av produkten än det finns
            //så tar jag bort alla som finns, hellre än att kasta ett exception

            Product inBasket = new Product() { Name="Temporary product", Price = 100m };
            b.AddProduct(inBasket, 1);
            decimal expected = b.TotalCost;

            Product toRemoveLater = new Product() { Name = "Temporary product 2", Price = 1590.9m };
            b.AddProduct(toRemoveLater, 2);

            b.RemoveProduct(toRemoveLater, 3);

            Assert.That(expected, Is.EqualTo(b.TotalCost));
            
        }
    }
}
