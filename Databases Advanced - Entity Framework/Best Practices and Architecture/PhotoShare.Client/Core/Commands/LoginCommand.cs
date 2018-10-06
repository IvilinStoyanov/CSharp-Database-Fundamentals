using PhotoShare.Client.Core.Contracts;
using PhotoShare.Services;
using PhotoShare.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoShare.Client.Core.Commands
{
    public class LoginCommand : ICommand
    {
        private const string DeletedUserExceptionMessage = "Invalid username or password!";
        private const string MultipleLoginExceptionMessage = "You are already logged in as {0}!";

        private const string SuccessfulLogIn = "User {0} successfully logged in!";

        private readonly IUserService userService;

        public LoginCommand(IUserService userService)
        {
            this.userService = userService;
        }

        // Login <username> <password>
        public string Execute(string[] data)
        {
            var username = data[0];
            var password = data[1];

            if (this.userService.User != null &&
                this.userService.User.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(string.Format(MultipleLoginExceptionMessage, username));
            }

            var user = this.userService.Login(username, password);
            if (user.IsDeleted.Value)
            {
                this.userService.Logout();
                throw new InvalidOperationException(DeletedUserExceptionMessage);
            }

            return string.Format(SuccessfulLogIn, username);
        }
    }
}



