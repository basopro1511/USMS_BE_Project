using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.AppDBContext;
using BusinessObject.Models;
using BusinessObject.ModelDTOs;
using DataAccess.Services.CustomerService;
using BusinessObject;

namespace UserService.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService _customerService;
        public CustomersController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/Customers
        [HttpGet]
        public APIResponse GetAllCustomers()
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _customerService.GetCustomers();
            return aPIResponse;
        }
        // GET: api/Customers/1
        [HttpGet("{id}")]
        public APIResponse GetCustomerById(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _customerService.GetCustomerById(id);
            return aPIResponse;
        }
        // POST: api/Customer
        [HttpPost]
        public APIResponse AddNewCustomer(CustomerDTO customerDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _customerService.AddNewCustomer(customerDTO);
            return aPIResponse;
        }
        [HttpDelete]
        public APIResponse DeleteCustomer(string id)
        {
            APIResponse aPIResponse = aPIResponse = _customerService.DeleteCustomer(id);
            return aPIResponse;
        }
    }
}
