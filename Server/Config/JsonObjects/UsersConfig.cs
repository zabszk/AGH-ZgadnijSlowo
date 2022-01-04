using System;
using System.Collections.Generic;
using Utf8Json;

namespace Server.Config.JsonObjects
{
    public readonly struct UsersConfig : IEquatable<UsersConfig>
    {
        public readonly Dictionary<string, User> Users;
        
        [SerializationConstructor]
        public UsersConfig(Dictionary<string, User> users)
        {
            Users = users;
        }

        public bool Equals(UsersConfig other)
        {
            return Equals(Users, other.Users);
        }

        public override bool Equals(object obj)
        {
            return obj is UsersConfig other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Users != null ? Users.GetHashCode() : 0);
        }

        public static bool operator ==(UsersConfig left, UsersConfig right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(UsersConfig left, UsersConfig right)
        {
            return !left.Equals(right);
        }
    }

    public class User : IEquatable<User>
    {
        public string Password;
        public Dictionary<string, UserRound> Score;
        public bool Suspended;
        public DateTime LastLogin;
        
        [SerializationConstructor]
        public User(string password, Dictionary<string, UserRound> score, bool suspended, DateTime lastLogin)
        {
            Password = password;
            Score = score;
            Suspended = suspended;
            LastLogin = lastLogin;
        }

        public User(OldJsonObjects.User u)
        {
            Password = u.Password;
            Score = new Dictionary<string, UserRound>(u.Score.Count);
            Suspended = u.Suspended;
            LastLogin = u.LastLogin;

            foreach ((string key, int value) in u.Score)
                Score.Add(key, new UserRound(value > 0 ? (uint) value : 0, 0));
        }

        public bool Equals(User other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Password == other.Password && Score == other.Score && Suspended == other.Suspended && LastLogin.Equals(other.LastLogin);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Password, Score, Suspended, LastLogin);
        }

        public static bool operator ==(User left, User right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(User left, User right)
        {
            return !Equals(left, right);
        }
    }

    public readonly struct UserRound : IEquatable<UserRound>
    {
        public readonly uint Score;
        public readonly uint Games;

        [SerializationConstructor]
        public UserRound(uint score, uint games)
        {
            Score = score;
            Games = games;
        }

        public bool Equals(UserRound other)
        {
            return Score == other.Score && Games == other.Games;
        }

        public override bool Equals(object obj)
        {
            return obj is UserRound other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Score, Games);
        }

        public static bool operator ==(UserRound left, UserRound right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(UserRound left, UserRound right)
        {
            return !left.Equals(right);
        }
        
        public static UserRound operator +(UserRound r, uint score) => new UserRound(r.Score + score, r.Games + 1);

        public override string ToString() => $"{Score} in {Games} {(Games == 1 ? "game" : "games")}, avg. {(Games == 0 ? "N/A" : $"{Score / (float)Games:F4}")}";
    }
}