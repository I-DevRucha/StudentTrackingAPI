using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JopPortalAPI.DataAccess.Repository
{
    public interface IDataRepository<TEntity, TDto>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        TEntity Get(long id);
        TDto GetDto(long id);
        TEntity Add(TEntity entity);
        void Update(TEntity entityToUpdate, TEntity entity);
        //TODO
        //void Update(TEntity entity);
        //void Delete(TEntity entity);
        bool Delete(int id);
    }
}
