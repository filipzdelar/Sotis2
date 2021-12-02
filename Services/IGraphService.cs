using Sotis2.Models.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sotis2.Services
{
    public interface IGraphService
    {
        Graph GetGraphSchema(string schemaName);
    }
}
