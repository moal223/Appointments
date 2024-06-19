using Appointement.Models;

namespace Appointement.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment> Add(Appointment entity);
        Task<Appointment> Update(Appointment entity);
        Task Delete(int id);
        Task<List<Appointment>> GetAll();
        Task<Appointment> Get(int id);
        Task<List<Appointment>> GetByTitle(string title, string userId);
        Task<List<Appointment>> GetByUserIdAsync(string userId);
    }
}
