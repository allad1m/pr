using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Comments;
using WebApplication1.News;

namespace WpfApp2.Comments
{
    internal interface ICommentsService
    {
        [Get("/comments")]
        Task<List<CommentsDTO>> Index([Query] int newsId);

        [Get("/comments/{id}")]
        Task<CommentsDTO> Details(int id);

        [Post("/comments")]
        Task<CommentsDTO> Create([Body] CommentCreateDTO item);
        [Post("/comments/{id}")]
        Task<CommentsDTO> Edit(int id, [Body] CommentCreateDTO item);

        [Delete("/comments/{id}")]
        Task Delete(int id);
    }
}
