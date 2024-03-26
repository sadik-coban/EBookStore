using Core.Entities;
using Core.Infrastructure.Base.RepositoriesBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Comments;
public class CommentsService(IRepositoryBase<Comment> commentsRepository) : ICommentsService
{
    public async Task CreateCommentAsync(Guid productId, Guid userId, string text, int rate)
    {
        var comment = new Comment
        {
            DateCreated = DateTime.UtcNow,
            Enabled = false,
            ProductId = productId,
            UserId = userId,
            Body = text,
            Rate = rate
        };

        await commentsRepository.CreateAsync(comment);
    }

    public async Task<int> EnableCommentAsync(Guid commentId)
    {
        return await commentsRepository.ExecuteUpdateAsync(p => p.Id == commentId, s => s.SetProperty(p => p.Enabled, true));
    }
}
