using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ChangeDisk
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MessageBox.Show("Server должен лежать в корне, и иметь имя \"Server\"");
        }

        string Text = "";

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                char disk = Convert.ToChar(Disk.Text);
                string httpd = string.Concat(disk, @":\Server\bin\Apache24\conf\httpd.conf");        // путь к файлу
                string RUNS = string.Concat(disk, @":\Server\bin\RUNS.bat");        // путь к файлу
                string php = string.Concat(disk, @":\Server\bin\php\php.ini");        // путь к файлу
                string my = string.Concat(disk, @":\Server\bin\mariadb\my.ini");        // путь к файлу

                ReadChangeFile(httpd, "Define SRVDISK \"");
                ReadChangeFile(RUNS, "SET DISKROOT=");
                ReadChangeFile(php, "On windows:\nextension_dir = \"");
                ReadChangeFile(php, "sqlite3.extension_dir = \"");
                ReadChangeFile(my, "datadir=");
                ReadChangeFile(my, "plugin-dir=");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void ReadChangeFile(string FileName, string SearchString)
        {
            try
            {
                using (FileStream fstream = File.OpenRead(FileName))
                {
                    // выделяем массив для считывания данных из файла
                    byte[] buffer = new byte[fstream.Length];
                    // считываем данные
                    fstream.Read(buffer, 0, buffer.Length);
                    // декодируем байты в строку
                    char[] textFromFile = Encoding.Default.GetString(buffer).ToCharArray();
                    try
                    {
                        Text = SearchStr(textFromFile, FileName, SearchString, Convert.ToChar(Disk.Text));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                using (FileStream fstream = new FileStream(FileName, FileMode.OpenOrCreate))
                {
                    // преобразуем строку в байты
                    byte[] buffer = Encoding.Default.GetBytes(Text);
                    // запись массива байтов в файл
                    fstream.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public string SearchStr(char [] text, string FileName, string desiredText, char disk)
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == desiredText[0])
                    {
                        for (int j = 0; j < desiredText.Length;)
                        {
                            if (text[i] == desiredText[j])
                            {
                                j++;
                                i++;
                            }
                            else
                                break;
                            if (desiredText[j] == desiredText[desiredText.Length - 1])
                            {
                                char OldDisk = text[i + 1];
                                text[i + 1] = disk;
                                if(checkBox1.IsChecked == true)
                                {
                                    MessageBox.Show(string.Concat("Прошлый диск:",OldDisk,".\nНовый диск: ",text[i + 1],"\nВ файле:",FileName));
                                }
                                string OutStr = new string(text);
                                return OutStr;
                            }
                                
                        }
                    }
                }
                return "";
        }
    }
}
