using System;
using System.Collections.Generic;

namespace StackX.ServiceModel
{
    public class Role
    {
        protected bool Equals(Role other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Role) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public string Name { get; }

        public Role(string name)
        {
            Name = name;
        }
    }

    public class Permission
    {
        protected bool Equals(Permission other)
        {
            return string.Equals(Value, other.Value, StringComparison.InvariantCulture);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Permission) obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? StringComparer.InvariantCulture.GetHashCode(Value) : 0);
        }

        public string Value { get; }
        public string Description { get; }

        public Permission(string value, string description = null)
        {
            Value = value;
            Description = description ?? value;
        }
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Contributor = "Contributor";
        public const string Member = "Member";

        public static readonly List<Role> All = new List<Role>()
        {
            new Role(Admin),
            new Role(Contributor),
            new Role(Member)
        };
    }
}
