using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProfileRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProfileRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       
        public async Task<User?> Get(Guid userId, CancellationToken ct)
        {
            return await _dbContext.Users
                .Include(u => u.Student) 
                .AsNoTracking()         
                .FirstOrDefaultAsync(x => x.UserId == userId, ct);
        }

        public async Task<User?> Update(Guid id, ProfileDto req, CancellationToken ct)
        {
            var res = await _dbContext.Users.FirstOrDefaultAsync(c => c.UserId == id) ?? throw new Exception();




            res.Email = req.Email;
            res.Name = req.Name;
            res.Phone = req.Phone;
            res.Role = req.Role;


            
            

            await _dbContext.SaveChangesAsync(ct);



            return res;



        }
    }
}
