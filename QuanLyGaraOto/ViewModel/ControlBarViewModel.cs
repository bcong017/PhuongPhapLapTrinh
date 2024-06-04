using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyGaraOto.ViewModel
{
    public class ControlBarViewModel : BaseViewModel
    {
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set;}
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MouseMoveCommand { get; set; }
        public ControlBarViewModel()
        {
            CloseWindowCommand = new RelayCommand<UserControl>((p) => p != null, (p) => { CloseWindow(p); });

            MaximizeWindowCommand = new RelayCommand<UserControl>((p) => p != null, (p) => { MaximizeWindow(p); });

            MinimizeWindowCommand = new RelayCommand<UserControl>((p) => p != null, (p) => { MinimizeWindow(p); });

            MouseMoveCommand = new RelayCommand<UserControl>((p) => p != null, (p) => { MoveWindow(p); });
        }

        FrameworkElement GetParent(UserControl p)
        {
            FrameworkElement parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }  
                
            return parent;
        }

        void CloseWindow(UserControl p)
        {
            FrameworkElement parent = GetParent(p);
            //Application.Current.Shutdown(); 
            Window window = parent as Window;
            if (window != null) window.Close();
        }

        void MaximizeWindow(UserControl p)
        {
            FrameworkElement parent = GetParent(p);
            Window window = parent as Window; 
            if (window != null)
            {
                if (window.WindowState != WindowState.Maximized)
                    window.WindowState = WindowState.Maximized;
                else
                    window.WindowState = WindowState.Normal;    
            }                 
        }

        void MinimizeWindow(UserControl p)
        {
            FrameworkElement parent = GetParent(p);
            Window window = parent as Window;
            if (window != null)
            {
                if (window.WindowState != WindowState.Minimized)
                    window.WindowState = WindowState.Minimized;
                else
                    window.WindowState = WindowState.Normal;
            }
        }

        void MoveWindow(UserControl p)
        {
            FrameworkElement parent = GetParent(p);
            Window window = parent as Window;

            if (window != null)
            {
                window.DragMove();
            }    
        }

    }
}
