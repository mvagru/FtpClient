using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace FTP_CLIENT
{
    class FtpClient
    {
        /* Создание полей для хранения информации о клиенте g */
                            /* Хранение веб адреса */
        private string sHost;
                            /* Хранение логина */
        private string sUsername;
                            /* Хранение пароля */
        private string sPassword;
                            /* Использование SSL */
        private bool sUseSSL = false;
        
        /* Подключение FTP */
        
        FtpWebRequest ftpRequest;
        FtpWebResponse ftpResponse;

        /* Задание свойств */
        public string Host
        { 
            get { return sHost; } 
            set { sHost = value; } 
        }
        public string UserName
        {
            get { return sUsername; }
            set { sUsername = value; }
        }
        public string Password
        {
            get { return sPassword; }
            set { sPassword = value; }
        }
        public bool UseSSL
        {
            get { return sUseSSL; }
            set { sUseSSL = value; }
        }

        /* Описание структуры файлов на ФТП сервере */
        public struct FileStruct
        {
            public string Name;
            public string CreateTime;
            public string Owner;
            public string Flags;
            public bool IsDirectory;
        }

        /* Подключение к ФТП и вывод подробного списка файлов на сервере */
        public FileStruct[] ListDirectory(string path)
        {
            if (path == null || path == "")
            {
                path = "/";
            }
            /* Путь */
            ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://" + sHost + path);
            /* Проверка логина\пароля */
            ftpRequest.Credentials = new NetworkCredential(sUsername, sPassword);
            /* Получение списка файлов с фтп */
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            /* SSL */
            ftpRequest.EnableSsl = sUseSSL;
            /* Получение ответа с фтп */
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            /* Перевод ответа с фтп в нужную кодировку и запись в строку */
            string content = "";           
            StreamReader sr = new StreamReader(ftpResponse.GetResponseStream(), System.Text.Encoding.ASCII);
            content = sr.ReadToEnd();
            /* Закрытие потока, закрытие обращения */
            sr.Close();
            ftpResponse.Close();
            /* Вывод конечного результата */
            DirectoryListParser parser = new DirectoryListParser(content);
            return parser.FullListing;
        }
        /* Загрузка файла с сервера */
        public void DownloadFile(string path, string fileName)
        {
            /* Путь */
            ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://" + sHost + path + "/" + fileName);
            /* Проверка логина\пароля */
            ftpRequest.Credentials = new NetworkCredential(sUsername, sPassword);
            /* Скачивание */
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            /* SSL если необходим */
            ftpRequest.EnableSsl = sUseSSL;
            /* Запрашиваем поток с фтп */
            FileStream downloadedFile = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            /* Забираем входящий поток */
            Stream responseStream = ftpResponse.GetResponseStream();
            /* Буффер? */
            byte[] buffer = new byte[1024];
            int size = 0;
            while ((size=responseStream.Read(buffer,0,1024))>0)
            { downloadedFile.Write(buffer, 0, size); }
            /* Закрываем поток, закрываем файл, закрываем запрос к фтп */
            ftpResponse.Close();
            downloadedFile.Close();
            responseStream.Close();
        }
        /* Загрузка файла на сервер */
        public void UploadFile(string path, string fileName)
        {
            /* Путь */
            ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://" + sHost + path);
            /* Проверка логина\пароля */
            ftpRequest.Credentials = new NetworkCredential(sUsername, sPassword);
            /* SSL если необходим */
            ftpRequest.EnableSsl = sUseSSL;
            /* Загрузка */
            ftpRequest.Method = WebRequestMethods.File.UploadFile;
            /* Запрашивае поток на фтп */
            FileStream uploadedFile = new FileStream(fileName,FileMode.Open,FileAccess.Read);            
            /* Буффер */
            byte[] files_to_bytes = new byte[uploadedFile.Length];
            /* Вносим данные */
            uploadedFile.Read(files_to_bytes, 0, files_to_bytes.Length);
            uploadedFile.Close();
            /* Поток загрузки */
            Stream writer = ftpRequest.GetRequestStream();
            writer.Write(files_to_bytes, 0, files_to_bytes.Length);
            writer.Close();
        }
        /* Удаление файла */
        public void DeleteFile(string path)
        {
            /* Путь */
            ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://"+sHost+path);
            /* Проверка логина\пароля */ 
            ftpRequest.Credentials = new NetworkCredential(sUsername, sPassword);
            /* SSL если необходим */
            ftpRequest.EnableSsl = sUseSSL;
            ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
            FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ftpResponse.Close();

        }
        /* Создание каталога */
        public void CreateDirectory(string path, string folderName)
        {
            ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://" + sHost + path + folderName);
            ftpRequest.Credentials = new NetworkCredential(sUsername, sPassword);
            ftpRequest.EnableSsl = sUseSSL;
            ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
            FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ftpResponse.Close();
        }
        /* Удаление каталога */
        public void RemoveDirectory(string path)
        {
            string filename = path;
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://" + sHost + path);
            ftpRequest.Credentials = new NetworkCredential(sUsername, sPassword);
            ftpRequest.EnableSsl = sUseSSL;
            ftpRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
            FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            ftpResponse.Close();
        }

    }
}
