﻿using CodeSearcher.BusinessLogic.Indexer;
using CodeSearcher.BusinessLogic.InternalInterfaces;
using CodeSearcher.BusinessLogic.Io;
using CodeSearcher.BusinessLogic.Searcher;
using CodeSearcher.BusinessLogic.SearchResults;
using CodeSearcher.BusinessLogic.Exporter;
using CodeSearcher.Interfaces;
using Ninject.Modules;
using CodeSearcher.BusinessLogic.Management;

namespace CodeSearcher.BusinessLogic.Ninject
{
    class CodeSearcherNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISearchResultContainer>().To(typeof(SearchResultContainer));
            Bind<ISearchResult>().To(typeof(SearchResult));
            Bind<IIndexer>().To(typeof(DefaultIndexer));
            Bind<ISearcher>().To(typeof(DefaultSearcher)).Named("Default");
            Bind<ISearcher>().To(typeof(WildcardSearcher)).Named("Wildcard");
            Bind<IFileReader>().To(typeof(FileReader));
            Bind<IResultExporter>().To<ResultFileExporter>().Named("Default");
            Bind<IResultExporter>().To<WildcardResultExporter>().Named("Wildcard");
            Bind<ICodeSearcherLogic>().To<CodeSearcherLogic>();
            Bind<ICodeSearcherManager>().To<CodeSearcherManager>();
        }
    }
}
