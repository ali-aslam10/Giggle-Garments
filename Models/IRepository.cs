namespace Giggle_Garments_MVC.Models
{
    public interface IRepository<TEntity>
    {
        void Add(TEntity entity);
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();
        void Update(TEntity entity);
        void Delete(int id);
    }
}
