[English][en]|[日本語][jp]|[简体中文][zh-chs]|[繁體中文][zh-cht]
-|-|-|-

[en]: /README.md
[jp]: /docs/jp/README.jp.md
[zh-chs]: /docs/zh-chs/README.zh-chs.md
[zh-cht]: /docs/zh-cht/README.zh-cht.md

![.NET Build & Test](https://github.com/dotnet-campus/CUnit/workflows/.NET%20Build%20&%20Test/badge.svg) ![NuGet Push](https://github.com/dotnet-campus/CUnit/workflows/NuGet%20Push/badge.svg) 


| Name | NuGet|
|--|--|
|MSTestEnhancer|[![](https://img.shields.io/nuget/v/MSTestEnhancer.svg)](https://www.nuget.org/packages/MSTestEnhancer)|
|dotnetCampus.UITest.WPF|[![](https://img.shields.io/nuget/v/dotnetCampus.UITest.WPF.svg)](https://www.nuget.org/packages/dotnetCampus.UITest.WPF)|

# CUnit

Don't you think that naming is very very hard? Especially naming for unit test method? Read this article for more data of naming: [Don’t go into programming if you don’t have a good thesaurus - ITworld](https://www.itworld.com/article/2833265/cloud-computing/don-t-go-into-programming-if-you-don-t-have-a-good-thesaurus.html).

CUnit (MSTestEnhancer) helps you to write unit tests without naming any method.

CUnit is a contract-style unit test extension for MSTestv2. You can write method contract descriptions instead of writing confusing test method name when writing unit tests.

---

## Getting Started with CUnit

You can write unit test like this:

```csharp
[TestClass]
public class DemoTest
{
    [ContractTestCase]
    public void Foo()
    {
        "When A happened, result A'.".Test(() =>
        {
            // Arrange
            // Action
            // Assert
        });

        "But when B happened, result B'".Test(() =>
        {
            // Arrange
            // Action
            // Assert
        });
    }
}
```

Then you'll see this kind of test result in testing explorer window:

![Unit test result](/docs/images/unit-test-result-of-demo.png)

For more usages, please visit:

- [English](/README.md)
- [日本語](/docs/jp/README.md)
- [简体中文](/docs/zh-chs/README.md)
- [繁體中文](/docs/zh-cht/README.md)

### Contributing Guide

There are many ways to contribute to MSTestEnhancer

- [Submit issues](https://github.com/dotnet-campus/MSTestEnhancer/issues) and help verify fixes as they are checked in.
- Review the [documentation changes](https://github.com/dotnet-campus/MSTestEnhancer/pulls).
- [How to Contribute](How%20to%20Contribute.md)

## License

MSTestEnhancer is licensed under the [MIT license](/LICENSE)
