using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TeamBuilder.Models.Enums;

namespace TeamBuilder.Models
{
    public class User
    {
        public User()
        {
            this.CreatedEvents = new List<Event>();
            this.UserTeams = new List<UserTeam>();
            this.CreatedUserTeams = new List<UserTeam>();
            this.ReceivedInvitations = new List<Invitation>();
        }

        [Key]
        public int Id { get; set; }

        [MinLength(3)]
        public string Username { get; set; }

        [StringLength(6 , MinimumLength = 30)]
        public string Password { get; set; }

        [MaxLength(25)]
        public string FirstName { get; set; }

        [MaxLength(25)]
        public string LastName { get; set; }

        public int Age { get; set; }

        public Gender Gender { get; set; }

        public bool IsDeleted { get; set; }

        // Navigation properties goes here
        public ICollection<Event> CreatedEvents { get; set; }

        public ICollection<UserTeam> UserTeams { get; set; }

        public ICollection<UserTeam> CreatedUserTeams { get; set; }

        public ICollection<Invitation> ReceivedInvitations { get; set; }




    }
}
