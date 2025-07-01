using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context): base(context) { }

        public IEnumerable<Produto> GetProdutoPorCategoria(int id)
        {
            return GetAll().Where(c => c.CategoriaId == id);
        }
    }
}
