using Memories.Business.Enums;
using Memories.Business.Models;
using Memories.Core;
using Memories.Core.Extensions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Memories.Modules.EditBook.ViewModels
{
    public class PreviewViewVM : BindableBase
    {
        private Book _editBook;
        private ObservableCollection<RenderTargetBitmap> _previews;
        private int _selectedIndex;

        private IApplicationCommands _applicationCommands;


        public Book EditBook
        {
            get { return _editBook; }
            set 
            {
                SetProperty(ref _editBook, value);
                if (EditBook == null)
                {
                    Previews = null;
                }
                else
                {
                    EditBookChanged();
                }
            }
        }

        public ObservableCollection<RenderTargetBitmap> Previews
        {
            get { return _previews; }
            set { SetProperty(ref _previews, value); }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set 
            {
                SetProperty(ref _selectedIndex, value);
                if (ApplicationCommands.PageMoveCommand.CanExecute(SelectedIndex))
                {
                    ApplicationCommands.PageMoveCommand.Execute(SelectedIndex);
                }
            }
        }

        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
            set { SetProperty(ref _applicationCommands, value); }
        }


        public PreviewViewVM(IApplicationCommands applicationCommands)
        {
            _applicationCommands = applicationCommands;
        }

        private void EditBookChanged()
        {
            var list = new List<RenderTargetBitmap>(EditBook.BookPages.Count + 2);
            int width = EditBook.PaperSize.GetWidth();
            int height = EditBook.PaperSize.GetHeight();
            double dpiX, dpiY;
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                dpiX = g.DpiX;
                dpiY = g.DpiY;
            }

            var pages = new List<BookPage>(EditBook.BookPages);
            pages.Insert(0, EditBook.FrontCover);
            pages.Add(EditBook.BackCover);

            foreach (var item in pages)
            {
                Canvas canvas = item.ToCanvas(EditBook.PaperSize);

                DrawingVisual drawingVisual = new DrawingVisual();
                using (DrawingContext dc = drawingVisual.RenderOpen())
                {
                    VisualBrush vb = new VisualBrush(canvas);
                    dc.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), new System.Windows.Point(width, height)));
                }

                RenderTargetBitmap bmp = new RenderTargetBitmap(width, height, dpiX, dpiY, PixelFormats.Pbgra32);
                bmp.Render(drawingVisual);

                list.Add(bmp);
            }

            Previews = new ObservableCollection<RenderTargetBitmap>(list);
        }

    }
}
