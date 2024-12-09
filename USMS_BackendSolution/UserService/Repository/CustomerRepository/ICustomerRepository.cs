using BusinessObject.ModelDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository.ICustomerRepository
{
    public interface ICustomerRepository
    {
        public List<CustomerDTO> getAllCustomer();
        public CustomerDTO GetCustomerById(string id);
        public bool AddNewCustomer(CustomerDTO customerDTO);
        public bool DeleteCustomer(string id);
    }
}
