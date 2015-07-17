using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Rhino.Etl.Tests.Branches
{
    public class BranchingOperationFixture
    {
        [Fact]
        public void TheOldBranchingOperationDoesNotReportErrors()
        {
            using (var process = new BranchingOperationProcess<BranchingOperationWithBug>())
            {
                process.Execute();
                var errors = process.GetAllErrors().Count();
                Assert.Equal(0, errors);
            }
        }

        [Fact]
        public void TheNewBranchingOperationReportsErrors()
        {
            using (var process = new BranchingOperationProcess<BranchingOperation>())
            {
                process.Execute();
                var errors = process.GetAllErrors().Count();
                Assert.NotEqual(0, errors);
            }
        }
    }
}