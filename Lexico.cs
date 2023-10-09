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
        //  WS,L,D,.,E,=;,&,|,!,<,>,+,-,%,*,/,?,“,{,},EOF,EOL,LMD
            {0,1,2,27,1,8,10,11,12,13,16,17,19,20,22,22,28,24,25,32,33,F,0,27},
            {F,1,1,F,1,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,3,3,5,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {E,E,4,E,E,E,E,E,E,E,E,E,E,E,E,E,E,E,E,E,E,E,E,E},
            {F,F,4,F,5,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {E,E,7,E,E,F,F,F,F,F,F,F,6,6,F,F,F,F,F,F,F,F,F,E},
            {E,E,7,E,E,F,F,F,F,F,F,F,E,E,F,F,F,F,F,F,F,F,F,E},
            {F,F,7,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,9,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,14,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,14,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,15,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,18,F,F,F,F,F,18,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,18,F,F,F,F,F,18,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,21,F,F,F,F,F,F,21,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,21,F,F,F,F,F,F,F,21,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,23,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,25,26,25,25,E,25,25},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,23,F,F,F,F,F,F,F,F,F,30,29,F,F,F,F,F,F,F},
            {29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,29,0,29},
            {30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,31,30,30,30,30,30,E,30,30},
            {30,30,30,30,30,30,30,30,30,30,30,30,30,30,30,31,0,30,30,30,30,E,30,30},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},
            {F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F,F},

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
        private void clasificar(int estado)
        {
            switch (estado)
            {
                case 1: setClasificacion(Tipos.Identificador); break;
                case 2: setClasificacion(Tipos.Numero); break;
                case 8: setClasificacion(Tipos.Caracter); break;
            }
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
                clasificar(estado);
                
                if (estado >= 0)
                {
                    if (estado > 0)
                    {
                        buffer += c;    
                    }
                    archivo.Read();
                }
            }
            if (estado == E)
            {
                throw new Error("Lexico: Se espera un digito",log);
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