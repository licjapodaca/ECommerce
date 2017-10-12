using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.CheckoutService.Model
{
    public interface ICheckoutService : IService
    {
		Task<CheckoutSummary> Checkout(string userId);

		Task<IEnumerable<CheckoutSummary>> GetOrderHistory(string userId);
    }
}
