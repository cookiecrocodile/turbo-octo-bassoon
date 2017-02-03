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
        Checkout: om IBilling kastar exception, om IBilling är null, om IBasket är null, om Basket är tom 
     Basket: 
        AddProduct: produkt är null, produktens property Name or null/empty, amount är 0 eller mindre
        RemoveProduct: amount är mindre än 1, produkten är null, produkten finns inte i korgen
                       
        Jag har förutsatt att namnen på produkterna är unika. Om de inte är det
        får man tänka lite annorlunda.

     Vilka är domänerna för IWebshop och IBasket?

     IWebshop: Basket: objekt som implementerar IBasket, eller null.
               Checkout, billing: objekt som implementerar IBilling, eller null

     IBasket: AddProduct: Product kan vara giltiga Product-objekt eller null, amount kan vara alla heltal mellan
              int.MinValue och int.MaxValue
              Remove Product: Product och amount, se ovan.
              TotalCost: värden mellan decimal.MinValue och decimal.MaxValue      
 
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

        /*
        Checkout
        
        Har förutsatt att checkout använder sig av IBillings pay-metod och att Billing 
        kastar exception om betalningen misslyckas
        Om inget exception kommer från Billing förutsätter jag att betalningen lyckats
        och att checkout i så fall tömmer varukorgen
        Om inget fanns i korgen när man gick till checkout har jag också förutsatt att man får ett exception.
        Man kan förstås teoretiskt tänka sig att man kan ha en korg full med gratis-produkter, 
        men jag tänkte att man kanske inte ville kalla på pay-metoden i onödan
        
        */

        [Test]
        public void Checkout_Success()
        {
            var mockBilling = new Mock<IBilling>();

            var basket = new Basket();
            Product p = new Product() { Name = "Thing", Price = 50.5m };
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
            Product p = new Product() { Name = "Stuff", Price = 100m };
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
        public void Checkout_BasketNullThrowsException()
        {
            var mockBilling = new Mock<IBilling>();

            Assert.That(() => mw.Checkout(mockBilling.Object), Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void Checkout_InvalidParameterThrowsException()
        {
            Assert.That(() => mw.Checkout(null), Throws.TypeOf<ArgumentNullException>());
        }

    }
}
