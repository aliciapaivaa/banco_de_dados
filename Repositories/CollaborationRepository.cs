using WebApi.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Models;
using Repositories.Interfaces;
using Util = Utils.Utils;
using EfExtensions = Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions;
using SQLitePCL;

namespace Repositories;

public class CollaborationRepository : ICollaborationRepository
{
    private readonly ApplicationContext _context;

    public CollaborationRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task add(CollaborationModel collaboration)
    {
        await _context.Collaborations.AddAsync(collaboration);
        await _context.SaveChangesAsync();    
    }

    public async Task delete(CollaborationModel collaboration)
    {
        _context.Collaborations.Remove(collaboration);
        await _context.SaveChangesAsync();    
    }

    public IQueryable<CollaborationModel> getAll()
    {
        var all = EfExtensions.Include(
                  EfExtensions.Include(
                  EfExtensions.Include(_context.Collaborations, e=> e.CollaborationPermission), e => e.Post), e=> e.User);

        return all;
    }

    public IQueryable<CollaborationModel> getByPost(PostModel post)
    {
        return getAll().Where(e => e.guid_post == post.guid );
    }

    public IQueryable<CollaborationModel> getByUser(UserModel user)
    {
        return getAll().Where(e => e.user_email == user.email );
    }

    public async Task<CollaborationModel> update(CollaborationModel collaboration)
    {
        var c = getAll().Where(e => e.user_email == collaboration.user_email && e.guid_post == collaboration.guid_post).FirstOrDefault();

        if(collaboration.guid_Collaboration_permission != Guid.Empty)
            c.guid_Collaboration_permission = collaboration.guid_Collaboration_permission;

        _context.Collaborations.Update(c);
        
        await _context.SaveChangesAsync();    

        return c;
    }
}