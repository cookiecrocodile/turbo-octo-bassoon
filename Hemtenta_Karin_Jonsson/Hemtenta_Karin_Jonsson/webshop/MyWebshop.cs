using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if (Basket.TotalCost == 0)
                throw new Exception();

            billing.Pay(Basket.TotalCost);

            Basket = new Basket();
        }
    }
}
