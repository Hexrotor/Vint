using EmbedIO;
using EmbedIO.WebApi;
using Newtonsoft.Json;
using Vint.Core.Server.Common.Serialization;

namespace Vint.Core.Server.Common.Attributes.Deserialization;

[AttributeUsage(AttributeTargets.Parameter)]
public class FromBodyAsJsonAttribute : Attribute, IRequestDataAttribute<WebApiController> {
    static JsonSerializerSettings Settings { get; } = new() {
        MissingMemberHandling = MissingMemberHandling.Error,
        ContractResolver = new RequiredFieldsContractResolver(),
        DateParseHandling = DateParseHandling.DateTimeOffset
    };

    public async Task<object?> GetRequestDataAsync(WebApiController controller, Type type, string parameterName) {
        using TextReader reader = controller.HttpContext.OpenRequestText();
        string json = await reader.ReadToEndAsync();

        try {
            return JsonConvert.DeserializeObject(json, type, Settings);
        } catch (JsonSerializationException e) {
            throw HttpException.BadRequest(e.Message);
        }
    }
}
