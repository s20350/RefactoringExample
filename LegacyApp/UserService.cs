using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            //Walidacja dla imienia, nazwiska, maila oraz daty urodzenia zostala zrealizowana za pomoca metod: IsUserValid oraz IsUserOfLegalAge i sa one wywolywane w tym miejscu
            if (!IsUserValid(firstName, lastName, email) || !IsUserOfLegalAge(dateOfBirth))
            {
                return false;
            }
            
            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
            
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    creditLimit = creditLimit * 2;
                    user.CreditLimit = creditLimit;
                }
            }
            else
            {
                user.HasCreditLimit = true;
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }
            
            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }
        
        private bool IsUserValid(string firstName, string lastName, string email)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || !email.Contains("@") ||
                !email.Contains(".")) {
                return false;
            }

            return true;
        }
        
        private bool IsUserOfLegalAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth > today.AddYears(-age)) age--;
            return age >= 21;
        }

        
        
    }
}
