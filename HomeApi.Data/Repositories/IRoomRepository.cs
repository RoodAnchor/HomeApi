using HomeApi.Data.Models;

namespace HomeApi.Data.Repositories
{
    public interface IRoomRepository
    {
        Task<Room> GetRoomByName(string name);
        Task AddRoom(Room room);
    }
}
