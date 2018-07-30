namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Contracts;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using PhotoShare.Models.Enums;
    using PhotoShare.Services;
    using PhotoShare.Services.Contracts;

    public class ShareAlbumCommand : ICommand
    {
        private const string AlbumNotFoundExceptionMessage = "Album {0} not found!";
        private const string UserNotFoundExceptionMessage = "User {0} not found!";
        private const string InvalidRoleExceptionMessage = "Permission must be either “Owner” or “Viewer”!";

        private readonly IUserService userService;
        private readonly IAlbumService albumService;
        private readonly IAlbumRoleService albumRoleService;
        private readonly PhotoShareContext context;

        public ShareAlbumCommand(PhotoShareContext context, IUserService userService, IAlbumService albumService, IAlbumRoleService albumRoleService)
        {
            this.context = context;
            this.userService = userService;
            this.albumService = albumService;
            this.albumRoleService = albumRoleService;

        }
        // ShareAlbum <albumId> <username> <permission>
        // For example:
        // ShareAlbum 4 dragon321 Owner
        // ShareAlbum 4 dragon11 Viewer
        public string Execute(string[] data)
        {
            var albumId = int.Parse(data[0]);
            var username = data[1];

            //check for user exist
            var userExist = this.userService.Exists(username);
            if(!userExist)
            {
                throw new ArgumentException(UserNotFoundExceptionMessage);
            }

            var albumInfo = this.albumService.ById<AlbumDto>(albumId);
            var user = this.userService.ByUsername<UserDto>(username);

            var permission = this.ParsePermission(data[2]);

            this.albumRoleService.PublishAlbumRole(albumId, user.Id, permission.ToString());

            return $"Username {user.Username} added to album {albumInfo.Name} ({permission.ToString()})";
        }

        private Role ParsePermission(string name)
        {
            Role role;
            var isRoleValid = Enum.TryParse(name, true, out role);
            if (!isRoleValid)
            {
                throw new ArgumentException(InvalidRoleExceptionMessage);
            }
            return role;
        }
    }
}

