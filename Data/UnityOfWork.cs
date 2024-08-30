using Todo_List_API.Data.Contexts;

namespace Todo_List_API.Data
{
    public class UnityOfWork
    {
        private readonly AppDbContext _context;
        private readonly SecurityDbContext _securityDbContext;
        public UnityOfWork(AppDbContext context, SecurityDbContext securityDbContext)
        {
            _context = context;
            _securityDbContext = securityDbContext;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Rollback()
        {
            _context.Dispose();
        }

        public void CommitSecurity()
        {
            _securityDbContext.SaveChanges();
        }

        public void RollbackSecurity()
        {
            _securityDbContext.Dispose();
        }
    }
}
