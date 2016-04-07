using MQ.Business;

namespace MQ.Cqrs.Factory
{
    public interface IEntityFactory<out T>
    {
        T Get(IBinaryReader reader);
    }
}