namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using PhotoShare.Services.Contracts;

    public class AddTagToCommand : ICommand
    {
        private const string UnexistentArgsExceptionMessage = "Either tag or album do not exist!";

        private const string SuccessfullAddition = "Tag {0} added to {1}!";

        private readonly ITagService tagService;
        private readonly PhotoShareContext context;

        public AddTagToCommand(ITagService tagService, PhotoShareContext context)
        {
            this.tagService = tagService;
            this.context = context;
        }

        // AddTagTo <albumName> <tag>
        public string Execute(string[] args)
        {
            var album = this.GetAlbum(args[0]);
            var tag = this.GetTag(args[1]);

            this.tagService.AddTag(tag.Name);
    
            return string.Format(SuccessfullAddition, tag.Name, album.Name);
        }

        private Tag GetTag(string tagName)
        {
            if (!tagName.StartsWith('#'))
            {
                tagName = $"#{tagName}";
            }

            var tag = this.context.Tags
                .SingleOrDefault(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase));

            if (tag == null)
            {
                throw new ArgumentException(UnexistentArgsExceptionMessage);
            }

            return tag;
        }

        private Album GetAlbum(string albumName)
        {
            var album = this.context.Albums
                .SingleOrDefault(a => a.Name.Equals(albumName, StringComparison.OrdinalIgnoreCase));

            if (album == null)
            {
                throw new ArgumentException(UnexistentArgsExceptionMessage);
            }

            return album;
        }
    }
}

