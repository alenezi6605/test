
namespace TestsService.Domain.Seed
{

    public abstract class AuditEntity<T> : Entity<T>
    {
        private DateTime? _created;

        public DateTime CreatedAt
        {
            get
            {
                if (!_created.HasValue)
                    _created = DateTime.Now;

                return _created.Value;
            }
            set
            {
                _created = value;
            }
        }
    }
}
