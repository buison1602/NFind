using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFind
{
    // Class này là lệnh find được nhập vào - tìm kiếm 1 chuỗi string ở trong các file
    public class FindOptions
    {
        public string StringToFind {  get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public bool IsCaseSensitive { get; set; } = true;
        public bool FindDontConstain { get; set; } = false;
        public bool CountMode { get; set; } = false ;
        public bool ShowLineNumber { get; set; } = false;
        public bool SkipOfflineFile { get; set; } = true;
        public bool HelpMode { get; set; } = false;

    }
}
