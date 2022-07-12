namespace Bets.Domain.Exceptions
{
    public class UnknownEntityException : Exception
    {
        public UnknownEntityException(Type entityType, object key)
        {
            EntityType = entityType;
            Key = key;
        }

        public Type EntityType { get; }
        public object Key { get; }
    }
}
