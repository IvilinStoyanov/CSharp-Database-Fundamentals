namespace PhotoShare.Models
{
    using System.Collections.Generic;

    public class Town
    {
        public Town()
        {
            this.UsersBornInTown = new HashSet<UserFriendDto>();
            this.UsersCurrentlyLivingInTown = new HashSet<UserFriendDto>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public ICollection<UserFriendDto> UsersBornInTown { get; set; }

        public ICollection<UserFriendDto> UsersCurrentlyLivingInTown { get; set; } 
    }
}
