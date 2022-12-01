using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using ProjetoFinal.Data;

namespace ProjetoFinal.Repositorio
{
    public class RepositorioBase<Model, Key> : IRepositorio<Model, Key> where Model : class
    {

        private readonly DbSet<Model> _dbSet;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RepositorioBase(ApplicationDbContext context)
        {
            _context = context;
            _mapper = new MapperConfiguration(c => c.CreateMap<Model, Model>()).CreateMapper();

            //Busca a propriedade do modelo.
            var contextType = typeof(ApplicationDbContext);
            var dbSetProperty = contextType
                .GetProperties()
                .Where(a => a.PropertyType == typeof(DbSet<Model>))
                .First();
            //Busca o dataset.
            if (dbSetProperty.GetValue(_context) is not DbSet<Model> dbSet)
                throw new System.Exception("Data model access not set in data context.");
            _dbSet = dbSet;
        }

        public void Adicionar(Model Item)
        {
            _dbSet.Add(Item);
            _context.SaveChanges();
        }

        public Result Atualizar(Key Id, Model Item)
        {
            var itemDb = _dbSet.Find(Id);
            if(itemDb == null) return Result.Fail("Item não encontrado.");

            _mapper.Map(Item, itemDb);
            _context.SaveChanges();

            return Result.Ok();
        }

        public Model? Buscar(Key Id)
        {
            return _dbSet.Find(Id);
        }

        public Result Deletar(Key Id)
        {
            var item = Buscar(Id);
            if (item == null) return Result.Fail("Item não encontrado");

            _dbSet.Remove(item);
            _context.SaveChanges();

            return Result.Ok();
        }

        public IQueryable<Model> Listar()
        {
            return _dbSet.AsQueryable();
        }
    }
}
