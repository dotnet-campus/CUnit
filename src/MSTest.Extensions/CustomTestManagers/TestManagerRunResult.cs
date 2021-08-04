using System;
using System.Collections.Generic;
using System.Text;

namespace MSTest.Extensions.CustomTestManagers
{
    /// <summary>
    /// The TestManager run result
    /// </summary>
    public class TestManagerRunResult
    {
        /// <summary>
        /// Create the TestManager run result
        /// </summary>
        /// <param name="allTestCount"></param>
        /// <param name="duration"></param>
        /// <param name="testExceptionResultList"></param>
        public TestManagerRunResult(int allTestCount, TimeSpan duration, List<TestExceptionResult> testExceptionResultList)
        {
            AllTestCount = allTestCount;
            Duration = duration;
            TestExceptionResultList = testExceptionResultList;
        }

        /// <summary>
        /// Is all test success
        /// </summary>
        public bool Success => FailTestCount == 0;

        /// <summary>
        /// The test run duration
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// The number of all test
        /// </summary>
        public int AllTestCount { get; }

        /// <summary>
        /// The number of the fail test
        /// </summary>
        public int FailTestCount => TestExceptionResultList.Count;

        /// <summary>
        /// The number of the success test
        /// </summary>
        public int SuccessTestCount => AllTestCount - FailTestCount;

        /// <summary>
        /// The test fail exception list
        /// </summary>
        public List<TestExceptionResult> TestExceptionResultList { get; }

        /// <inheritdoc />
        [NotNull]
        public override string ToString()
        {
            if (Success)
            {
                return
                    $"Passed! - Failed:{FailTestCount,8}，Passed:{SuccessTestCount,8}，Skipped:     0，Total:{AllTestCount,8}，Duration: {Duration.TotalSeconds:0.00} s";
            }
            else
            {
                var stringBuilder = new StringBuilder();
                foreach (var exception in TestExceptionResultList)
                {
                    stringBuilder.AppendLine($"Failed {exception.DisplayName}");
                    stringBuilder.AppendLine($"Message:");
                    stringBuilder.AppendLine(exception.Exception.ToString());
                    stringBuilder.AppendLine();
                }

                stringBuilder.AppendLine($"Failed! - Failed:{FailTestCount,8}，Passed:{SuccessTestCount,8}，Skipped:     0，Total:{AllTestCount,8}，Duration: {Duration.TotalSeconds:0.00} s");
                return stringBuilder.ToString();
            }
        }
    }
}