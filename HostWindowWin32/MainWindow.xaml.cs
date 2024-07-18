using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace HostWindowWin32
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var process = Process.Start(@"calc.exe"); //Your path, this is an example with the Windows base calculator
            process.WaitForInputIdle();

            IntPtr hWndExternalProc = process.MainWindowHandle;
            while (hWndExternalProc == IntPtr.Zero)
            {
                process.Refresh();
                hWndExternalProc = process.MainWindowHandle;
            }


            if (hWndExternalProc != IntPtr.Zero)
            {
                var host = new WindowHost(33163818);

                uxGrid.Children.Add(host);
            }
        }
    }
}
