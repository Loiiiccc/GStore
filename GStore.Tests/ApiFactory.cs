using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;

namespace GStore.Tests
{
    public class ApiFactory : WebApplicationFactory<IApiAssemblyMarker>
    {
    }
}
