using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Comments;
public interface ICommentsService
{
    Task CreateCommentAsync(Guid productId, Guid userId, string text, int rate);
}
