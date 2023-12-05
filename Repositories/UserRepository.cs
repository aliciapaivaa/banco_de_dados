using System.Data.Entity;
using WebApi.Helpers;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;
using Util = Utils.Utils;
using EfExtensions = Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions;

namespace Repositories;

public class UserRepository : IUserRepository
{

    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task add(UserModel user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task delete(UserModel user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();    
    }

    public IQueryable<UserModel> getAll()
    {
        var all = EfExtensions.Include(_context.Users, e=> e.UserPermission);

        return all;
    }

    public List<UserModel> getByName(string name)
    {        
        return Util.toList<UserModel>(getAll().Where(e => EF.Functions.Like(e.name,  $"%{name}%"))); 
    }

    public UserModel getByEmail(string email)
    {
        return getAll().Where(e => e.email == email).FirstOrDefault(); 
    }

    public async Task<UserModel> update(UserModel user)
    {
        var u = getByEmail(user.email);

        if(user.guid_permission != Guid.Empty)
            u.guid_permission = user.guid_permission;
        if(user.name != null)
            u.name = user.name;
        if(user.pass_hash != -1)
            u.pass_hash = user.pass_hash;

        _context.Users.Update(u);
        
        await _context.SaveChangesAsync();    

        return u;
    }
}