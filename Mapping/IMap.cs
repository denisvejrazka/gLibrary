using gLibrary.Models;

namespace gLibrary.Mapping
{
    public interface IMap
    {
        Cell GetMap(int value, int row, int col);
    }   
}
