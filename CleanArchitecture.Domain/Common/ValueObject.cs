namespace CleanArchitecture.Domain.Common
{
    /*
     * The definition of a value object class is a basic requirement to implement DDD (domain driven design)
     * value object is immutable and unic, it is distinguished by the value of its properties.
     * for example now you have an address where you live, and if you want to move you don't update your
     * address you create/change it for a brand new address, and you may live with some other person
     * which means that the object value is reusable, by that we can say that a value object can be
     * linked to one or more entities of the client
     */
    public abstract class ValueObject
    {
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, right) || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObject left, ValueObject right)
        {
            return !(EqualOperator(left, right));
        }

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;

            return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
        // Other utility methods
    }
}
