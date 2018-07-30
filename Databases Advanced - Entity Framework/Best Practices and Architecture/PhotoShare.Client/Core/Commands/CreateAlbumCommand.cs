namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Client.Utilities;
    using PhotoShare.Models.Enums;
    using Services.Contracts;


    public class CreateAlbumCommand : ICommand
    {
        private readonly IAlbumService albumService;
        private readonly IUserService userService;
        private readonly ITagService tagService;

        public CreateAlbumCommand(IAlbumService albumService, IUserService userService, ITagService tagService)
        {
            this.albumService = albumService;
            this.userService = userService;
            this.tagService = tagService;
        }

        // CreateAlbum <username> <albumTitle> <BgColor> <tag1> <tag2>...<tagN>
        public string Execute(string[] data)
        {
            var username = data[0];
            var albumTitle = data[1];
            var color = data[2];
            var tags = data.Skip(3).ToArray();

            var userExist = this.userService.Exists(username);

            if (!userExist)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            var albumExist = this.albumService.Exists(albumTitle);

            if (albumExist)
            {
                throw new ArgumentException($"Album {albumTitle} exists!");
            }

            var isValidColor = Enum.TryParse(color, out Color result);

            if (!isValidColor)
            {
                throw new ArgumentException($"Color {color} not found!");
            }

            for (int i = 0; i < tags.Length; i++)
            {
                tags[i] = tags[i].ValidateOrTransform();

                var currentTag = this.tagService.Exists(tags[i]);

                if (!currentTag)
                {
                    throw new ArgumentException("Invalid tags!");
                }

                var userId = this.userService.ByUsername<UserDto>(username).Id;

                this.albumService.Create(userId, albumTitle, color, tags);
            }

            return $"Album {albumTitle} successfully created!";

        }

        private object UserDto(object userId)
        {
            throw new NotImplementedException();
        }
    }
}
