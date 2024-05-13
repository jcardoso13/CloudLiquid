using CloudLiquid.ContentFactory;
using DotLiquid;
using Newtonsoft.Json;

namespace CloudLiquid.Tags
{
    public class CaptureJSON : BaseCloudLiquidTag
    {
        #region Public Methods

        public override void Render(Context context, TextWriter result)
        {
            using TextWriter textWriter = new StringWriter(result.FormatProvider);
            base.Render(context, textWriter);
            string contents = textWriter.ToString();
            var requestJson = JsonConvert.DeserializeObject<IDictionary<string, object>>(contents, new DictionaryConverter());
            context.Scopes.Last()[this.To] = Hash.FromDictionary(requestJson);
        }

        #endregion
    }
}