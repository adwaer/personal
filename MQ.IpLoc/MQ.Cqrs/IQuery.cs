namespace MQ.Cqrs
{
    public interface IQuery<in TPar, in TPar1, in TPar2, out T> : IQuery
    {
        T Execute(TPar parameter, TPar1 par1, TPar2 par2);
    }
    public interface IQuery<in TPar, in TPar1, out T> : IQuery
    {
        T Execute(TPar parameter, TPar1 par1);
    }
    public interface IQuery<in TPar, out T> : IQuery
    {
        T Execute(TPar parameter);
    }

    public interface IQuery<out T> : IQuery
    {
        T Execute();
    }

    public interface IQuery
    {

    }
}
