using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIView
{
    public interface IFileDialog
    {
        public string OpenFileDialog();
        public string SaveFileDialog();
    }
}
