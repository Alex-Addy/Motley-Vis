﻿// Copyright (c) 2015 Alexander Addy
// Released under MIT license,
// view License.txt in root of project for full text
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Motley_Vis
{
    /// <summary>
    /// Class handles files too large to fit into memory, using LruCache
    /// </summary>
    public class DataRowProvider : IDisposable
    {
        private readonly List<String> headerList;
        private readonly LruCache<int, List<String>> cache;
        private readonly char[] seperationChars;
        private readonly FileStream dataSource;
        private const int CacheSize = 100000;

        /// <summary>
        /// Given a file and separator list, provide a list of non-empty lines.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="separators"></param>
        public DataRowProvider(string fileName, char[] separators)
        {
            // TODO: cache pages instead of individual rows
            cache = new LruCache<int, List<string>>(CacheSize);
            // TODO: deal with file open failure
            dataSource = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            seperationChars = separators;

            // Assume file has headers
            headerList = File.ReadLines(dataSource.Name).Take(1).First().Split(seperationChars).ToList();
            Headers = headerList;
            FileName = fileName;

            InitializeCache();
        }

        /// <summary>
        /// Fill cache and determine total number of rows in file.
        /// 
        /// Should only be called once on object creation.
        /// </summary>
        private void InitializeCache()
        {
            // TODO: add indexing
            // possible indexing implementation:
            //  scan file directly and look for newline characters, storing their index+1 as the start of each line
            dataSource.Seek(0, SeekOrigin.Begin);
            int rowIndex = 0;
            foreach (var values in File.ReadLines(dataSource.Name).Skip(1). // skip header line
                Select(line => line.Split(seperationChars).ToList()).
                Where(values => values.Count != 0))
            {
                // fill cache
                if (cache.Capacity > cache.Count)
                {
                    cache[rowIndex] = values;
                }
                rowIndex++;
            }
            Count = rowIndex;
        }

        /// <summary>
        /// Gets a row from storage.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private List<String> Get(int index)
        {
            if (index < 0 || Count <= index)
            {
                throw new IndexOutOfRangeException();
            }
            if (cache.ContainsKey(index))
            {
                return cache[index];
            }
            else
            {
                var row = GetLineFromFile(index).Split(seperationChars).ToList();
                cache[index] = row;
                return row;
            }
        }

        private string GetLineFromFile(int index)
        {
            // TODO: make this not stupidly inefficient
            return File.ReadLines(dataSource.Name).Skip(index+1).Take(1).First();
        }

        public List<string> this[int index]
        {
            get { return Get(index); }
        }

        public List<String> Headers { get; private set; }

        public String FileName { get; private set; }

        /// <summary>
        /// The number of rows contained in the file.
        /// </summary>
        public int Count { get; private set; }

        public void Dispose()
        {
            dataSource.Close();
        }

        public IEnumerable<List<string>> GetEnumerable()
        {
            return File.ReadLines(dataSource.Name).Select(s => s.Split(seperationChars).ToList()).Skip(1);
        }
    }
}
