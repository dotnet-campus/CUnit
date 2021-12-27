using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSTest.Extensions;
using MSTest.Extensions.Contracts;

// ReSharper disable InconsistentNaming

namespace dotnetCampus.UITest.WPF
{
    /// <summary>
    /// A contract based test case that is discovered from a unit test method.
    /// </summary>
    [PublicAPI]
    public class UIContractTestCaseAttribute: ContractTestCaseAttribute
    {

    }
}
