using System;
using System.Collections.Generic;
using System.Text;

namespace Quicker.Service.Configuration
{
    public class QuickerExceptionConstants
    {
        private static readonly string _Prepend = "__QUICKER__";
        public static readonly string Key = _Prepend + "KEY";
        public static readonly string Number = _Prepend +"NUMBER";
        public static readonly string Page = _Prepend + "PAGE";
        public static readonly string Conditions = _Prepend + "CONDITIONS";
        public static readonly string Precondition = _Prepend + "PRECONDITION";
        public static readonly string Entities = _Prepend + "ENTITIES";
    }
}
