using Application.Interfaces;

namespace Application.Features.Common
{
    public abstract class BaseHandler
    {
        protected readonly IApplicationDbContext _context;

        public BaseHandler(IApplicationDbContext context)
        {
            _context = context;
        }
    }
}
