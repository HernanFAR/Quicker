using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Quicker.Test.Repository
{
    public class TestContext : DbContext
    {
        public TestContext([NotNull] DbContextOptions options) : base(options)
        {
        }
    }
}
