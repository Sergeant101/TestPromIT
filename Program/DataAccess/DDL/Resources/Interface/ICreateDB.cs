
namespace DDL.Resources.Interface
{
    public interface ICreateDB
    {
        public ValueTask<int> CreateDB();
        public string GetName {get; }
    }

}