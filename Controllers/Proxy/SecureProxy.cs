using doan.Controllers.AdapterAuthenticator;
using doan.Models;
using Microsoft.Extensions.Logging;
using System;

namespace doan.Controllers.Proxy
{
    public class SecureProxy : IAuthenticationAdapter
    {
    
            private readonly ILogger<SecureProxy> _logger;
            private readonly StoreContext _context;

            public SecureProxy(ILogger<SecureProxy> logger, StoreContext context)
            {
                _logger = logger;
                _context = context;
            }

        public Taikhoan Authenticate(string sdt, string pass)
        {
            try
            {
                // Add password hashing logic here
                // For example, using BCrypt.NET to hash the password before authentication
                string hashedPassword = HashPassword(pass);

                // Log the authentication attempt
                _logger.LogInformation("Attempting to authenticate user with phone number / username and pass: {0} :{1}", sdt, pass);


                // Call the actual authentication method from your original authentication adapter
                // This can be your existing authentication logic
                // For demonstration purposes, let's assume there's another adapter named OriginalAuthenticationAdapter
                AuthenticationAdapter adapter = new AuthenticationAdapter(_context);
                return adapter.Authenticate(sdt, hashedPassword);
            }
            catch (Exception e)
            {
                _logger.LogError("Error authenticating user with phone / username: {0}. Error: {1} with pass: {2}", sdt, e.Message, HashPassword(pass));
                return null;
            }
        }
        public string HashPassword(string password)
        {
            password = BCrypt.Net.BCrypt.HashPassword(password);
            return password;
        }
    }
}

