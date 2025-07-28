using DAL.Models;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class AccountService
    {
        private readonly CustomerRepo _repo = new();

        public bool Login(string email, string password)
        {
            var customer = _repo.GetByEmail(email);
            return customer != null && customer.Password == password;
        }
        

        public bool Register(string name, string email, string password, out string message)
        {
            if (_repo.GetByEmail(email) != null)
            {
                message = "Email already exists.";
                return false;
            }

            var newCustomer = new Customer { Name = name, Email = email, Password = password };
            _repo.Add(newCustomer);
            message = "Registered successfully!";
            return true;
        }

        public bool ChangePassword(string email, string oldPass, string newPass, out string message)
        {
            var customer = _repo.GetByEmail(email);
            if (customer == null || customer.Password != oldPass)
            {
                message = "Old password is incorrect.";
                return false;
            }

            customer.Password = newPass;
            _repo.Update(customer);
            message = "Password updated successfully.";
            return true;
        }

        public Customer? GetByEmail(string email) => _repo.GetByEmail(email);
    }
}