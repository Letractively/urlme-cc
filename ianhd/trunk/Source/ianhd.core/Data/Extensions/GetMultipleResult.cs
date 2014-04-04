using System.Collections.Generic;

namespace ianhd.core.Data.Extensions
{
    public interface IMultipleResultReader
    {
        IEnumerable<T> Read<T>();
    }

    public class GridReaderResultReader : IMultipleResultReader
    {
        private readonly Dapper.GridReader _reader;

        public GridReaderResultReader(Dapper.GridReader reader)
        {
            _reader = reader;
        }

        public IEnumerable<T> Read<T>()
        {
            return _reader.Read<T>();
        }
    }

    public class SequenceReaderResultReader : IMultipleResultReader
    {
        private readonly Queue<Dapper.GridReader> _items;

        public SequenceReaderResultReader(IEnumerable<Dapper.GridReader> items)
        {
            _items = new Queue<Dapper.GridReader>(items);
        }

        public IEnumerable<T> Read<T>()
        {
            Dapper.GridReader reader = _items.Dequeue();
            return reader.Read<T>();
        }
    }
}