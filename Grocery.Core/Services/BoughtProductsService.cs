
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        public BoughtProductsService(IGroceryListItemsRepository groceryListItemsRepository, IGroceryListRepository groceryListRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _groceryListItemsRepository=groceryListItemsRepository;
            _groceryListRepository=groceryListRepository;
            _clientRepository=clientRepository;
            _productRepository=productRepository;
        }
        public List<BoughtProducts> Get(int? productId)
        {
            if (productId == null) return new List<BoughtProducts>();
            List<GroceryList> groceryLists = _groceryListRepository.GetAll();
            List<BoughtProducts> boughtProducts = new List<BoughtProducts>();
            
            foreach (GroceryList groceryList in groceryLists)
            {
                
                
                List<GroceryListItem> groceryListItem = _groceryListItemsRepository.GetAllOnGroceryListId(groceryList.Id);
                if (!groceryListItem.Any(g => g.ProductId == productId)) continue;
                Client client = _clientRepository.Get(groceryList.ClientId);
                Product product = _productRepository.Get(groceryList.Id);
                
                boughtProducts.Add(new BoughtProducts(client, groceryList, product));
            }
                
            return boughtProducts;
        }
    }
}
