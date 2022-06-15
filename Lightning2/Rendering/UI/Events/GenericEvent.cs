namespace LightningGL
{
    /// <summary>
    /// GenericEvent
    /// 
    /// June 15, 2022
    /// 
    /// Defines a LightningGL event that has no parameters.
    /// This is to reduce duplicate event classes and therefore the size and cleanliness of the binary and is not an event in its own right.
    /// </summary>
    public delegate void GenericEvent
    (
    );
}