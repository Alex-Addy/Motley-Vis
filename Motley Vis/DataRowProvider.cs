using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Motley_Vis
{
    public class DataRowProvider : IDisposable
    {
        private readonly LruCache<int, List<String>> cache;
        private readonly char[] seperationChars;
        private readonly FileStream dataSource;

        /// <summary>
        /// Given a file and separator list, provide a list of non-empty lines.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="separators"></param>
        public DataRowProvider(string fileName, char[] separators)
        {
            // TODO: cache pages instead of individual rows
            cache = new LruCache<int, List<string>>(100000);
            dataSource = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            seperationChars = separators;

            InitializeCache();
        }

        /// <summary>
        /// Fill cache and determine total number of rows in file.
        /// 
        /// Should only be called once on object creation.
        /// </summary>
        private void InitializeCache()
        {
            using (var stream = new StreamReader(dataSource))
            {
                string line;
                dataSource.Seek(0, SeekOrigin.Begin);
                int rowIndex = 0;
                while ((line = stream.ReadLine()) != null)
                {
                    // fill cache
                    if (cache.Capacity > cache.Count)
                    {
                        var tempList = line.Split(seperationChars).ToList();
                        if (tempList.Count > 0)
                        {
                            cache[rowIndex] = tempList;
                        }
                    }

                    rowIndex++;
                }
                Count = rowIndex + 1;
            }
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
            return File.ReadLines(dataSource.Name).Skip(index - 1).Take(1).First();
        }

        public List<string> this[int index]
        {
            get { return this.Get(index); }
        }

        /// <summary>
        /// The number of rows contained in the file.
        /// </summary>
        public int Count { get; private set; }

        public void Dispose()
        {
            dataSource.Close();
        }
    }
}
