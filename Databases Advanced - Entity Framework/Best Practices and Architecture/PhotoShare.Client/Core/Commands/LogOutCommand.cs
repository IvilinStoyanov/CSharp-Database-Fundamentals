using PhotoShare.Client.Core.Contracts;
using PhotoShare.Services;
using PhotoShare.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoShare.Client.Core.Commands
{
    public class LogOutCommand : ICommand
    {
        private IUserService userService;

        public LogOutCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(string[] data) => this.userService.Logout();
    }
}
