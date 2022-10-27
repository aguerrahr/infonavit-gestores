using System.IO;
using System;

namespace GestoresAPI.Funciones
{
    public static class FuncionesUtiles
    {
        public static void LogToFile(string text)
        {

            try
            {
                //string modoDebug = ConfigurationManager.AppSettings["modoDebug"];
                //modoDebug = SecurityManager.Decrypt(modoDebug);
                string filePrefix = "WsListaExcepcionLog";

                //if (modoDebug.Equals("false"))
                //{
                //    return;
                //}

                string extension;
                extension = "txt";

                string filePath = @"c:/logautmanager";
                //filePath = SecurityManager.Decrypt(filePath);
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                string fileName = $"{filePath}/{filePrefix}{DateTime.Now.ToString("ddMMyyyy")}.{extension}";

                File.AppendAllText(fileName, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss:tt") + " :: " + text + Environment.NewLine);
            }
            catch (Exception ex)
            {

            }

        }
    }
}
