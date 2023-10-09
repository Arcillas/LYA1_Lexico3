using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel.DataAnnotations.Schema;

namespace LYA1_Lexico3
{
    public class Lexico : Token, IDisposable
    {
        const int F = -1;
        const int E = -2;
        private StreamReader archivo;
        private StreamWriter log;

        int[,] TRAND =  
        {
        //  WS,L,D,.,E,+,-,La
            {0,1,2,8,1,8,8,8}, // 0
            {F,1,1,F,1,F,F,F}, // 1
            {F,F,2,3,5,F,F,F}, // 2
            {E,E,4,E,E,E,E,E}, // 3
            {F,F,4,F,5,F,F,F}, // 4
            {E,E,7,E,E,6,6,E}, // 5
            {E,E,7,E,E,E,E,E}, // 6
            {F,F,7,F,F,F,F,F}, // 7
            {F,F,F,F,F,F,F,F}, // 8
        };
        public Lexico()
        {
            archivo = new StreamReader("prueba.cpp");
            log = new StreamWriter("prueba.log");
            log.AutoFlush = true;
        }
        public Lexico(string nombre)
        {
            archivo = new StreamReader(nombre);
            log = new StreamWriter("prueba.log");
            log.AutoFlush = true;
        }
        public void Dispose()
        {
            archivo.Close();
            log.Close();
        }
        private int columna(char c)
        {
            if (char.IsWhiteSpace(c))
                return 0;
            else if (char.ToLower(c) == 'e')
                return 4;
            else if (char.IsLetter(c))
                return 1;
            else if (char.IsAsciiDigit(c))
                return 2;
            else if (c=='.')
                return 3;
            else if (c=='+')
                return 5;
            else if (c=='-')
                return 6;
            else
                return 7;
            
        }
        public void nextToken()
        {
            char c;
            string buffer = "";

            int estado = 0;

            while (estado >= 0)
            {
                c = (char)archivo.Peek();
                
                estado = TRAND[estado,columna(c)];

                if (estado >= 0)
                {
                    if (estado > 0)
                    {
                        buffer += c;    
                    }
                    archivo.Read();
                }
            }
            setContenido(buffer);
            log.WriteLine("[" + getContenido() + "] : " + getClasificacion());
        }
        public bool FinArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}