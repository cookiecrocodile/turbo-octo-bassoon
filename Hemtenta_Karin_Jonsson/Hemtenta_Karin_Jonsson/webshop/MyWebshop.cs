using System;

namespace HemtentaTdd2017.webshop
{
    public class MyWebshop : IWebshop
    {
        public void SetBasket(IBasket basket)
        {
            Basket = basket;
        }

        public IBasket Basket { get; private set; }

        public void Checkout(IBilling billing)
        {
            if (billing == null)
                throw new ArgumentNullException();

            if (Basket == null)
            {
                throw new NullReferenceException();
            }

            if (Basket.TotalCost == 0)
                throw new Exception();

            billing.Pay(Basket.TotalCost);

            Basket = new Basket();
        }
    }
}
