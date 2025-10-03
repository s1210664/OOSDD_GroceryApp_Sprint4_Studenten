using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    public partial class GlobalViewModel : BaseViewModel
    {
        public required Client Client { get; set; }
    }
}
