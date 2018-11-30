# Feature Toggles (aka Feature Flags)

[by Martin Fowler](https://martinfowler.com/articles/feature-toggles.html)

Feature Toggles (often also refered to as Feature Flags) are a powerful technique, allowing teams to modify system behavior without changing code. They fall into various usage categories, and it's important to take that categorization into account when implementing and managing toggles. Toggles introduce complexity. We can keep that complexity in check by using smart toggle implementation practices and appropriate tools to manage our toggle configuration, but we should also aim to constrain the number of toggles in our system.

## Toggle Router

A Toggle Router which can be used to dynamically control which codepath is live. There are many ways to implement a Toggle Router, varying from a simple in-memory store to a highly sophisticated distributed system with a fancy UI. For now we'll start with a very simple system:

`IToggleRouter.cs`

```csharp
interface IToggleRouter
{
    bool IsEnabled(string featureName);
    void SetFeature(string featureName, bool isEnabled);
}
```

You'll need to be able to enable or disable the Feature dynamically:

`SplinesReticulator.cs`

```csharp
public Splines[] ReticulateSplines()
{
    if (_features.IsEnabled("use-new-SR-algorithm"))
        return EnhancedSplineReticulation();
    else
        return OldFashionedSplineReticulation();
}
```

This allows automated tests to verify both sides of a toggled feature:

```csharp
[Theory]
[InlineData(true)]
[InlineData(false)]
public void It_works_correctly_with_any_algorithm(bool toggle)
{
    // given
    _toggleRouter.SetFeature("use-new-SR-algorithm", toggle);

    // when
    var result = _simulationEngine.DoSomethingWhichInvolvesSplineReticulation();

    // then
    VerifySplineReticulation(result);
}
```

## Implementation Techniques

Feature Flags seem to beget rather messy Toggle Point code, and these Toggle Points also have a tendency to proliferate throughout a codebase. It's important to keep this tendency in check for any feature flags in your codebase, and critically important if the flag will be long-lived. There are a few implementation patterns and practices which help to reduce this issue.

### Layer of Indirection

We can create a new toggle router based on some default configuration - perhaps read in from a config file - but we can also dynamically toggle a feature on or off.

In software design we can often solve these coupling issues by applying Inversion of Control. This is true in this case.

Happily, [any problem in software can be solved by adding a layer of indirection](https://en.wikipedia.org/wiki/Fundamental_theorem_of_software_engineering). We can decouple a toggling decision point from the logic behind that decision like so:

`FeatureDecisions.cs`

```csharp
public class FeatureDecisions
{
    private readonly ToggleRouter _features;

    public FeatureDecisions(IToggleRouter features)
    {
        _features = features;
    }

    public bool IncludeOrderCancellationInEmail()
        => _features.IsEnabled("next-gen-ecomm");
}
```

Here's how we might decouple our invoice emailer from our feature flagging infrastructure:

`InvoiceEmailer.cs`

```csharp
public Email GenerateInvoiceEmail()
{
    var baseEmail = BuildEmailForInvoice(this.Invoice);

    if (_featureDecisions.IncludeOrderCancellationInEmail())
        return AddOrderCancellationInEmail(baseEmail);
    else
        return baseEmail;
}
```

We've introduced a `FeatureDecisions` object, which acts as a collection point for any feature toggle decision logic. We create a decision method on this object for each specific toggling decision in our code - in this case "should we include order cancellation functionality in our invoice email" is represented by the `IncludeOrderCancellationInEmail` decision method.

```csharp
[Theory]
[InlineData(true)]
[InlineData(false)]
public void It_works_correctly_with_any_algorithm(bool toggle)
{
    // given
    _toggleRouter.SetFeature("next-gen-ecomm", toggle);

    // when
    var email = _invoiceEmailer.GenerateInvoiceEmail();

    // then
    VerifyInvoiceEmail(email);
}
```

### Inversion of Decision

In software design we can often solve these coupling issues by applying Inversion of Control. This is true in this case. Here's how we might decouple our invoice emailer from our feature flagging infrastructure:

`InvoiceEmailer.cs`

```csharp
public Email GenerateInvoiceEmail()
{
    var baseEmail = BuildEmailForInvoice(_invoice);

    if (_config.AddOrderCancellationContentToEmail)
        return AddOrderCancellationInEmail(baseEmail);
    else
        return baseEmail;
}
```

`FeatureAwareFactory.cs`

```csharp
public InvoiceEmailer CreateInvoiceEmailer(Invoice invoice)
    => new InvoiceEmailer(invoice, new InvoiceEmailerConfig
    {
        AddOrderCancellationContentToEmail = _featureDecisions.IncludeOrderCancellationInEmail()
    });
```

This also makes testing `InvoiceEmailler`'s behavior easier - we can test the way that it generates emails both with and without order cancellation content just by passing a different configuration option during test:

```csharp
[Theory]
[InlineData(true, true)]
[InlineData(false, false)]
public void It_works_correctly_with_any_algorithm(bool toggle, bool anyOrderCancellationContent)
{
    // given
    _toggleRouter.SetFeature("next-gen-ecomm", toggle);
    var factory = FeatureAwareFactory.CreateFeatureAwareFactoryBasedOn(_featureDecisions);
    var emailer = factory.CreateInvoiceEmailer(new Invoice());

    // when
    var email = emailer.GenerateInvoiceEmail();

    // then
    VerifyEmailContent(email, anyOrderCancellationContent);
}
```

### Avoiding Conditional

A more maintainable alternative is to implement alternative codepaths using some sort of Strategy pattern:

`InvoiceEmailer.cs`

```csharp
public Email GenerateInvoiceEmail(Invoice invoice)
{
    var baseEmail = BuildEmailForInvoice(invoice);
    return _additionalContentEnhancer(baseEmail);
}
```

`FeatureAwareFactory.cs`

```csharp
public InvoiceEmailer CreateInvoiceEmailer()
{
    Func<Email, Email> identifyFn = x => x;

    return _featureDecisions.IncludeOrderCancellationInEmail()
        ? new InvoiceEmailer(InvoiceEmailer.AddOrderCancellationInEmail)
        : new InvoiceEmailer(identifyFn);
}
```

Here we're applying a Strategy pattern by allowing our invoice emailer to be configured with a content enhancement function. FeatureAwareFactory selects a strategy when creating the invoice emailer, guided by its FeatureDecision. If order cancellation should be in the email it passes in an enhancer function which adds that content to the email. Otherwise it passes in an identityFn enhancer - one which has no effect and simply passes the email back without modifications.

```csharp
[Theory]
[InlineData(true, true)]
[InlineData(false, false)]
public void It_works_correctly_with_any_algorithm(bool toggle, bool anyOrderCancellationContent)
{
    // given
    _toggleRouter.SetFeature("next-gen-ecomm", toggle);
    var factory = FeatureAwareFactory.CreateFeatureAwareFactoryBasedOn(_featureDecisions);
    var emailer = factory.CreateInvoiceEmailer(new Invoice());

    // when
    var email = emailer.GenerateInvoiceEmail();

    // then
    VerifyEmailContent(email, anyOrderCancellationContent);
}
```