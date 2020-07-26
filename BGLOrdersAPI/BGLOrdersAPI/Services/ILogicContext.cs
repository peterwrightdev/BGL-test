using BGLOrdersAPI.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BGLOrdersAPI.Services
{
    public interface ILogicContext<T, U> where U: IBaseReport
    {
        bool Validate(T item);

        T ConvertReportToModel(U report);
    }
}
