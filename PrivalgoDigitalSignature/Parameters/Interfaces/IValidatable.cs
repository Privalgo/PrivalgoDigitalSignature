namespace PrivalgoDigitalSignature.Parameters.Interfaces;

/* The `IValidatable` interface defines a contract for classes that need to perform validation. It
declares a single method `Validate()` which is responsible for performing the validation logic.
Classes that implement this interface must provide an implementation for the `Validate()` method. */
public interface IValidatable
{
    void Validate();
}
