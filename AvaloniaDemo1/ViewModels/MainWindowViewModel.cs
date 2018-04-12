using AvaloniaDemo1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace AvaloniaDemo1.ViewModels
{

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string p_UserName;

        public string UserName
        {
            get { return p_UserName; }

            set
            {
                p_UserName = value;
                OnPropertyChanged();
            }
        }

        private string p_BoardName;

        public string BoardName
        {
            get { return p_BoardName; }

            set
            {
                p_BoardName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ImageItem> Items { get; }

        private ImageItem p_SelectedItem;

        public ImageItem SelectedItem
        {
            get { return p_SelectedItem; }

            set
            {
                p_SelectedItem = value;
                OnPropertyChanged();
            }
        }

        private string p_Message;

        public string Message
        {
            get { return p_Message; }

            set
            {
                p_Message = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            Items = new ObservableCollection<ImageItem>();

            SelectedItem = new ImageItem();
        }

        public void ReadPinterest()
        {
            Message = "";

            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(BoardName))
            {
                Message = "Please enter both Pinterest user name and board name.";
            }
            else
            {
                string uri = $"https://www.pinterest.com/{ UserName }/{ BoardName }.rss/";
                var document = XDocument.Load(uri);

                var items = document.Element("rss").Element("channel").Elements("item");
                foreach (var item in items)
                {
                    try
                    {
                        string title = item.Element("title").Value;

                        if (string.IsNullOrWhiteSpace(title))
                        {
                            title = "Pinterest Image";
                        }

                        string description = item.Element("description").Value;
                        DateTime publishedDate = Convert.ToDateTime(item.Element("pubDate").Value);

                        var htmlDescription = XDocument.Parse($"<div>{ description.Replace("></a>", "/></a>") }</div>");

                        var mainImage = htmlDescription.Root.Descendants("img").FirstOrDefault();

                        var imageItem = new ImageItem
                        {
                            Title = title,
                            Url = mainImage.Attribute("src").Value,
                            PublishedDate = publishedDate.ToString("yyyy-MM-dd HH:mm")
                        };

                        using (var webClient = new WebClient())
                        {
                            Uri imageUri = new Uri(imageItem.Url);
                            string filename = System.IO.Path.GetFileName(imageUri.LocalPath);

                            string localImageFilePath = $"{ Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) }/images/{ filename }";

                            webClient.DownloadFile(imageItem.Url, localImageFilePath);

                            imageItem.Image = new Avalonia.Media.Imaging.Bitmap(localImageFilePath);
                        }

                        Items.Add(imageItem);
                    }
                    catch (Exception)
                    {
                        
                    }
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
