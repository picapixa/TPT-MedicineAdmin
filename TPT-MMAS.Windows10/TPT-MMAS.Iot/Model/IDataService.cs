using System.Threading.Tasks;

namespace TPT_MMAS.Iot.Model
{
    public interface IDataService
    {
        Task<DataItem> GetData();
    }
}