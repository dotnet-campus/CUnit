namespace MSTest.Extensions.Contracts
{
    /// <summary>
    /// 测试的配置
    /// </summary>
    public static class ContractTestConfiguration
    {
        /// <summary>
        /// 强制采用 STA 线程执行单元测试
        /// </summary>
        public static bool MustSTAThread { set; get; }
    }
}