namespace MQ.Business.Factory
{
    public interface IEntityFactory<out T>
    {
        T Get(IBinaryReader reader);
    }
}