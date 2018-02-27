# MSTestEnhancer 风格的 ReSharper 代码片段

MSTest 有着独特的单元测试编写风格，这意味着目前并没有现成的工具提供这种风格单元测试的快速生成方案。但是你可以使用 ReSharper 手工导入我们预设好的代码片段来自动生成这种风格的代码。

## 目前提供的代码片段

目前提供了四种代码片段：

- [类](#生成单元测试文件和类)
- [方法](#生成单元测试方法)
- [测试用例](#生成单元测试用例)
- [转换](#%E8%BD%AC%E6%8D%A2%E7%8E%B0%E6%9C%89%E7%9A%84%E5%8D%95%E5%85%83%E6%B5%8B%E8%AF%95%E7%94%A8%E4%BE%8B%E5%88%B0-mstestenhancer-%E9%A3%8E%E6%A0%BC)

### 生成单元测试文件和类

如果你使用 Visual Studio（带 ReSharper 插件）开发，那么在项目上或者项目的文件夹上点击右键 → `添加` → `New from Template` → `Contract Unit Test` 即可添加一个 MSTestEnhancer 风格的单元测试类。

![文件模板](/docs/images/2018-02-27-11-11-10.png)  
▲ 根据模板创建类

![填写文件名](/docs/images/2018-02-27-11-15-09.png)  
▲ 填写文件名（正好也是类名）

添加好的类会默认选中那段契约文字，这样可以立刻开始编写或者粘贴契约。

![生成的类](/docs/images/2018-02-27-11-32-59.png)  
▲ 生成的类

### 生成单元测试方法

在类中输入 `test` 会出现 `Insert a test method` 代码片段。敲回车或者 Tab 便能够插入单元测试方法。

![Insert a test method 代码片段](/docs/images/2018-02-27-11-36-50.png)  
▲ 输入 test（并不需要完整输入，ReSharper 还是很厉害的）

![生成的方法](/docs/images/2018-02-27-11-33-49.png)  
▲ 生成的方法（命名空间会自动加入）

### 生成单元测试用例

使用 MSTestEnhancer 风格编写单元测试后，一个方法内部可以包含多个单元测试用例了。所以如果在方法内部输入 `test` 会出现 `Insert a test case` 代码片段。敲回车或者 Tab 便能够插入单元测试用例。

![Insert a test case 代码片段](/docs/images/2018-02-27-11-38-16.png)  
▲ 输入 test（同样不需要完整输入）

![生成的测试用例](/docs/images/2018-02-27-11-39-09.png)  
▲ 生成的测试用例

### 转换现有的单元测试用例到 MSTestEnhancer 风格

如果项目中已经存在传统风格的单元测试，可以使用此代码片段将测试用例转换成 MSTestEnhancer 风格。

1. 选中传统风格的测试用例。
1. 直接输入 `test`，不用担心覆盖掉刚刚选中的代码。
1. 敲回车或者 Tab 便会转换刚刚选中的代码到 MSTestEnhancer 风格。

![选中](/docs/images/2018-02-27-11-48-07.png)  
▲ 选中测试用例代码（可使用多次 Ctrl + W 快捷键快速选中）

![Convert to MSTestEnhancer test case 代码片段](/docs/images/2018-02-27-11-56-40.png)  
▲ 输入 `test` 转换测试用例

![转换的测试用例](/docs/images/2018-02-27-11-51-43.png)  
▲ 转换的测试用例

步骤 2 也可以替换成菜单操作：按下 Ctrl + Enter，选择 `Surround with...` → `test`。

![使用菜单操作](/docs/images/2018-02-27-11-50-18.png)  
▲ 使用菜单操作（当然效率会低很多）

## 将代码片段导入到我的 ReSharper

你可以在 [MSTestEnhancer/MSTest.Extensions.sln.DotSettings](/MSTest.Extensions.sln.DotSettings) 文件中找到我们预设好的 ReSharper 代码片段。不过这种格式的文件人类可读性较差，我们更推荐你使用导入的方式来使用这些代码片段。

### 如何导入到我的 ReSharper

在 ReSharper 菜单中选择 `Tools` → `Templates Explorer` 打开模板浏览器。

![Templates Explorer](/docs/images/2018-02-27-11-59-28.png)

依次选中所有种类的模板标签，点击 `导入`，选择从本仓库下载的 [MSTestEnhancer/MSTest.Extensions.sln.DotSettings](/MSTest.Extensions.sln.DotSettings) 文件即可。

![导入](/docs/images/2018-02-27-12-01-29.png)
