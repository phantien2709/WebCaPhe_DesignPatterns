using doan.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Text.RegularExpressions;

namespace doan.Controllers.AdapterAuthenticator
{
    public class AuthenticationAdapter : IAuthenticationAdapter
    {
        private readonly StoreContext _context;


        public AuthenticationAdapter(StoreContext context)
        {
            _context = context;
        }
        
        public Taikhoan Authenticate(string identifier, string password)
        {

            if (IsValidPhoneNumber(identifier))
            {
                // Authenticate using phone number
                return _context.GetTaikhoan(identifier, password); //if true == phone number
            }
            else
            {
                // Authenticate using username
                return _context.GetTaikhoanByUsernameAndPassword(identifier, password); // if false == username
            }
        }
        private bool IsValidPhoneNumber(string identifier)
        {
            // Regular expression pattern to match only numbers
            string pattern = "^[0-9]*$";

            // Check if the identifier contains exactly 10 digits and consists only of numbers
            if ( Regex.IsMatch(identifier, pattern))
            {
                return true; // Phone number is valid
            }
            else
            {
                return false; // Phone number is invalid
            }
        }

        }
    }
