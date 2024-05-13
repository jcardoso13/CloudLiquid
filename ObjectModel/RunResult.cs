namespace CloudLiquid.ObjectModel
{
    public class RunResult
    {
        #region Public Properties

        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public string ErrorAction { get; set; }

        public string Output { get; set; }

        #endregion
    }
}
