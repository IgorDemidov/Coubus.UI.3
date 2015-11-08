using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adam.Core;
using Adam.Core.DataMapper;
using Adam.Core.Search;

namespace Coub.Domain.ConcreteRepositories
{
    public class RepositoryBase<T>
       where T : ExtendedItemBase
    {
        protected Application app;

        public RepositoryBase(Application app)
        {
            this.app = app;
        }

        public T Get(Guid id)
        {
            T item = Adam.Tools.Activator.CreateInstance<T>(app);
            item.TryLoad(id);
            return item;
        }
    }
}
