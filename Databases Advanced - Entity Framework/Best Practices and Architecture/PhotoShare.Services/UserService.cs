using AutoMapper.QueryableExtensions;
using PhotoShare.Data;
using PhotoShare.Models;
using PhotoShare.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotoShare.Services
{
    public class UserService : IUserService
    {
        private const string AuthorizationFailedExceptionMessage = "You have to LogIn first!";
        private const string FailedLoginExceptionMessage = "Invalid username or password!";
        private const string AlreadyLoggedInExceptionMessage = "You should logout first!";
        private const string AnonimousLogOutExceptionMessage = "You should log in first in order to logout.";

        private const string SuccessfulLogOut = "User {0} successfully logged out!";

        private const int MinutesSinceLastActivityToLogout = 30;

        public User user;
        private DateTime? lastTimeOnline;


        private readonly PhotoShareContext context;

        public UserService(PhotoShareContext context)
        {
            this.context = context;
        }

        public TModel ById<TModel>(int id)
               => this.By<TModel>(x => x.Id == id).SingleOrDefault();

        public TModel ByUsername<TModel>(string username)
               => this.By<TModel>(x => x.Username == username).SingleOrDefault();

        public bool Exists(int id)
               => this.ById<User>(id) != null;
            
        public bool Exists(string name)
                => this.ByUsername<User>(name) != null;

        public Friendship AcceptFriend(int userId, int friendId)
        {
            var friendship = new Friendship
            {
                UserId = userId,
                FriendId = friendId
            };

            this.context.Friendships.Add(friendship);

            this.context.SaveChanges();

            return friendship;
    }

        public Friendship AddFriend(int userId, int friendId)
        {
            var friendship = new Friendship
            {
                UserId = userId,
                FriendId = friendId
            };

            this.context.Friendships.Add(friendship);

            this.context.SaveChanges();

            return friendship;
        }

        public void ChangePassword(int userId, string password)
        {
            var user = this.ById<User>(userId);

            user.Password = password;

            this.context.SaveChanges();
        }

        public void Delete(string username)
        {
            var user = this.ByUsername<User>(username);

            user.IsDeleted = true; 

           // this.context.Users.Remove(user);

            this.context.SaveChanges();
        }

        public User Register(string username, string password, string email)
        {
            var user = new User()
            {
                Username = username,
                Password = password,
                Email = email,
                IsDeleted = false      
            };

            this.context.Users.Add(user);

            this.context.SaveChanges();

            return user;
        }

        public void SetBornTown(int userId, int townId)
        {
            var user = this.ById<User>(userId);

            user.BornTownId = townId;

            this.context.SaveChanges();
        }

        public void SetCurrentTown(int userId, int townId)
        {
            var user = this.ById<User>(userId);

            user.CurrentTownId = townId;

            this.context.SaveChanges();
        }

        //Login Session
        public User User
        {
            get
            {
                if (this.user != null)
                {
                    var timeSpan = this.lastTimeOnline - DateTime.Now;
                    if (timeSpan.Value.Minutes > MinutesSinceLastActivityToLogout)
                    {
                        this.user = null;
                    }
                    else
                    {
                        this.lastTimeOnline = DateTime.Now;
                    }
                }

                return this.user;
            }

            private set
            {
                this.user = value;
                this.lastTimeOnline = DateTime.Now;
            }
        }

        public bool IsLoggedIn => this.User != null;

        public User Login(string username, string password)
        {
            if (this.IsLoggedIn)
            {
                throw new ArgumentException(AlreadyLoggedInExceptionMessage);
            }

            this.User = context.Users
                .SingleOrDefault(u => u.Username == username && u.Password == password);

            if (this.User == null)
            {
                throw new ArgumentException(FailedLoginExceptionMessage);
            }

            return this.User;
        }

        public string Logout()
        {
            if (!this.IsLoggedIn)
            {
                throw new InvalidCastException(AnonimousLogOutExceptionMessage);
            }

            var username = this.User.Username;
            this.User = null;

            return string.Format(SuccessfulLogOut, username);
        }

        public void Authorize()
        {
            if (!this.IsLoggedIn)
            {
                throw new InvalidOperationException(AuthorizationFailedExceptionMessage);
            }
        }

        private IEnumerable<TModel> By<TModel>(Func<User, bool> predicate)
            => this.context.Users
            .Where(predicate)
            .AsQueryable()
            .ProjectTo<TModel>();
    }
}