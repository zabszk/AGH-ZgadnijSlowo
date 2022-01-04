using System;
using System.Collections.Generic;
using Utf8Json;

namespace Server.Config.JsonObjects.OldJsonObjects
{
    public readonly struct OldUsersConfig : IEquatable<OldUsersConfig>
    {
        public readonly Dictionary<string, User> Users;
        
        [SerializationConstructor]
        public OldUsersConfig(Dictionary<string, User> users)
        {
            Users = users;
        }

        public bool Equals(OldUsersConfig other)
        {
            return Equals(Users, other.Users);
        }

        public override bool Equals(object obj)
        {
            return obj is OldUsersConfig other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Users != null ? Users.GetHashCode() : 0);
        }

        public static bool operator ==(OldUsersConfig left, OldUsersConfig right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OldUsersConfig left, OldUsersConfig right)
        {
            return !left.Equals(right);
        }
    }

    public class User : IEquatable<User>
    {
        public string Password;
        public Dictionary<string, int> Score;
        public bool Suspended;
        public DateTime LastLogin;
        
        [SerializationConstructor]
        public User(string password, Dictionary<string, int> score, bool suspended, DateTime lastLogin)
        {
            Password = password;
            Score = score;
            Suspended = suspended;
            LastLogin = lastLogin;
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
}