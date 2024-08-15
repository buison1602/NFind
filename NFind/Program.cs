using System.Diagnostics;

namespace NFind
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) {
                Console.WriteLine("FIND: Parameter format not correct");
                return;
            } 

            var findOptions = BuildOptions(args);

            if (findOptions.HelpMode)
            {
                Console.WriteLine(@"Searches for a text string in a file or files.

                    FIND [/V] [/C] [/N] [/I] [/OFF[LINE]] ""string"" [[drive:][path]filename[ ...]]

                      /V         Displays all lines NOT containing the specified string.
                      /C         Displays only the count of lines containing the string.
                      /N         Displays line numbers with the displayed lines.
                      /I         Ignores the case of characters when searching for the string.
                      /OFF[LINE] Do not skip files with offline attribute set.
                      ""string""   Specifies the text string to find.
                      [drive:][path]filename
                                 Specifies a file or files to search.

                    If a path is not specified, FIND searches the text typed at the prompt
                    or piped from another command.");
                return;
            }

            // Trước tiên có 2 vấn đề:
            //      - Thứ nhất: Nếu đầu vào là 1 đường dẫn mà nó có sử dụng dấu * thì nó có thể là 'nhiều' file. Trong khi ta chỉ 
            //      return ra 1 file, vậy nên ta cần làm sao để có thể trả về được nhiều file cùng lúc 
            //          --> Giải pháp: Cách đơn giản nhất là ta dùng 1 cái Array 
            //
            //      - Thứ hai: Nhìn vào dòng code bên dưới thấy khá rắc rối và nếu như sau này ta có thêm dữ liệu từ 1 nguồn khác 
            //      nữa thì ta phải sửa lại chương trình chính 
            //          --> Giải pháp: Sử dụng 'factory pattern' để code dễ dàng và dễ mở rộng hơn

            // Dòng code chưa tối ưu
            // var source = string.IsNullOrEmpty(findOptions.Path) ? [new ConsoleLineSource()] : [new FileLineSource(findOptions.Path)]; 

            // sources là 1 mảng gồm các intance của FileLineSource
            var sources = LineSourceFactory.CreateInstance(findOptions.Path, findOptions.SkipOfflineFile);

            foreach (var source in sources) 
            {
                ProcessSource(source, findOptions); 
            }
            
            Console.ReadKey();
        }

        private static void ProcessSource(ILineSource source, FindOptions findOptions)
        {
            // Ordinal là phân biệt chữ hoa chữ thường 
            // OrdinalIgnoreCase là không phân biệt chữ hoa chữ thường
            var stringComparison = findOptions.IsCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            source = new FilteredLineSource(source,
                (line) => findOptions.FindDontConstain ? !line.Text.Contains(findOptions.StringToFind, stringComparison) : line.Text.Contains(findOptions.StringToFind, stringComparison)
                );

            Console.WriteLine($"--------- {source.Name.ToUpper()}");

            // Lần lượt đọc từng dòng ở trong file và kiểm tra điều kiện với từng dòng được đọc
            try
            {
                source.Open();
                var line = source.ReadLine();

                while (line != null) 
                {
                    Print(line, findOptions.ShowLineNumber);

                    line = source.ReadLine();
                }
            } 
            finally
            {
                source.Close();
            }

        }

        // In ra từng dòng thỏa mãn điều kiện có/không có số dòng
        private static void Print(Line line, bool printLineNumber)
        {
            if (printLineNumber) 
            {
                Console.WriteLine($" [{line.LineNumber}] {line.Text}");
            }
            else
            {
                Console.WriteLine(line.Text);
            }

        }

        // Hàm này chỉ dùng để xử lý các tham số - Điều kiện 
        public static FindOptions BuildOptions(string[] args) {
            var options = new FindOptions();

            foreach (var arg in args) {
                if (arg == "/v") 
                {
                    options.FindDontConstain = true;
                }
                else if (arg == "/c")
                {
                    options.CountMode = true;
                }
                else if (arg == "/n")
                {
                    options.ShowLineNumber = true;
                }
                else if (arg == "/i")
                {
                    options.IsCaseSensitive = false;
                }
                else if (arg == "/off" || arg == "offline")
                {
                    options.SkipOfflineFile = false;
                }
                else if (arg == "/?")
                {
                    options.HelpMode = true;
                }
                else 
                {
                    if (string.IsNullOrEmpty(options.StringToFind))
                    {
                        options.StringToFind = arg;
                    }
                    else if (string.IsNullOrEmpty(options.Path))
                    {
                        options.Path = arg;
                    }
                    else
                    {
                        throw new ArgumentException(arg);
                    }
                }
            }

            return options;
        }
    }
}

/*
 
 
 */
