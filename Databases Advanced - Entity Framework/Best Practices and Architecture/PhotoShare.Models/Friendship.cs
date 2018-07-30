namespace PhotoShare.Models
{
    public class Friendship
    {
        public int UserId { get; set; }

        public UserFriendDto User { get; set; }

        public int FriendId { get; set; }

        public UserFriendDto Friend { get; set; }
    }
}
