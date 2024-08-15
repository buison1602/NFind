using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFind
{
    internal class LineSourceFactory
    {
        public static ILineSource[] CreateInstance(string path, bool skipOffLineFiles)
        {
            if (string.IsNullOrEmpty(path))
            {
                return [new ConsoleLineSource()];
            }
            else
            {
                string pattern;

                // Có thể viết như này
                //      int idx = path.LastIndexOf("\\");
                // Tuy nhiên khi ta viết chương trình cho nhiều hệ điều hành khác nhau thì nên dùng lệnh dưới
                // Bởi vì ký tự phân cấp đường dẫn ở trên các hệ điều hành khác nhau 
                int idx = path.LastIndexOf(Path.PathSeparator);
                if (idx < 0)
                {
                    pattern = path;
                    path = ".";
                }
                else
                {
                    // 2 cách viết có tác dụng như nhau
                    //pattern = path.Substring(idx + 1);
                    //path = path.Substring(0, idx);
                    pattern = path[(idx + 1)..];
                    path = path[..idx];
                }

                var dir = new DirectoryInfo(path);
                if (dir.Exists) {
                    // Trả ra 1 mảng gồm các file từ thư mục hiện tại 
                    var files = dir.GetFiles(pattern);
                    // Nếu skipOffLineFiles == true tức là ta bỏ đi những file có thuộc tính 'Offline' 
                    if (skipOffLineFiles) 
                    {
                        // Dùng LINQ để chọn ra những file mà nó 'KHÔNG CÓ' thuộc tính 'Offline'
                        files = files.Where(f => !f.Attributes.HasFlag(FileAttributes.Offline)).ToArray();
                    }

                    // Lấy ra 1 mảng gồm full path của các file vừa tìm được
                    return files.Select(f => new FileLineSource(f.FullName, f.Name)).ToArray();
                }
            }

            return [];
        }
    }
}
