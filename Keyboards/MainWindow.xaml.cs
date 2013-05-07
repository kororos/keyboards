using System;
using System.IO;
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
using CNParser;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Keyboards
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            //In order for the application to work properly Windows Vista is
            //the minimum required OS
            if (CommonFileDialog.IsPlatformSupported != true)
            {
                MessageBox.Show("The application requires at least Windows Vista to work correctly.", "OS Not Supported", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return;
            }
        }

        private async void testbutton_Click(object sender, RoutedEventArgs e)
        {
            String inFileName = null;
            String outFilename = null;

            
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".txt.";
            dlg.Filter = "Text Files|*.txt";
            dlg.Title = "Select input file";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                inFileName = dlg.FileName;
            }
            
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.Title = "Select Output Folder";
            CommonFileDialogResult dirResult = dialog.ShowDialog();

            if (dirResult == CommonFileDialogResult.Ok)
            {
                outFilename = dialog.FileName + "\\outfile.txt";
            }
            
            //MessageBox.Show("inFile:\t" + inFileName + "\nOutFile:\t" + outFilename);
            if ((inFileName != null) && (outFilename != null))
            {
                LKParser parser = new LKParser(inFileName, outFilename);
                MessageBox.Show("inFile:\t" + parser.getInPath() + "\nOutFile:\t" + parser.getOutPath());
                
                Dictionary<string, UInt16> letters;
                Task<Dictionary<string, UInt16>> getLetters = parser.readFileAsync();
                letters = await getLetters;
                //await parser.readFileAsync();
                //Dictionary<string, UInt16> letters = parser.getLettersDic();
                string outputString = "";
                foreach (string s in letters.Keys)
                {
                    outputString += "Key: " + s + "\tTimes: " + letters[s] + "\n";
                }
                parser.writeFileAsync();
                MessageBox.Show("Done", "Completed");
                //MessageBox.Show("Result:\n" + outputString);
            }
        }
    }
}
