namespace PhotoShare.Client.Core.Commands
{
    using System;
    using Contracts;
    using PhotoShare.Services.Contracts;

    public class AddTagToCommand : ICommand
    {
        private readonly ITagService tagService;

        public AddTagToCommand(ITagService tagService)
        {
            this.tagService = tagService;
        }

        // AddTagTo <albumName> <tag>
        public string Execute(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
