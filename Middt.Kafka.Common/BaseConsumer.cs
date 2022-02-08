using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middt.Kafka.Common
{
    public abstract class BaseConsumer<T>
    {
        public Action<T>? OnConsume { get; set; }
        KafkaSettings kafkaSettings;

        public BaseConsumer(KafkaSettings _kafkaSettings)
        {
            kafkaSettings = _kafkaSettings;
        }

        ConsumerConfig GetConsumerConfig()
        {
            return new ConsumerConfig
            {
                BootstrapServers = kafkaSettings.BootstrapServers,
                GroupId = kafkaSettings.GroupId,              
                SecurityProtocol = Confluent.Kafka.SecurityProtocol.Ssl,
                SslCaLocation = kafkaSettings.SslCaLocation,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }

        public async Task ConsumeAsync()
        {
            using (var consumer = new ConsumerBuilder<Null, T>(GetConsumerConfig())
                .SetValueDeserializer(new JsonValueDeserializer<T>())
                .Build())
            {
                consumer.Subscribe(kafkaSettings.Topic);

                Console.WriteLine($"Subscribed to {kafkaSettings.Topic}");


                await Task.Run(() =>
                {
                    while (true)
                    {
                        var consumeResult = consumer.Consume(default(CancellationToken));

                        if (consumeResult.Message is Message<Null, T> result)
                        {
                            Process(result.Value);
                            OnConsume?.Invoke(result.Value);
                           
                        }
                    }
                });

                consumer.Close();
            }
        }

        public abstract Task Process(T model);
    }
}