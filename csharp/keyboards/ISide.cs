namespace keyboards
{
    /// <summary>
    /// Represents the side of a keyboard that can be a different color
    /// </summary>
    public interface ISide
    {
        /// <summary>
        /// The current color of the side
        /// </summary>
        Color CurrentColor { get; set; }
        
        /// <summary>
        /// Render the current color of the side to memory
        /// </summary>
        /// <param name="time">The current time</param>
        void Render(long time);
    }
}