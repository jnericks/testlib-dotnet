TestLib.NET
================================

[![Build status](https://ci.appveyor.com/api/projects/status/h9dbwikk0ca8chb3?svg=true)](https://ci.appveyor.com/project/jnericks/testlib-dotnet)
[![NuGet](http://img.shields.io/nuget/v/testlib-dotnet.svg)](https://www.nuget.org/packages/testlib-dotnet/)

Dependencies
--------------------------------
* [FluentAssertions](http://www.fluentassertions.com/)
* [NSubstitute](http://nsubstitute.github.io/)
* [xUnit](https://xunit.github.io/)

What is TestLib.NET?
--------------------------------
TestLib.NET is the base package that I personally include in all my .NET unit test projects. I have tried many testing frameworks and always come back to xUnit because it is consistently stable, actively developed and is the easiest to grasp for junior engineers when learning to write testable code (utilizing TDD or not). Although we use xUnit, I wanted to be able to adopt a BDD style of testing where we can so this led to the `SystemUnderTestFactory` that will auto-generate an object pre-filled with mocks for your constructor based dependencies. This [blog post](http://blog.ploeh.dk/2009/02/13/SUTFactory/) by Mark Seeman explains the benefits for a SUTFactory, basically it allows your tests to be resilient to changes in the signatures of the constructors of the objects they are testing.
