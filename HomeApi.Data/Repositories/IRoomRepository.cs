using HomeApi.Data.Models;
using HomeApi.Data.Queries;

namespace HomeApi.Data.Repositories
{
    public interface IRoomRepository
    {
        Task<Room> GetRoomByName(string name);
        Task<Room> GetRoomById(Guid id);
        Task AddRoom(Room room);
        Task UpdateRoom(Room room, UpdateRoomQuery query);
    }
}
