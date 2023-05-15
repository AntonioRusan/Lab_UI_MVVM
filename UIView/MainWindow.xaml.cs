using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using ViewModel;

namespace UIView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(new MessageBoxErrorSender(), new SaveAndLoadFileDialog());
        }
        public class MessageBoxErrorSender : IErrorSender
        {
            public void SendError(string message) => MessageBox.Show(message);
        }
        public class SaveAndLoadFileDialog : IFileDialog
        {
            public string OpenFileDialog()
            {
                try
                {
                    string resultFileName = "";
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    if (openFileDialog.ShowDialog() == true)
                        resultFileName = openFileDialog.FileName;
                    return resultFileName;
                }
                catch (Exception ex)
                {
                    throw new Exception("Не удалось открыть файл: " + ex.Message);
                }
            }
            public string SaveFileDialog()
            {
                try
                {
                    string resultFileName = "";
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    if (saveFileDialog.ShowDialog() == true)
                        resultFileName = saveFileDialog.FileName;
                    return resultFileName;
                }
                catch (Exception ex)
                {
                    throw new Exception("Не удалось сохранить файл: " + ex.Message);
                }
            }
        }
    }
}
