using System;
using System.Collections.Generic;
using System.IO;
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

using CeasarsCode.Logic;
using Microsoft.Win32;
using static CeasarsCode.Logic.CeasarsCode;

namespace CeasarsCode.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filterString = "Text file (*.txt)|*.txt";

        public MainWindow()
        {
            InitializeComponent();
        }

        private enum Mode
        {
            Encrypt,
            Decrypt
        }

        private void transformButton_Click(object sender, RoutedEventArgs e)
        {
            var alphabet = (Alphabet)alphabetComboBox.SelectedIndex;
            var inputText = GetInputText();

            if (int.TryParse(keyTextBox.Text, out int key)
                && IsTextValid(inputText, alphabet)
                && IsKeyValid(key, alphabet)
            )
            {
                var mode = (Mode)modeComboBox.SelectedIndex;

                if (mode == Mode.Encrypt)
                {
                    outputTextBox.Text = Encrypt(inputText, key, alphabet);
                }
                else if (mode == Mode.Decrypt)
                {
                    outputTextBox.Text = Decrypt(inputText, key, alphabet);
                }
            }
        }

        private void CheckIsTextValid()
        {
            var inputText = GetInputText();
            var alphabet = (Alphabet)alphabetComboBox.SelectedIndex;

            if (IsTextValid(inputText, alphabet))
            {
                inputTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 200, 0));
            }
            else
            {
                inputTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 0, 0));
            }
        }

        private void inputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIsTextValid();
        }

        private void keyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var alphabet = (Alphabet)alphabetComboBox.SelectedIndex;

            if (int.TryParse(keyTextBox.Text, out int key)
                && IsKeyValid(key, alphabet)
            )
            {
                keyTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 200, 0));
            }
            else
            {
                keyTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 0, 0));
            }
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = filterString
            };

            if (openFileDialog.ShowDialog().Value)
            {
                inputTextBox.Text = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
            }
        }

        private void SaveToFile(string text)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = filterString
            };


            if (saveFileDialog.ShowDialog().Value)
            {
                File.WriteAllText(saveFileDialog.FileName, text);
            }
        }

        private void SaveInputMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveToFile(inputTextBox.Text);
        }

        private void SaveOutputMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveToFile(outputTextBox.Text);
        }

        private void NewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            inputTextBox.Clear();
            outputTextBox.Clear();
            keyTextBox.Clear();

            modeComboBox.SelectedIndex = 0;
            alphabetComboBox.SelectedIndex = 0;
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void bruteforceButton_Click(object sender, RoutedEventArgs e)
        {
            var alphabet = (Alphabet)alphabetComboBox.SelectedIndex;
            var inputText = GetInputText();

            if (IsTextValid(inputText, alphabet))
            {
                outputTextBox.Clear();

                int key = 0;
                while (IsKeyValid(key, alphabet))
                {
                    var brutforcedText = Decrypt(inputText, key, alphabet);
                    outputTextBox.Text += $"Key: {key}\n{brutforcedText}\n\n";

                    ++key;
                }
            }
        }

        private string GetInputText()
        {
            return inputTextBox.Text.ToLower();
        }

        private void alphabetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckIsTextValid();
        }
    }
}
