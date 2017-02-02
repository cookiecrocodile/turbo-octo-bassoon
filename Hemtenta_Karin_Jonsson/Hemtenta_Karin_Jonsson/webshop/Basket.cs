using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017.webshop
{
    public class Basket : IBasket
    {
        List<Product> products = new List<Product>(); //skapat en variabel för att hålla produkterna

        public decimal TotalCost { get; private set; }

        public void AddProduct(Product p, int amount)
        {
            if (amount < 1)
                throw new ArgumentException();

            if (p == null)
                throw new ArgumentNullException();

            if (string.IsNullOrEmpty(p.Name))
            {
                throw new ArgumentException();
            }

            for (int i = 0; i < amount; i ++)
            {
                products.Add(p);
                TotalCost += p.Price;
            }
        }

        public void RemoveProduct(Product p, int amount)
        {
            if (amount < 0)
                throw new ArgumentException();

            var search = from pr in products
                         where pr.Name == p.Name
                         select pr;

            int hits = search.Count();

            if (hits == 0)
            {
                throw new NullReferenceException();
            }

            if (hits <= amount)
            {
                products.RemoveAll(item => item.Name == p.Name);
                TotalCost -= hits * p.Price;
            }

            if (hits > amount && amount != 0)
            {
                int count = 0;
                
                while(count< amount)
                {
                    Product found = products.Find(item => item.Name == p.Name);
                    products.Remove(found);
                    TotalCost -= p.Price;
                    count++;
                }
            }
        }
    }
}
