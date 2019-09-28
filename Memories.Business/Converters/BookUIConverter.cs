using Memories.Business.Enums;
using Memories.Business.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;

namespace Memories.Business.Converters
{
    /// <summary>
    /// https://stackoverflow.com/a/30579193/7990500
    /// Json serialize abstract class
    /// </summary>
    public class BookUISpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(BookUI).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }

    public class BookUIConverter : JsonConverter
    {
        private static JsonSerializerSettings _specifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new BookUISpecifiedConcreteClassConverter() };

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BookUI);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            switch ((BookUIEnum)jo["UIType"].Value<int>())
            {
                case BookUIEnum.TextUI:
                    return JsonConvert.DeserializeObject<BookTextUI>(jo.ToString(), _specifiedSubclassConversion);
                case BookUIEnum.ImageUI:
                    return JsonConvert.DeserializeObject<BookImageUI>(jo.ToString(), _specifiedSubclassConversion);
                default:
                    throw new JsonSerializationException("Exception in BookUIConverter");
            }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        /// <summary>
        /// Read only converter
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}
