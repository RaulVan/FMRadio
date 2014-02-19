using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace FMRaidoLoca
{
    public class AppConfig
    {
        /// <summary>
        /// 是否第一次使用
        /// </summary>
        public static bool IsUsedFirst
        {
            get
            {
                return IsolatedStorageSettings.ApplicationSettings.Contains("IsUsedFirst") ? (bool)IsolatedStorageSettings.ApplicationSettings["IsUsedFirst"] : false;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["IsUsedFirst"] = value;
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }
    }
}
