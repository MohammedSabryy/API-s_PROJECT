using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities.OrderEntities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderPaymentStatus
    {
        Pending = 0,
        PaymentRecived = 1,
        PaymentFailed = 2
    }
}
