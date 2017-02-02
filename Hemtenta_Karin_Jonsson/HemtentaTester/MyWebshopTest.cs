using System;
using NUnit.Framework;
using Moq;
using HemtentaTdd2017.webshop;
using HemtentaTdd2017;

namespace HemtentaTester
{
    /*
     Vilka metoder behöver testas?
     
     MyWebshop: Checkout
     Basket: AddProduct, RemoveProduct

     Ska några exceptions kastas?
    
     MyWebshop: 
        Checkout: om IBilling kastar exception, om IBilling är null, om IBasket är tom 
     Basket: 
        AddProduct: produkt är null, amount är 0 eller mindre
        RemoveProduct: amount är mindre än 0, produkten är null, produkten finns inte i korgen
                       
        Jag valde att inte kasta exception om man vill ta bort fler än det finns,
        utan att i så fall istället bara ta bort de som finns.

        Jag har förutsatt att namnen på produkterna är unika. Om de inte är det
        får man tänka lite annorlunda.

     Vilka är domänerna för IWebshop och IBasket?

     Giltiga objekt som implementerar respektive interface, eller null.
 
    */

    [TestFixture]
    class MyWebshopTest
    {
        MyWebshop mw;

        [SetUp]
        public void Setup()
        {
            mw = new MyWebshop();
        }
   
        [Test]
        public void Checkout_Success()
        {
            var mockBilling = new Mock<IBilling>();

            var basket = new Basket();
            Product p = new Product();
            p.Price = 50.5m;
            basket.AddProduct(p, 4);
            decimal total = basket.TotalCost;
            mw.SetBasket(basket);

            mw.Checkout(mockBilling.Object);

            Assert.That(mw.Basket.TotalCost, Is.EqualTo(0));
            mockBilling.Verify(x => x.Pay(total), Times.Once);
           
        }

        [Test]
        public void Checkout_ThrowsExceptionIfBillingFails()
        {
            var basket = new Basket();
            Product p = new Product();
            p.Price = 100m;
            basket.AddProduct(p, 4);
            decimal total = basket.TotalCost;
            mw.SetBasket(basket);

            var mockBilling = new Mock<IBilling>();
            mockBilling.Setup(x => x.Pay(total)).Throws(new InsufficientFundsException());
            
            Assert.That(() => mw.Checkout(mockBilling.Object), Throws.TypeOf<InsufficientFundsException>());
            Assert.That(basket.TotalCost, Is.EqualTo(total));
        }

        [Test]
        public void Checkout_NothingInBasketThrowsException()
        {
            var mockBasket = new Mock<IBasket>();
            mockBasket.Setup(x => x.TotalCost).Returns(0);
            mw.SetBasket(mockBasket.Object);

            var mockBilling = new Mock<IBilling>();

            Assert.That(() => mw.Checkout(mockBilling.Object), Throws.TypeOf<Exception>());
            
        }

        [Test]
        public void Checkout_InvalidParameterThrowsException()
        {
            Assert.That(() => mw.Checkout(null), Throws.TypeOf<ArgumentNullException>());
        }

    }
}
