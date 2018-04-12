using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvaloniaDemo1.Models
{
    public class ImageItem
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public Bitmap Image { get; set; }

        public string PublishedDate { get; set; }
    }
}
