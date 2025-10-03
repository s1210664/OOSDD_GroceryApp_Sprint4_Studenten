using System.Collections.Immutable;
using System.Data;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class GroceryListItemsService : IGroceryListItemsService
    {
        private readonly IGroceryListItemsRepository _groceriesRepository;
        private readonly IProductRepository _productRepository;

        public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository, IProductRepository productRepository)
        {
            _groceriesRepository = groceriesRepository;
            _productRepository = productRepository;
        }

        public List<GroceryListItem> GetAll()
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll().Where(g => g.GroceryListId == groceryListId).ToList();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            return _groceriesRepository.Add(item);
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            throw new NotImplementedException();
        }

        public GroceryListItem? Get(int id)
        {
            return _groceriesRepository.Get(id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            return _groceriesRepository.Update(item);
        }

        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            List<BestSellingProducts> bestsellingproducts = new List<BestSellingProducts>();
            
            List<Product> products = _productRepository.GetAll();
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            Dictionary<Product, int> unrankedproducts = new Dictionary<Product, int>();

            foreach (var product in products) // telt de hoeveelheden van producten die hoger dan 0 zijn op en plaatst ze in unrankedproducts
            {
                int productAmount = groceryListItems
                    .Where(groceryListItem => groceryListItem.ProductId == product.Id && groceryListItem.Amount > 0)
                    .Sum(groceryListItem => groceryListItem.Amount);

                unrankedproducts.Add(product, productAmount);

            }
            
            var rankedproducts = unrankedproducts.OrderByDescending(kvp => kvp.Value).ToList(); // sorteer de keyvaluepairs op hoeveelheid en plaatst deze in een lijst
            for (int i = 0;i < topX && i < rankedproducts.Count; i++)
            {
                Product rankedproduct = rankedproducts[i].Key;
                int rankedproductamount = rankedproducts[i].Value;
                bestsellingproducts.Add(new BestSellingProducts(rankedproduct.Id, rankedproduct.Name, rankedproduct.Stock, rankedproductamount, i+1));
            }
            
            return bestsellingproducts;
        }

        private void FillService(List<GroceryListItem> groceryListItems)
        {
            foreach (GroceryListItem g in groceryListItems)
            {
                g.Product = _productRepository.Get(g.ProductId) ?? new(0, "", 0);
            }
        }
    }
}
