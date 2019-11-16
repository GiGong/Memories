using Memories.Business.Enums;
using Memories.Business.Models;
using Memories.Core;
using Memories.Core.Extensions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Memories.Modules.EditBook.ViewModels
{
    public class PreviewViewVM : BindableBase
    {
        private int _width, _height;

        private Book _editBook;
        private ObservableCollection<Preview> _previews;
        private int _selectedIndex;

        private IApplicationCommands _applicationCommands;
        private readonly double _dpiX, _dpiY;
        private readonly DispatcherTimer _timer;

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
                    _width = EditBook.PaperSize.GetWidth();
                    _height = EditBook.PaperSize.GetHeight();
                    EditBookChanged();
                    EditBook.BookPages.CollectionChanged += (s, e) =>
                        {
                            _timer.Stop();
                            _timer.Start();
                        };
                }
            }
        }

        public ObservableCollection<Preview> Previews
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

            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                _dpiX = g.DpiX;
                _dpiY = g.DpiY;
            }

            foreach (Window window in Application.Current.Windows)
            {
                Mouse.AddMouseUpHandler(window, new MouseButtonEventHandler(MouseUpEvent));
                Keyboard.AddPreviewKeyUpHandler(window, new KeyEventHandler(KeyUpEvent));
            }

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            _timer.Tick += Timer_Tick;
        }

        private void KeyUpEvent(object sender, KeyEventArgs e)
        {
            _timer.Stop();
            _timer.Start();
        }

        private void MouseUpEvent(object sender, MouseButtonEventArgs e)
        {
            _timer.Stop();
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            EditBookChanged();
        }

        private void EditBookChanged()
        {
            var bookPages = new List<BookPage>(EditBook.BookPages);
            bookPages.Insert(0, EditBook.FrontCover);
            bookPages.Add(EditBook.BackCover);

            var list = new List<Preview>(EditBook.BookPages.Count + 2);

            for (int index = 0, l = bookPages.Count; index < l; index++)
            {
                list.Add(new Preview
                {
                    Number = index.ToString(),
                    Source = PageToRTB(bookPages[index])
                });
            }

            list[0].Number = "표지";
            list[list.Count - 1].Number = "뒷면";

            Previews = new ObservableCollection<Preview>(list);
        }

        private RenderTargetBitmap PageToRTB(BookPage bookPage)
        {
            Canvas canvas = bookPage.ToCanvas(EditBook.PaperSize);

            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext dc = drawingVisual.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(canvas);
                dc.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), new System.Windows.Point(_width, _height)));
            }

            RenderTargetBitmap bmp = new RenderTargetBitmap(_width, _height, _dpiX, _dpiY, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            return bmp;
        }

    }

    public class Preview
    {
        public string Number { get; set; }
        public RenderTargetBitmap Source { get; set; }
    }
}
