
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
        public List<BoughtProducts> Get(int? productId) // haalt de client, product en boodschappenlijst van iedereen die een bepaald product in hun boodschappenlijst heeft.
        {
            if (productId == null) return new List<BoughtProducts>();
            List<GroceryList> groceryLists = _groceryListRepository.GetAll();
            
            return (from groceryList in groceryLists let groceryListItems = _groceryListItemsRepository
                .GetAllOnGroceryListId(groceryList.Id) where groceryListItems
                .Any(g => g.ProductId == productId) let client = _clientRepository
                .Get(groceryList.ClientId) let product = _productRepository
                .Get(groceryList.Id) select new BoughtProducts(client, groceryList, product))
                .ToList();
        }
    }
}
