using BusinessObject.AppDBContext;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository.ICustomerRepository
{
    public class CustomerRepository : ICustomerRepository
    {
        /// <summary>
        /// Get All Customer in Database
        /// </summary>
        /// <returns>List of Customer exsist in Database</returns>
        public List<CustomerDTO> getAllCustomer()
        {
            try
            {
                var dbContext = new MyDbContext();
                List<Customer> customers = dbContext.Customer.ToList();
                List<CustomerDTO> customersDTOs = new List<CustomerDTO>();
                foreach (var customer in customers)
                {
                    CustomerDTO customerDTO = new CustomerDTO();
                    customerDTO.CopyProperties(customer);
                    customersDTOs.Add(customerDTO);
                }
                return customersDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }
        }

        /// <summary>
        /// Retrieve a Customer by their ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns> a Customer by their ID </returns>
        /// <exception cref="Exception"></exception>
        public CustomerDTO GetCustomerById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "Customer ID cannot be null or empty.");
            }
            try
            {
                var customers = getAllCustomer();
                CustomerDTO customerDTO = customers.FirstOrDefault(x => x.Id == id);
                return customerDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///  Adds a new customer to the database.
        /// </summary>
        /// <param name="customerDTO"></param>
        /// <exception cref="Exception"></exception>
        public bool AddNewCustomer (CustomerDTO customerDTO)
        {
            try
            {
                var dbContext = new MyDbContext();
                Customer customer = new Customer();
                customer.CopyProperties(customerDTO);
                dbContext.Customer.Add(customer);
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Detele Customer by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A new list without the customer just deleted</returns>
        public bool DeleteCustomer(string id)
        {
            try
            {
                var dbContext = new MyDbContext();
                var existingCustomer = GetCustomerById(id);
                Customer customer = new Customer();
                customer.CopyProperties(existingCustomer);
                dbContext.Customer.Remove(customer);             
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
