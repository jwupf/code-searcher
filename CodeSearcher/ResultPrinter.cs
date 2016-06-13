﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSearcher
{
    internal class ResultPrinter
    {
        private String m_SearchedWord;
        private String m_FileName;

        public int NumbersToShow  { get; set; }

        internal ResultPrinter(String searchedWord, String fileName)
        {
            m_SearchedWord = searchedWord;
            m_FileName = fileName;
            NumbersToShow = 40;
        }

        
        public void PrintFileInformation()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("file: {0}", m_FileName);
            Console.ResetColor();

            var lines = File.ReadAllLines(m_FileName, Encoding.Default);

            for (int i = 0, numbersToShow = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(m_SearchedWord))
                {
                    var line = lines[i].Trim();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("line {0}: ", i);
                    Console.ResetColor();

                    //TODO handle multiple hits in one line
                    var indexOfWord = line.IndexOf(m_SearchedWord);
                    Console.Write(line.Substring(0, indexOfWord));

                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(m_SearchedWord);
                    Console.ResetColor();

                    int afterHit = indexOfWord + m_SearchedWord.Length;
                    int toEnd = line.Length - afterHit;
                    if (toEnd >= 0)
                    {
                        Console.WriteLine(line.Substring(afterHit, toEnd));
                    }
                    else
                    {
                        Console.WriteLine();
                    }

                    if (numbersToShow == NumbersToShow)
                    {
                        Console.WriteLine("... press any key to show more results ...");
                        Console.ReadKey();
                        numbersToShow = -1;
                    }
                    numbersToShow++;
                }
            }
        }
    }
}
