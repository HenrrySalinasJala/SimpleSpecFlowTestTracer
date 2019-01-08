namespace SimpleExecutionEngine.SpecFlowPlugin.TraceClient
{
    public class Step
    {
        public string Keyword { get; set; }
        public string Text { get; set; }
        public string MultilineText { get; set; }
        public string Arguments { get; set; }
        public string[][] Table { get; set; }
        public string State { get; set; }
        public string Duration { get; set; }
    }
}
