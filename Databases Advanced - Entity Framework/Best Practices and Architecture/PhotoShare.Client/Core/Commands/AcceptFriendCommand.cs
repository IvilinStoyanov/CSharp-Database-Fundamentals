namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Services.Contracts;

    public class AcceptFriendCommand : ICommand
    {
        private readonly IUserService userService;

        public AcceptFriendCommand(IUserService userService)
        {
            this.userService = userService;
        }

        // AcceptFriend <username1> <username2>
        public string Execute(string[] data)
        {
            string acceptingUser = data[0];
            string suggesterUser = data[1];

            var userExist = this.userService.Exists(acceptingUser);
            var friendExist = this.userService.Exists(suggesterUser);

            if (!userExist)
            {
                throw new ArgumentException($"{acceptingUser} not found!");
            }

            if (!friendExist)
            {
                throw new ArgumentException($"{suggesterUser} not found!");
            }

            var userToAccpet = this.userService.ByUsername<UserFriendsDto>(acceptingUser);
            var userSuggested = this.userService.ByUsername<UserFriendsDto>(suggesterUser);

           bool isSendAcceptedFromUser = userToAccpet.Friends.Any(x => x.Username == userSuggested.Username);
           bool isSendRequestFromFriend = userSuggested.Friends.Any(x => x.Username == userToAccpet.Username);

            if (isSendAcceptedFromUser)
            {
                throw new ArgumentException($"{userSuggested.Username} is already a friend to {userToAccpet.Username}");
            }

            if (!isSendRequestFromFriend)
            {
                throw new ArgumentException($"{userSuggested.Username} has not added {userToAccpet.Username} as a friend");
            }

            this.userService.AcceptFriend(userToAccpet.Id, userSuggested.Id);

            return $"{userSuggested.Username} accepted {userToAccpet.Username} as a friend";
        }
    }
}
