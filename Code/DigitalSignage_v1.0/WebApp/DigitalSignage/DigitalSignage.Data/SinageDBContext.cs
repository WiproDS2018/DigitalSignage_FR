using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalSignage.Data.EF;

namespace DigitalSignage.Data
{
    public sealed class SinageDBManager
    {
        private SignageDBContext _context;
        private SinageDBManager()
        {
            _context = new SignageDBContext();
        }
        private static readonly Lazy<SinageDBManager> lazy = new Lazy<SinageDBManager>(() => new SinageDBManager());
        public static SignageDBContext Context
        {
            get
            {
                return lazy.Value._context;
            }
        }
    }
}
