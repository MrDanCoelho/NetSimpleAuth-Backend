using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetPOC.Backend.Domain.Interfaces.IRepositories
{
    public interface ICrudRepository<T> where T : class
    {
        /// <summary>
        /// Serviço de coleta de todos os objetos do tipo indicado
        /// </summary>
        /// <returns>Lista de objetos do tipo indicado</returns>
        Task<IEnumerable<T>> GetAll();
        
        /// <summary>
        /// Serviço de coleta de objeto do tipo indicado por ID
        /// </summary>
        /// <param name="id">ID do objeto a ser coletado</param>
        /// <returns>Objeto do tipo indicado</returns>
        Task<T> GetById(object id);
        
        /// <summary>
        /// Serviço de inserção de objeto do tipo indicado
        /// </summary>
        /// <param name="obj">Objeto do tipo indicado a ser inserido</param>
        Task Insert(T obj);
        
        /// <summary>
        /// Serviço de atualização de objeto do tipo indicado
        /// </summary>
        /// <param name="obj">Objeto do tipo indicado a ser atualizado</param>
        void Update(T obj);
        
        /// <summary>
        /// Serviço de exclusão de objeto do tipo indicado de acordo com seu ID
        /// </summary>
        /// <param name="obj">Objeto do tipo indicado a ser excluído</param>
        void Delete(T obj);
        
        /// <summary>
        /// Serviço de comprometimento de transações do repositório
        /// </summary>
        Task Save();
    }
}