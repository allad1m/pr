using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
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
using System.Xml.Linq;
using WebApplication1.News;
using WpfApp2.Comments;
using WpfApp2.Sessions;
using WpfApp2.Users;

namespace WpfApp2.News
{
    /// <summary>
    /// Логика взаимодействия для NewsPage.xaml
    /// </summary>
    public partial class NewsPage : Page
    {
        private INewsService service;
        readonly ObservableCollection<NewsDTO> items = new();
        public NewsPage()
        {
            InitializeComponent();
            service = NetworkManager.Instance.NewsService;
            newsList.ItemsSource = items;
            FetchNews();
        }

        void FetchNews()
        {
            errorBlock.Visibility = Visibility.Collapsed;
            loadingIndicator.IsActive = true;
            Task.Run(async () =>
            {
                try
                {
                    var response = await service.Index();
                    await Dispatcher.BeginInvoke(() =>
                    {
                        if ( response.Capacity == 0)
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

        void DeleteNews(int id)
        {
            Task.Run(async () =>
            {
                try
                {
                    await service.Delete(id);
                    await Dispatcher.BeginInvoke(() =>
                    {
                        items.Remove(items.First(n => n.Id == id));
                    });
                }
                catch (Exception ex)
                {
                    await Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show(ex.Message);
                    });
                }
            });
        }

        private void NewsChanged(object sender, EventArgs e)
        {
            var item = (NewsDTO)sender;
            var existing = items.FirstOrDefault(n => n.Id == item.Id);
            if (existing == null)
            {
                items.Insert(0, item);

            }
            else
            {
                var index = items.IndexOf(existing);
                items.RemoveAt(index);
                items.Insert(index, item);
            }
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NewsEditPage(NewsChanged));
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            int newsId = GetContext<NewsDTO>(sender).Id;
            var item = GetContext<NewsDTO>(sender).AuthorId;
            var serviceAuth = NetworkManager.Instance.AuthService;
            Task.Run(async () =>
            {
                var nowuser = await serviceAuth.Profile();
                await Dispatcher.BeginInvoke(() =>
                {
                    if (nowuser.User.Id == item)
                    {
                        NavigationService?.Navigate(new NewsEditPage(NewsChanged, newsId));
                    }
                    else
                    {
                        MessageBox.Show("Это не ваша новость.");
                    }
                });
            });
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var item = GetContext<NewsDTO>(sender).AuthorId;
            
            var serviceAuth = NetworkManager.Instance.AuthService;
            Task.Run(async () =>
            {
                var nowuser = await serviceAuth.Profile();
                await Dispatcher.BeginInvoke(() =>
                {
                    if (nowuser.User.Id == item)
                    {
                        var action = MessageBox.Show("Удалить новость? Это действие нельзя отменить!", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (action == MessageBoxResult.Yes)
                        {
                            DeleteNews(GetContext<NewsDTO>(sender).Id);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Это не ваша новость.");
                    }
                });
            });
            
        }

        private void CommentsBtn_Click(object sender, RoutedEventArgs e)
        {
            int newsId = GetContext<NewsDTO>(sender).Id;
            NavigationService?.Navigate(new CommentsPage(newsId));
        }

        private void RetryBtn_Click(object sender, RoutedEventArgs e)
        {
            FetchNews();
        }

        private T GetContext<T>(object sender)
        {
            var view = sender as FrameworkElement;
            return (T)view.DataContext;
        }

        private void AuthBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthPage());
        }

        private void QuitBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProfilePage());
        }
    }

}
