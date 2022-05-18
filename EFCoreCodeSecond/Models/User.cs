using System;
using System.Collections.Generic;

namespace EFCoreCodeSecond
{
    public partial class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Age { get; set; }
    }
}
