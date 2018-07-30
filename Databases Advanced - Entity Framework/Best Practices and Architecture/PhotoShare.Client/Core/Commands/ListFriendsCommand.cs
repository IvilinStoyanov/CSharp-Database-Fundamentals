

using PhotoShare.Client.Core.Contracts;
using PhotoShare.Data;
using PhotoShare.Models;
using PhotoShare.Services.Contracts;
using System;
using System.Linq;

namespace PhotoShare.Client.Core.Commands
{
    public class ListFriendsCommand : ICommand
    {
        private readonly string FriendsListTitle = $"Friends: {Environment.NewLine}- ";

        private PhotoShareContext context;
        private readonly IUserService userService;

        public ListFriendsCommand(PhotoShareContext context, IUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        // ListFriends <username>
        public string Execute(string[] data)
        {
            var username = data[0];

            var userExist = this.userService.Exists(username);
            if(!userExist)
            {
                throw new ArgumentException(string.Format("User {0} not found", username));
            }

            var user = this.context.Users
                .Select(u => new
                {
                    u.Username,
                    FriendsAdded = u.FriendsAdded
                        .Select(f => f.Friend.Username)
                        .ToArray()
                })
                .SingleOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            var friends = user.FriendsAdded;


            return friends.Any()
                ? FriendsListTitle + string.Join($"{Environment.NewLine}- ", friends)
                : "No friends for this user.";
        }
    }
}
