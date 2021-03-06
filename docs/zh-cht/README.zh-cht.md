[English][en]|[日本語][jp]|[简体中文][zh-chs]|[繁體中文][zh-cht]
-|-|-|-

[en]: /README.md
[jp]: /docs/jp/README.jp.md
[zh-chs]: /docs/zh-chs/README.zh-chs.md
[zh-cht]: /docs/zh-cht/README.zh-cht.md

# MSTestEnhancer

有沒有覺得命名太難？有沒有覺得單元測試的命名更難？沒錯，你不是一個人！看看這個你就知道了：[程序員最頭疼的事：命名](http://blog.jobbole.com/50708/#rd?sukey=fc78a68049a14bb285ac0d81ca56806ac10192f4946a780ea3f3dd630804f86056e6fcfe6fcaeddb3dc04830b7e3b3eb) 或它的英文原文 [Don't go into programming if you don't have a good thesaurus - ITworld](https://www.itworld.com/article/2833265/cloud-computing/don-t-go-into-programming-if-you-don-t-have-a-good-thesaurus.html)。

**MSTestEnhancer** 的出現將解決令你頭疼的單元測試命名問題——因為，你再也不需要為任何單元測試方法命名了！

MSTestEnhancer 是 MSTest v2 的一個擴展。使用它，你可以用契約的方式來描述一個又一個的測試用例，這些測試用例將在單元測試運行結束後顯示到單元測試控制台或 GUI 窗口中。全過程你完全不需要為任何單元測試方法進行命名——你關注的，是測試用例本身。

## 新手入門

現在，你的單元測試可以這樣寫了：

```csharp
[TestClass]
public class DemoTest
{
    [ContractTestCase]
    public void Foo()
    {
        "當滿足 A 條件時，應該發生 X 事。".Test(() =>
        {
            // Arrange
            // Action
            // Assert
        });

        "但是當滿足 B 條件時，應該發生 Y 事。".Test(() =>
        {
            // Arrange
            // Action
            // Assert
        });
    }
}
```

於是，運行單元測試將看到這樣的結果視圖：

![單元測試運行結果](/docs/images/unit-test-result-of-demo.zh-cht.png)
