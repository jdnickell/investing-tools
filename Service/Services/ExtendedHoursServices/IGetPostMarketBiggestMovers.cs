using Service.Services.AfterHoursServices.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Services.ExtendedHoursServices
{
    public interface IGetPostMarketBiggestMovers
    {
        /// <summary>
        /// Get <see cref="DailyOpenCloseResult"/> for symbols that increased/decreased more than the specified percentage in afterhours.
        /// </summary>
        /// <param name="openCloseDate"></param>
        /// <returns></returns>
        Task<List<DailyOpenCloseResult>> GetListAsync(string openCloseDate);
    }
}
