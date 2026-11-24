namespace MCP_Project.Models.Spreadsheet
{
    /// <summary>
    /// Comparison operators supported by targeted spreadsheet search.
    /// </summary>
    public enum SpreadsheetSearchOperator
    {
        /// <summary>
        /// Matches when the candidate contains the search value.
        /// </summary>
        Contains,

        /// <summary>
        /// Matches when the candidate equals the search value.
        /// </summary>
        Equals,

        /// <summary>
        /// Matches when the candidate starts with the search value.
        /// </summary>
        StartsWith,

        /// <summary>
        /// Matches when the candidate ends with the search value.
        /// </summary>
        EndsWith
    }
}
