using FileCompare.Core;
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

namespace FileCompare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal FileCompareCore core;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            core = new FileCompareCore(tbLeft.Text, tbRight.Text);
            core.RunCompare();
            LeftResults.ItemsSource = core.LeftOnly;
            RightResults.ItemsSource = core.RightOnly;
            Both.ItemsSource = (checkBox.IsChecked ?? false) ? core.Differ : core.Both;
        }

        private void Browse1_Click(object sender, RoutedEventArgs e)
        {
            tbLeft.Text = OpenDialog();
        }

        private void Browse1_Copy_Click(object sender, RoutedEventArgs e)
        {
            tbRight.Text = OpenDialog();
        }

        private string OpenDialog()
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                return dlg.SelectedPath;

            return string.Empty;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Both.ItemsSource = ((sender as CheckBox).IsChecked ?? false) ? core.Differ : core.Both;
        }
    }
}
