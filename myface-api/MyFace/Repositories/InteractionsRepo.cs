using System;
using System.Collections.Generic;
using System.Linq;
using MyFace.Models.Database;
using MyFace.Models.Request;

namespace MyFace.Repositories
{
    public interface IInteractionsRepo
    {
        IEnumerable<Interaction> Search(SearchRequest search);
        int Count(SearchRequest search);
        Interaction GetById(int id);
        Interaction Get(int postId, int userId);
        Interaction Create(CreateInteractionRequest create);
        void Delete(int id);
        void DeleteByPostId(int postId, int userId);

    }

    public class InteractionsRepo : IInteractionsRepo
    {
        private readonly MyFaceDbContext _context;

        public InteractionsRepo(MyFaceDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Interaction> Search(SearchRequest search)
        {
            return _context.Interactions
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize);
        }

        public int Count(SearchRequest search)
        {
            return _context.Interactions.Count();
        }

        public Interaction GetById(int id)
        {
            return _context.Interactions.Single(i => i.Id == id);
        }

        public Interaction Get(int postId, int userId)
        {
            return _context.Interactions.Single(i => i.PostId == postId && i.UserId == userId);
        }

        public Interaction Create(CreateInteractionRequest create)
        {
            var insertResult = _context.Interactions.Add(new Interaction
            {
                Date = DateTime.Now,
                Type = create.InteractionType,
                PostId = create.PostId,
                UserId = create.UserId,
            });
            _context.SaveChanges();
            return insertResult.Entity;
        }

        public void Delete(int id)
        {
            var interaction = GetById(id);
            _context.Interactions.Remove(interaction);
            _context.SaveChanges();
        }

        public void DeleteByPostId(int postId, int userId)
        {
            var interactions = _context.Interactions.Where(i => i.UserId == userId && i.PostId == postId);
            _context.Interactions.RemoveRange(interactions);
            _context.SaveChanges();
        }
    }
}