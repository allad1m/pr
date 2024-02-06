using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebApplication1.Comments;
using WebApplication1.News;
using Windows.ApplicationModel.Contacts;
using Windows.System;
using WpfApp2.News;
using WpfApp2.Sessions;

namespace WpfApp2.Comments
{
    /// <summary>
    /// Логика взаимодействия для CommentsPage.xaml
    /// </summary>
    public partial class CommentsPage : Page
    {
        private ICommentsService service;
        readonly ObservableCollection<CommentsDTO> items = new();

        private int _newsId;
        private int? commentId = null;

        public CommentsPage(int newsId)
        {
            InitializeComponent();
            service = NetworkManager.Instance.CommmentsService;
            commentsList.ItemsSource = items;
            
            
           
            _newsId = newsId;
            FetchComments();

           /* if (_newsId != null)
            {
                Task.Run(async () =>
                {

                    try
                    {
                        var content = await service.Index(_newsId);
                        await Dispatcher.BeginInvoke(() =>
                        {
                            items.Clear();
                            content.ForEach(x => items.Add(x));
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
            }*/
        }
        private void CommentsChanged(object sender, EventArgs e)
        {
            var item = (CommentsDTO)sender;
           
        }

        void FetchComments()
        {
            errorBlock.Visibility = Visibility.Collapsed;
            loadingIndicator.IsActive = true;
            Task.Run(async () =>
            {
                try
                {
                    var response = await service.Index(_newsId);
                    await Dispatcher.BeginInvoke(() =>
                    {
                        if (response.Capacity == 0)
                        {
                            errorBlock.Visibility = Visibility.Visible;
                            errorText.Text = "Нет данных";
                        }
                        items.Clear();
                        response.ForEach(item => items.Add(item));
                    });
                }
                catch (Exception ex)
                {
                    await Dispatcher.BeginInvoke(() =>
                    {
                        errorBlock.Visibility = Visibility.Visible;
                        errorText.Text = ex.Message;
                    });
                }
                finally
                {
                    await Dispatcher.BeginInvoke(() =>
                    {
                        loadingIndicator.IsActive = false;
                    });
                }
            });
        }

        private void Save()
        {
            CommentCreateDTO item = new()
            {
                NewsId = _newsId,
                Content = CommentText.Text,
                AuthorId = 0,
            };
            Task.Run(async () =>
            {
                try
                {
                    CommentsDTO createditem = commentId == null ? await service.Create(item) : await service.Edit((int)commentId, item);

                    await Dispatcher.BeginInvoke(() =>
                    {
                        var existing = items.FirstOrDefault(n => n.Id == createditem.Id);
                        if (existing == null)
                        {
                            items.Insert(0, createditem);
                        }
                        else
                        {
                            var index = items.IndexOf(existing);
                            items.RemoveAt(index);
                            items.Insert(index, createditem);
                        }
                        CommentText.Text = "";
                        commentId = null;
                    });

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

        private void CommentsBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NewsEditPage(CommentsChanged));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var comment = GetContext<CommentsDTO>(sender);
            var serviceAuth = NetworkManager.Instance.AuthService;
            Task.Run(async () =>
            {
                var nowuser = await serviceAuth.Profile();
                await Dispatcher.BeginInvoke(() =>
                {
                    if (nowuser.User.Id == comment.AuthorId)
                    {
                        commentId = comment.Id;
                        CommentText.Text = comment.Content;
                    }
                    else
                    {
                        MessageBox.Show("Это не ваш комментарий.");
                    }
                });
            });


        }

        private T GetContext<T>(object sender)
        {
            var view = sender as FrameworkElement;
            return (T)view.DataContext;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
