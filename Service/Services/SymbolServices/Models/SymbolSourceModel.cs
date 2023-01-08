namespace Service.Services.SymbolServices.Models
{
    public class SymbolSourceModel
    {
        public string Symbol { get; set; }
        public string SymbolName { get; set; }
        public string SourceName { get; set; }
        public string SourceApiUrl { get; set; }
        public string SourceApiKey { get; set; }
        public bool IsSandbox { get; set; }
    }
}
