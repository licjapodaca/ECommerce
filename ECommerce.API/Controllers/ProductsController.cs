using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Model;
using ECommerce.ProductCatalog.Model;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace ECommerce.API.Controllers
{
	[Route("api/[controller]")]
	public class ProductsController : Controller
	{
		private readonly IProductCatalogService _catalogService;
		private readonly ILogger _logger;

		public ProductsController(ILogger<ProductsController> logger)
		{
			_logger = logger;

			_catalogService = ServiceProxy.Create<IProductCatalogService>(
				new Uri("fabric:/ECommerce/ProductCatalog"),
				new ServicePartitionKey(0));
		}

		[HttpGet]
		public async Task<IEnumerable<ApiProduct>> Get(CancellationToken cancellationToken)
		{
			IEnumerable<Product> allProducts = await _catalogService.GetAllProducts();

			_logger.LogInformation("Start long process...");

			//for (var i = 0; i < 10; i++)
			//{
			cancellationToken.ThrowIfCancellationRequested();
			//	// slow non-cancellable work
			await Task.Delay(10_000);
			//}

			_logger.LogInformation("Finished long process...");

			return allProducts.Select(p => new ApiProduct
			{
				Id = p.Id,
				Name = p.Name,
				Description = p.Description,
				Price = p.Price,
				IsAvailable = p.Availability > 0
			});
		}

		[HttpPost]
		public async Task Post([FromBody] ApiProduct product)
		{
			var newProduct = new Product()
			{
				Id = Guid.NewGuid(),
				Name = product.Name,
				Description = product.Description,
				Price = product.Price,
				Availability = 100
			};

			await _catalogService.AddProduct(newProduct);
		}
	}
}
