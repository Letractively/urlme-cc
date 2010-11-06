using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace urlme.Core.Diagnostics
{
    public class DebugWriter : TextWriter
    {
        private readonly int level;

        private readonly string category;

        private static UnicodeEncoding encoding;

        private bool isOpen;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugWriter"/> class.
        /// </summary>
        public DebugWriter()
            : this(0, Debugger.DefaultCategory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugWriter"/> class with the specified level and category.
        /// </summary>
        /// <param name="level">A description of the importance of the messages.</param>
        /// <param name="category">The category of the messages.</param>
        public DebugWriter(int level, string category)
            : this(level, category, CultureInfo.CurrentCulture)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugWriter"/> class with the specified level, category and format provider.
        /// </summary>
        /// <param name="level">A description of the importance of the messages.</param>
        /// <param name="category">The category of the messages.</param>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> object that controls formatting.</param>
        public DebugWriter(int level, string category, IFormatProvider formatProvider)
            : base(formatProvider)
        {
            this.level = level;
            this.category = category;
            this.isOpen = true;
        }

        public int Level
        {
            get { return this.level; }
        }

        public string Category
        {
            get { return this.category; }
        }

        public override Encoding Encoding
        {
            get
            {
                if (encoding == null)
                {
                    encoding = new UnicodeEncoding(false, false);
                }

                return encoding;
            }
        }

        public override void Write(char value)
        {
            if (!this.isOpen)
            {
                throw new ObjectDisposedException(null);
            }

            Debugger.Log(this.level, this.category, value.ToString());
        }

        public override void Write(string value)
        {
            if (!this.isOpen)
            {
                throw new ObjectDisposedException(null);
            }

            if (value != null)
            {
                Debugger.Log(this.level, this.category, value);
            }
        }

        public override void Write(char[] buffer, int index, int count)
        {
            if (!this.isOpen)
            {
                throw new ObjectDisposedException(null);
            }

            if (buffer == null || index < 0 || count < 0 || buffer.Length - index < count)
            {
                base.Write(buffer, index, count); // delegate throw exception to base class
            }

            Debugger.Log(this.level, this.category, new string(buffer, index, count));
        }

        protected override void Dispose(bool disposing)
        {
            this.isOpen = false;
            base.Dispose(disposing);
        }
    }
}
