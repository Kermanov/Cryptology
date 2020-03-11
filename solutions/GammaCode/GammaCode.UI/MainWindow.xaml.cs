using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
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
using static GammaCode.Logic.GammaCode;

namespace GammaCode.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CreateFileIfNotExist();
            using (var fs = new FileStream(usedKeysFileName, FileMode.Open))
            {
                var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<int>));
                UsedKeys = (ObservableCollection<int>)jsonSerializer.ReadObject(fs);
                DataContext = this;
            }
        }

        private byte[] inputFileData;
        private byte[] outputFileData;
        private string usedKeysFileName = "usedKeys.json";
        public ObservableCollection<int> UsedKeys { get; set; }

        private void CreateFileIfNotExist()
        {
            if (!File.Exists(usedKeysFileName))
            {
                using (var fileWriter = File.CreateText(usedKeysFileName))
                {
                    fileWriter.Write("{}");
                }
            }
        }

        private void openFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false
            };

            if (openFileDialog.ShowDialog().Value)
            {
                using (var fileStream = File.Open(openFileDialog.FileName, FileMode.Open))
                {
                    using (var reader = new BinaryReader(fileStream))
                    {
                        inputFileData = reader.ReadBytes((int)fileStream.Length);
                        inputFilenameLabel.Content = openFileDialog.SafeFileName;
                    }
                }
            }
        }

        private void saveFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = (string)outputFilenameLabel.Content;

            if (saveFileDialog.ShowDialog().Value)
            {
                File.WriteAllBytes(saveFileDialog.FileName, outputFileData);
            }
        }

        private void encryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (inputFileData != null && IsKeyValid(out int key) && !IsKeyUsed(key))
            {
                outputFileData = XorBytesWithGamma(inputFileData, key);

                SaveKey(key);

                var inputFilename = (string)inputFilenameLabel.Content;
                outputFilenameLabel.Content = inputFilename.Insert(inputFilename.LastIndexOf('.'), "__encrypted");
            }
        }

        private bool IsKeyValid(out int key)
        {
            return int.TryParse(keyTextBox.Text, out key);
        }

        private bool IsKeyUsed(int key)
        {
            return UsedKeys.Contains(key);
        }

        private void SaveKey(int key)
        {
            UsedKeys.Add(key);

            CreateFileIfNotExist();
            using (var fs = new FileStream(usedKeysFileName, FileMode.Open))
            {
                var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<int>));
                jsonSerializer.WriteObject(fs, UsedKeys);
            }
        }

        private void decryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (inputFileData != null && IsKeyValid(out int key))
            {
                outputFileData = XorBytesWithGamma(inputFileData, key);

                var inputFilename = (string)inputFilenameLabel.Content;
                outputFilenameLabel.Content = inputFilename.Insert(inputFilename.LastIndexOf('.'), "__decrypted");
            }
        }

        private void keyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsKeyValid(out int key))
            {
                keyStateRect.Fill = new SolidColorBrush(Color.FromRgb(200, 0, 0));
            }
            else if (IsKeyUsed(key))
            {
                keyStateRect.Fill = new SolidColorBrush(Color.FromRgb(200, 137, 0));
            }
            else
            {
                keyStateRect.Fill = new SolidColorBrush(Color.FromRgb(60, 200, 0));
            }
        }

        private void aboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = new AboutWindow();
            window.ShowDialog();
        }
    }
}
