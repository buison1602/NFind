using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFind
{
    // Đây là decorator design pattern
    internal class FilteredLineSource : ILineSource
    {
        private readonly ILineSource parent;
        private readonly Func<Line, bool>? f;

        public FilteredLineSource(ILineSource parent, Func<Line, bool>? f = null)
        {
            // Toán tử ?? kiểm tra xem parent có bằng null hay không 
            // Nếu không thì trả về parent 
            // Nếu có thì trả ra ngoại lệ ở bên phải
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
            this.f = f;
        }

        public string Name => parent.Name;

        public void Close()
        {
            parent.Close();
        }

        public void Open()
        {
            parent.Open();
        }

        public Line? ReadLine()
        {
            if (f == null) 
            {
                return parent.ReadLine();
            }

            var line = parent.ReadLine();

            if (line == null) 
            {
                return line;
            }
            else
            {
                while (line != null && f(line) == false) 
                {
                    line = parent.ReadLine();
                }

                return line;
            }
        }
    }
}
