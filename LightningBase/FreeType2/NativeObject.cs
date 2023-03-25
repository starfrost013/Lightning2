namespace LightningBase
{
    /// <summary>
    /// Provide a consistent means for using pointers as references.
    /// </summary>
    public abstract class NativeObject
    {
        private nint reference;

        /// <summary>
        /// Construct a new NativeObject and assign the reference.
        /// </summary>
        /// <param name="reference"></param>
        protected NativeObject(nint reference)
        {
            this.reference = reference;
        }

        public virtual nint Reference
        {
            get
            {
                return reference;
            }
            set
            {
                reference = value;
            }
        }
    }
}
