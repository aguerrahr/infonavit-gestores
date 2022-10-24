using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestoresAPI
{
    public enum ErrorType
    {
        GestorNotFound,
        GerenteNotFound
    }
    public static class ErrorMessages
    {
        public static string GetString(this ErrorType type)
        {
            switch (type)
            {
                case ErrorType.GestorNotFound:
                    return "El gestor no existe.";
                case ErrorType.GerenteNotFound:
                    return "El gerente no existe.";
                default:
                    return "";
            }
        }
    }
}
