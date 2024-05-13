using CloudLiquid.ContentFactory;
using DotLiquid;
using System.Text.Json;

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
            var requestJson = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(contents,  new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                AllowTrailingCommas = true,
                Converters = {new DictionaryStringObjectJsonConverter()}
            });
            context.Scopes.Last()[this.To] = requestJson;
        }

        #endregion
    }
}