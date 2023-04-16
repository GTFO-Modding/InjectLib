using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InjectLib.JsonNETInjection.Supports;
public class InjectLibJSON
{
    public static JsonSerializerOptions JsonOptions
    {
        get
        {
            var options = new JsonSerializerOptions(GTFO.API.JSON.JsonSerializer.DefaultSerializerSettingsWithLocalizedText);
            options.Converters.Add(new InjectLibConnector());
            return options;
        }
    }

    public static T Deserialize<T>(string json, params JsonConverter[] converters)
    {
        var options = JsonOptions;
        if (converters != null)
        {
            foreach (var converter in converters)
            {
                options.Converters.Add(converter);
            }
        }
        return JsonSerializer.Deserialize<T>(json, options);
    }

    public static object Deserialize(string json, Type type, params JsonConverter[] converters)
    {
        var options = JsonOptions;
        if (converters != null)
        {
            foreach (var converter in converters)
            {
                options.Converters.Add(converter);
            }
        }
        return JsonSerializer.Deserialize(json, type, options);
    }

    public static string Serialize<T>(T obj, params JsonConverter[] converters)
    {
        var options = JsonOptions;
        if (converters != null)
        {
            foreach (var converter in converters)
            {
                options.Converters.Add(converter);
            }
        }
        return JsonSerializer.Serialize<T>(obj, JsonOptions);
    }

    public static string Serialize(object obj, params JsonConverter[] converters)
    {
        var options = JsonOptions;
        if (converters != null)
        {
            foreach (var converter in converters)
            {
                options.Converters.Add(converter);
            }
        }
        return JsonSerializer.Serialize(obj, JsonOptions);
    }
}
