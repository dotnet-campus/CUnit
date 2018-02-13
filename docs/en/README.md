# MSTest Enhancer

MSTestEnhancer is a MSTest v2 extension to connect unit test and the method that should be tested. You'll find out that all unit test contracts are listed under target methods, and you can see all the result of them directly, no need to translate the obscure method name into what you want to test.

## Getting Started

1. Install [MSTestEnhancer](https://www.nuget.org/packages/MSTestEnhancer/) from the [nuget.org](https://www.nuget.org/).
1. Write unit test code in the style listed below.

## Recommended Style of Writing Unit Tests

Assuming that you want to test a class named `TheTestedClass` containing a method named `TheTestedMethod`. Then you can write unit tests like this:

```csharp
[TestClass]
public class TheTestedClassTest
{
    [ContractTestCase]
    public void TheTestedMethod()
    {
        "When Xxx happens, results in Yyy.".Test(() =>
        {
            // Write test case code here...
        });
        
        "When Zzz happens, results in Www.".Test(() =>
        {
            // Write test case code here...
        });
    }
}
```

Notice that the name of class and method are almost the name of the tested class and tested method. As a result, we don't need to think about anything about naming unit test, nor to read the obscure name of the unit test.

![Test results](/docs/images/2018-02-13-13-09-26.png)

## Unit Test with Arguments

Some unit tests need multiple values to verify the contracts, so MSTestEnhancer provides `WithArguments` method to config the arguments.

```csharp
"prime number.".Test((int num) =>
{
    // Write test case code here...
}).WithArguments(2, 3, 5, 7, 11);

"{0} is not a prime number.".Test((int num) =>
{
    // Write test case code here...
}).WithArguments(1, 4);
```

You can pass up to 8 parameters into the test case.

```csharp
"Contract 1: {0} and {1} are allowed in the contract description.".Test((int a, int b) =>
{
    // Now, a is 2 and b is 3.
}).WithArguments(2, 3);

"Contract 2".Test((int a, int b) =>
{
    // Now the test case will run twice. The first group, a is 2 and b is 3; and the second group, a is 10 and b is 20.
    // ValueTuple is supported, too.
}).WithArguments((2, 3), (10, 20));
```

In this example, the contract description will be replaced to the arguments that you have passed into.

## Async Unit Test

All `Test` extension method support async action so that you can test any async method.

## Some Fantastic Feature

Nested unit test classes are supported by MSTest v2, so you can write an infinite level unit test tree.
