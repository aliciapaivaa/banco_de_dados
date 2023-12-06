using System.Data.Entity;
using WebApi.Helpers;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;
using Util = Utils.Utils;
using EfExtensions = Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions;


// using like = EF.Functions.Like; 
namespace Repositories;

public class PostRepository : IPostRepository
{
    private readonly ApplicationContext _context;

    public PostRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task add(PostModel post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();  
    }

    public async Task delete(PostModel post)
    {
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();  
    }

    public IQueryable<PostModel> getAll()
    {
        var all = EfExtensions.Include(
                  EfExtensions.Include(
                  EfExtensions.Include(_context.Posts, e=> e.Likes), e => e.User), e => e.Categories);

        return all;
    }

    public  List<PostModel> getByCategory(CategoryModel category)
    {
        return Util.toList<PostModel>( getAll().Where(e => e.Categories.Contains(category)));
    }

    public List<PostModel> getByDate(DateTime begin, DateTime end)
    {
        return Util.toList<PostModel>(getAll().Where(e=> e.date >= begin && e.date <= end));
    }

    public async Task<PostModel> getByGuid(Guid guid)
    {
        var p = getAll().Where(e => e.guid == guid).FirstOrDefault(); 

        return p;
    }

    public List<PostModel> getByTitle(string title)
    {
        return Util.toList<PostModel>(getAll().Where(e => EF.Functions.Like(e.title,  $"%{title}%")));
    }

    public async Task<List<PostModel>> getByUser(UserModel user)
    {
        var all = EfExtensions.Include(_context.Users, e=> e.Posts);

        var u = all.Where(e => e.email == user.email).FirstOrDefault(); 

        return u.Posts;
    }

    public async Task<PostModel> update(PostModel post)
    {
        var p = await getByGuid(post.guid);

        if(post.subtitle != null)
            p.subtitle = post.subtitle;
        if(post.content != null)
            p.content = post.content;
        if(post.title != null)
            p.title = post.title;
        if(post.approved != false)
            p.approved = post.approved;
        
        p.date = DateTime.Now;

        _context.Posts.Update(p);
        
        await _context.SaveChangesAsync();

        return await getByGuid(post.guid);
    }
}