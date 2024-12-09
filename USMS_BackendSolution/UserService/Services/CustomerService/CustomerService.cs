using BusinessObject;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using IRepository.ICustomerRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CustomerService
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService()
        {
            _customerRepository = new CustomerRepository();
        }
        #region Get All Customer
        /// <summary>
        /// Get All Customer in database
        /// </summary>
        /// <returns></returns>
        public APIResponse GetCustomers()
        {
            APIResponse aPIResponse = new APIResponse();
            List<CustomerDTO> customers = _customerRepository.getAllCustomer();
            if (customers == null)
            {
                aPIResponse.IsSuccess = true;
                aPIResponse.Message = "Customer List Empty";
            }
            aPIResponse.Result = customers;
            return aPIResponse;
        }
        #endregion

        #region GetCustomerByID
        /// <summary>
        /// Get Customer by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A customer by their ID</returns>
        public APIResponse GetCustomerById(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            CustomerDTO customer = _customerRepository.GetCustomerById(id);
            if (customer == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Customer with Id: " + id + " is not found";
            }
            aPIResponse.Result = customer;
            return aPIResponse;
        }
        #endregion

        #region Add new Customer
        /// <summary>
        /// Add New Customer to databse
        /// </summary>
        /// <param name="customer"></param>
        public APIResponse AddNewCustomer(CustomerDTO customer)
        {
            var existingCustomer = GetCustomerById(customer.Id);
            if (existingCustomer != null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Customer with the given ID already exists."
                };
            }
            bool isAdded = _customerRepository.AddNewCustomer(customer);
            if (isAdded)
            {
                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "Customer added successfully."
                };
            }
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Failed to add customer."
            };
        }
        #endregion

        #region Delete Customer
        /// <summary>
        /// Delete A Customer in Customer List
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public APIResponse DeleteCustomer(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            var existingCustomer = GetCustomerById(id);
            if (existingCustomer.Result == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Cannot found the customer with Id: " + id
                };
            }
            bool isRemoved = _customerRepository.DeleteCustomer(id);
            if (isRemoved)
            {
                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "Customer Removed successfully."
                };
            }
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Failed to Removed customer."
            };
        }
        #endregion
    }
}
