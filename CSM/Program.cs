using System;
using System.Threading;

namespace CSM
{
    /// <summary>
    /// @author Foton63rus
    /// Созданные пары логин/пароль будут рамещаться в файле lp.txt рядом с основным файлом
    /// Созданные логи будут сохраняться в logs.txt
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            WebHelper wh = new WebHelper();
            wh.generateMail();
        }
    }
}
