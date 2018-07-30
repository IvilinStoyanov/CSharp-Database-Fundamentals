namespace PhotoShare.Models
{
    using Models.Enums;

    public class AlbumRole
    {
        public int UserId { get; set; }
        public UserFriendDto User { get; set; }

        public int AlbumId { get; set; }
        public Album Album { get; set; }

        public Role Role { get; set; }
    }
}
