using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adam.Core;
using Adam.Core.Classifications;
using Adam.Core.Records;
using Adam.Core.Search;

namespace Coub.Domain.ConcreteRepositories
{
    public class RecordRepository: RepositoryBase<Record>
    {
        public RecordRepository(Application app) 
            : base(app)
        {
        }

        public RecordCollection GetRecordCollection(SearchExpression expression)
        {
            RecordCollection collection = new RecordCollection(app);
            try
            {
                collection.Load(expression);
            }
            catch (Exception)
            {
                // ignored
            }

            return collection;
        }

        public RecordCollection GetRecordCollectionByClassificationNamePath(string classificationNamePath)
        {
            SearchExpression expression = new SearchExpression("Classification.NamePath = '" + classificationNamePath + "'");
            return GetRecordCollection(expression);
        }

        

        
    }
}
