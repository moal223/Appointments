using Appointement.Data;
using Appointement.Interfaces;
using Appointement.Models;
using Microsoft.EntityFrameworkCore;

namespace Appointement.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;
        public AppointmentRepository(AppDbContext context) {
            _context = context;
        }
        public async Task<Appointment> Add(Appointment entity)
        {
            await _context.Appointments.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0 ? entity : null;
        }

        public async Task Delete(int id)
        {
            _context.Appointments.Remove(await Get(id));
            await _context.SaveChangesAsync();
        }

        public async Task<Appointment> Get(int id)
        {
            return await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Appointment>> GetAll()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task<List<Appointment>> GetByTitle(string title, string userId)
        {
            return await _context.Appointments.Where(a => a.Title != null && a.Title.Contains(title) && a.ApplicationUserId == userId).ToListAsync();
        }

        public async Task<Appointment> Update(Appointment entity)
        {
            var temp = await Get(entity.Id);
            temp.Title = entity.Title;
            temp.Details = entity.Details;
            temp.ExpiDate = entity.ExpiDate;
            return await _context.SaveChangesAsync() > 0 ? entity : null;
        }
        public async Task<List<Appointment>> GetByUserIdAsync(string userId)
        {
            return await _context.Appointments.Where(a=> a.ApplicationUserId == userId).ToListAsync();
        }
    }
}
