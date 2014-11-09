using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Motley_Vis
{
    public class DataRowProvider : IDisposable
    {
        private LruCache<int, List<String>> cache;
        private readonly char[] seperationChars;
        private FileStream dataSource;
        private List<long> indexList;

        /// <summary>
        /// Given a file and separator list, provide a list of non-empty lines.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="separators"></param>
        public DataRowProvider(string fileName, char[] separators)
        {
            cache = new LruCache<int, List<string>>(1000);
            dataSource = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            indexList = new List<long>();
            seperationChars = separators;

            IndexAndInitialCache();
        }

        private void IndexAndInitialCache()
        {
            using (var stream = new StreamReader(dataSource))
            {
                string line;
                long prev_position = dataSource.Seek(0, SeekOrigin.Begin);
                int row_index = 0;
                while ((line = stream.ReadLine()) != null)
                {
                    // fill cache
                    if (cache.Capacity > cache.Count)
                    {
                        var tempList = line.Split(seperationChars).ToList();
                        if (tempList.Count > 0)
                        {
                            cache[row_index] = tempList;
                        }
                    }

                    indexList.Add(prev_position);
                    prev_position = dataSource.Position;
                    row_index++;
                }
            }
        }

        private List<String> Get(int index)
        {
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
            using (var stream = new StreamReader(dataSource))
            {
                dataSource.Seek(indexList[index], SeekOrigin.Begin);
                return stream.ReadLine();
            }
        }

        public List<string> this[int index]
        {
            get { return this.Get(index); }
        }

        public int Count
        {
            get { return indexList.Count; }
        }

        public void Dispose()
        {
            dataSource.Close();
        }
    }
}
