[English][en]|[日本語][jp]|[简体中文][zh-chs]|[繁體中文][zh-cht]
-|-|-|-

[en]: /README.md
[jp]: /docs/jp/README.jp.md
[zh-chs]: /docs/zh-chs/README.zh-chs.md
[zh-cht]: /docs/zh-cht/README.zh-cht.md

# MSTestEnhancer

命名は非常に難しいと思いませんか？ 特にユニットテストメソッドの命名は？ 詳細な命名についてはこの記事をお読みください。

[あなたが良いシソーラスを持っていない場合、プログラミングに参加しないでください - ITworld](https://www.itworld.com/article/2833265/cloud-computing/don-t-go-into-programming-if-you-don-t-have-a-good-thesaurus.html)。

MSTestEnhancerは、メソッドの名前を付けずに単体テストを記述するのに役立ちます。

MSTestEnhancerは、MSTestv2の契約スタイルの単体テスト拡張です。 単体テストを書くときに、混乱しているテストメソッド名を記述するのではなく、メソッド契約の記述を書くことができます。

## 入門

次のようにユニットテストを書くことができます：

```csharp
[TestClass]
public class DemoTest
{
    [ContractTestCase]
    public void Foo()
    {
        "A条件が満たされると、Xのことが起こるはずです。".Test(() =>
        {
            // Arrange
            // Action
            // Assert
        });

        "しかし、あなたがB条件を満たすとき、Y事が起こるはずです。".Test(() =>
        {
            // Arrange
            // Action
            // Assert
        });
    }
}
```

次に、エクスプローラウィンドウのテストでこのようなテスト結果が表示されます。

![ユニットテスト結果](/docs/images/unit-test-result-of-demo.jp.png)
