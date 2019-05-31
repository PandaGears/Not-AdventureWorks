using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WTCPortal.Controllers;
using WTCPortal.Models;
using WTCPortal.Repository;

namespace WTCPortal.ViewModel
{
    public class UserPageViewModel
    {
        private const string ConnectionString = "AdventureWorks";
        UserController _controller = new UserController();
        User _user = new User();

        public UserPageViewModel()
        {

        }

        public UserPageViewModel(UserController controller)
        {
            _controller = controller;
        }

        public void Registration(User user)
        {
            try
            {
                UserRepository repo = new UserRepository(ConnectionString);
                repo.InsertUser(user);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}