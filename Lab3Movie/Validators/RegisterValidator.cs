using Lab3Movie.Models;
using Lab3Movie.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3Movie.Validators
{

    public interface IRegisterValidator
    {
        ErrorsCollection Validate(RegisterPostModel registerPostModel, MoviesDbContext context);
    }

    public class RegisterValidator : IRegisterValidator
    {
        public ErrorsCollection Validate(RegisterPostModel registerPostModel, MoviesDbContext context)
        {
            ErrorsCollection errorsCollection = new ErrorsCollection { Entity = nameof(RegisterPostModel) };

            User existing = context.Users.FirstOrDefault(u => u.Username == registerPostModel.UserName);

            if (existing != null)
            {
                errorsCollection.ErrorMessages.Add($"The username {registerPostModel.UserName} is already taken!");
            }

            if (registerPostModel.Password.Length < 6)
            {
                errorsCollection.ErrorMessages.Add("The password cannot be shorter than 6 characters!");
            }

            int nrOfDigits = 0;
            foreach (char c in registerPostModel.Password)
            {
                if (c >= '0' && c <= '9')
                {
                    nrOfDigits++;
                }
            }

            if (nrOfDigits < 2)
            {
                errorsCollection.ErrorMessages.Add("The password must contain at least two digits!");
            }

            if (errorsCollection.ErrorMessages.Count > 0)
            {
                return errorsCollection;
            }
            return null;

        }
    }
}