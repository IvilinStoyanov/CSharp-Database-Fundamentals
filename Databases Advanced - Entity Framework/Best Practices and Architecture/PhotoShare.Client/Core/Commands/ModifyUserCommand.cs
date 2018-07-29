namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Services.Contracts;

    public class ModifyUserCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly ITownService townService;

        public ModifyUserCommand(IUserService userService, ITownService townService)
        {
            this.userService = userService;
            this.townService = townService;
        }

        // ModifyUser <username> <property> <new value>
        // For example:
        // ModifyUser <username> Password <NewPassword>
        // ModifyUser <username> BornTown <newBornTownName>
        // ModifyUser <username> CurrentTown <newCurrentTownName>
        // !!! Cannot change username
        public string Execute(string[] data)
        {
            string username = data[0];
            string property = data[1];
            string value = data[2];

            var userExist = this.userService.Exists(username);

            if(!userExist)
            {
                throw new ArgumentException($"User {username} not found");  
            }

            var userId = this.userService.ByUsername<UserDto>(username).Id;

            if (property == "Password")
            {
                SetPassowrd(userId, value);
            }
            else if(property == "BornTown")
            {
                SetBornTown(userId, value);
            }
            
        }

        private void SetBornTown(int userId, string name)
        {
            var isTownExist = this.townService.Exists(name);
        }

        private void SetPassowrd(int userId, string value)
        {
           var isValidPassword = value.Any(x => char.IsLower(x) && char.IsDigit(x));

            if(!isValidPassword)
            {
                throw new ArgumentException($"Value {value} not valid/n Invalid Password");
            }

            this.userService.ChangePassword(userId, value);
        }
    }
}
