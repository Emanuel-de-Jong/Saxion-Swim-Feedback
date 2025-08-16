namespace Swim_Feedback.Services.PageDataService
{
    public class PageDataService
    {
        private readonly Dictionary<string, PageData> data = new();

        public void AddData<T>(PageData pageData)
        {
            data[nameof(T)] = pageData;
        }

        public PageData? GetData<T>()
        {
            string className = nameof(T);
            return data.ContainsKey(className) ? data[className] : null;
        }
    }
}
