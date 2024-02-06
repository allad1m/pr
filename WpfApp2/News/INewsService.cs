using Refit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WebApplication1.News;
using WpfApp2.Comments;
using WpfApp2.Sessions;

namespace WpfApp2.News
{
    public interface INewsService
    {
        [Get("/news")]
        Task<List<NewsDTO>> Index();

        [Get("/news/{id}")]
        Task<NewsDTO> Details(int id);

        [Post("/news")]
        Task<NewsDTO> Create([Body] NewsCreateDTO item);

        [Post("/news/{id}")]
        Task<NewsDTO> Edit(int id, [Body] NewsCreateDTO item);

        [Delete("/news/{id}")]
        Task Delete(int id);
    }

    

}
