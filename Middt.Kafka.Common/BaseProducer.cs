using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middt.Kafka.Common
{
    public class BaseProducer<T>
    {

        readonly string? _host;
        readonly int _port;
        readonly string? _topic;

          KafkaSettings kafkaSettings;

        public BaseProducer(KafkaSettings _kafkaSettings)
        {
            kafkaSettings = _kafkaSettings;
        }

        ProducerConfig GetProducerConfig()
        {
            return new ProducerConfig
            {
                BootstrapServers = kafkaSettings.BootstrapServers,         
               // SecurityProtocol = Confluent.Kafka.SecurityProtocol.Ssl,
               // SslCaLocation = kafkaSettings.SslCaLocation
            };
        }

        public async Task ProduceAsync(T data)
        {
            using (var producer = new ProducerBuilder<Null, T>(GetProducerConfig())
                                                 .SetValueSerializer(new JsonValueSerializer<T>())
                                                 .Build())
            {
                await producer.ProduceAsync(_topic, new Message<Null, T> { Value = data });
                Console.WriteLine($"{data} published");
            }
        }
    }
}