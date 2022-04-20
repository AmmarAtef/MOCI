using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOCI.Services.Interfaces
{
    public interface IMappedColumnsService
    {
        List<string> GetColumnsNames();
    }
}
