using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

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
        
        FtpWebRequest ftpWebRequest;
        FtpWebResponse ftpWebResponce;

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


    }
}
