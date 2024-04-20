
namespace TestsService.Domain.Seed
{

    public abstract class Entity<T>
    {
        public virtual T? Id { get; set; }
    }

}