using WTCPortal.Models;
using System;
using System.Data.SqlClient;


namespace WTCPortal.Repository
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void InsertUser(User user)
        {
            var result = 0;
            using (SqlCommand command = new SqlCommand("Person.UserRegistration"))
            {
                command.Parameters.Add(new SqlParameter("@PersonType", user.PersonType));
                command.Parameters.Add(new SqlParameter("@NameStyle", user.NameStyle));
                command.Parameters.Add(new SqlParameter("@Title", user.Title));
                command.Parameters.Add(new SqlParameter("@FirstName", user.FirstName));
                command.Parameters.Add(new SqlParameter("@MiddleName", user.MiddleName));
                command.Parameters.Add(new SqlParameter("@LastName", user.LastName));
                command.Parameters.Add(new SqlParameter("@Suffix", user.Suffix));
                command.Parameters.Add(new SqlParameter("@EmailPromotion", user.EmailPromotion));
                command.Parameters.Add(new SqlParameter("@EmailAddress", user.EmailAddress.EmailAddress1));
                command.Parameters.Add(new SqlParameter("@PasswordHash", user.PassThe));
                command.Parameters.Add(new SqlParameter("@PasswordSalt", user.Salt));

                result = ExecuteCommand(command);
                if ((result == -99) || (result == -1000))
                    throw new Exception("Whats going on!?");
            }
        }

    }
}