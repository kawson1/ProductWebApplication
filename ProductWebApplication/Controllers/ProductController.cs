using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProductWebApplication.Components;
using ProductWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductWebApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _productApiServiceUrl;
        public readonly ErrorHandler errorHandler;

        public ProductController(IHttpClientFactory httpClientFactory, IConfiguration configuration) 
        { 
            _httpClient  = httpClientFactory.CreateClient();

            _productApiServiceUrl = Environment.GetEnvironmentVariable("SERVICE_URL");
            if (string.IsNullOrEmpty(_productApiServiceUrl))
                _productApiServiceUrl = configuration.GetValue<string>("ExternalServices:ProductApiServiceUrl");

            errorHandler = new ErrorHandler();
        }

        // GET: ProductController
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var response = await _httpClient.GetAsync(_productApiServiceUrl + "/api/Product");
            string responseBody = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<Product>>(responseBody);

            await Task.Delay(100);
            errorHandler.Error($"Test error message in Index action");
            errorHandler.NotError($"Test message in Index action");
            return View(products);
        }

        // GET: ProductController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync(_productApiServiceUrl + "/api/Product/" + id);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var p = JsonConvert.DeserializeObject<Product>(responseBody);

                return View(p);
            }
            errorHandler.Error($"Product with id: { id} not found.");
            return StatusCode(520, $"Product with id: {id} not found.");
        }

        // GET: ProductController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string name, string description, float value)
        {
            Product product = new Product()
            {
                Name = name,
                Description = description,
                Value = value
            };
            var httpContent = JsonContent.Create(product);
            var response = await _httpClient.PostAsync(_productApiServiceUrl + "/api/Product", httpContent);

            if(response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var p = JsonConvert.DeserializeObject<Product>(responseBody);
                return View($"Details", p);
            }
            return View("Error");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync(_productApiServiceUrl + "/api/Product/" + id);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var p = JsonConvert.DeserializeObject<Product>(responseBody);

                return View(p);
            }
            errorHandler.Error($"Product with id: {id} not found.");
            return StatusCode(520, $"Product with id: {id} not found.");
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Product product)
        {
            var httpContent = JsonContent.Create(product);
            var response = await _httpClient.PutAsync(_productApiServiceUrl + "/api/Product/" + id, httpContent);
            
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var p = JsonConvert.DeserializeObject<Product>(responseBody);
                return View($"Details", p);
            }
            return View("Error");
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync(_productApiServiceUrl + "/api/Product/" + id);
            return View();
        }

        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }
    }
}
