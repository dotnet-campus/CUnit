[English][en]|[日本語][jp]|[简体中文][zh-chs]|[繁體中文][zh-cht]
-|-|-|-

[en]: /README.md
[jp]: /README.jp.md
[zh-chs]: /README.zh-chs.md
[zh-cht]: /README.zh-cht.md

# MSTestEnhancer

Don't you think that naming is very very hard? Especially naming for unit test method? Read this article for more data of naming: [Don’t go into programming if you don’t have a good thesaurus - ITworld](https://www.itworld.com/article/2833265/cloud-computing/don-t-go-into-programming-if-you-don-t-have-a-good-thesaurus.html).

MSTestEnhancer helps you to write unit tests without naming any method.

MSTestEnhancer is a contract-style unit test extension for MSTestv2. You can write method contract descriptions instead of writing confusing test method name when writing unit tests.

---

## Getting Started with MSTestEnhancer

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

## How to Engage, Contribute and Provide Feedback

// Constructing

### Issue Guide

// Constructing

### Contributing Guide

// Constructing

## License

MSTestEnhancer is licensed under the [MIT license](/LICENSE)
