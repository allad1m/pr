using Refit;
using System;
using System.Collections.Generic;
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
using WebApplication1.News;
using WpfApp2.Sessions;

namespace WpfApp2.News
{
    /// <summary>
    /// Логика взаимодействия для NewsEditPage.xaml
    /// </summary>
    public partial class NewsEditPage : Page
    {
        private readonly INewsService service;
        private int? _newsId;
        private EventHandler OnNewsChanged;
        public NewsEditPage(EventHandler onNewsChanged, int? newsId = null)
        {
            InitializeComponent();
            service = NetworkManager.Instance.NewsService;
            _newsId = newsId;
            OnNewsChanged += onNewsChanged;
            if (_newsId != null)
            {
                Task.Run(async () =>
                {
                    
                    try
                    {
                        var item = await service.Details((int)_newsId);
                        await Dispatcher.BeginInvoke(() =>
                        {
                            titleText.Text = item.Title;
                            contentText.Text = item.Content;
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
            }
        }

        private void Save()
        {
            NewsCreateDTO item = new()
            {
                Title = titleText.Text,
                Content = contentText.Text
            };
            Task.Run(async () =>
            {
                try
                {
                    NewsDTO createditem = _newsId == null ? await service.Create(item) : await service.Edit((int)_newsId, item);

                    await Dispatcher.BeginInvoke(() =>
                    {
                        OnNewsChanged?.Invoke(createditem, EventArgs.Empty);
                        NavigationService?.GoBack();
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            var action = MessageBox.Show("Все правильно написали? Грамматических ашыбок нет?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (action == MessageBoxResult.Yes)
            {
                Save();
            }
        }
    }

}
