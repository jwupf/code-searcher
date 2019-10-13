﻿using System.IO;
using CodeSearcher.Interfaces;

namespace CodeSearcher.BusinessLogic.Exporter
{
    internal sealed class ResultFileExporter : IResultExporter
    {
        private readonly StreamWriter m_ExportWriter;

        public ResultFileExporter(StreamWriter exportWriter)
        {
            m_ExportWriter = exportWriter;
        }

        public void Export(ISearchResultContainer searchResultContainer, string searchedWord)
        {
            foreach (var result in searchResultContainer)
            {
                m_ExportWriter.WriteLine(result.FileName);
                var lines = File.ReadAllLines(result.FileName);

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains(searchedWord))
                    {
                        m_ExportWriter.WriteLine($"{i + 1};{lines[i]}");
                    }
                }
            }
        }

        public void Dispose()
        {
            m_ExportWriter?.Close();
        }
    }
}